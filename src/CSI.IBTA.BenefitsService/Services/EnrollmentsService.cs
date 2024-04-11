using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using System.Net;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class EnrollmentsService : IEnrollmentsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IDecodingService _decodingService;
        private readonly IUserBalanceService _userBalanceService;

        public EnrollmentsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper, IDecodingService decodingService, IUserBalanceService userBalanceService)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
            _decodingService = decodingService;
            _userBalanceService = userBalanceService;
        }

        public async Task<GenericResponse<PagedEnrollmentsResponse>> GetActiveEnrollmentsPaged(int employeeId, byte[] encodedEmployerEmployee, int page, int pageSize)
        {
            var decodedResponse = _decodingService.GetDecodedEmployerEmployee(encodedEmployerEmployee);

            if (decodedResponse.Result == null)
            {
                return new GenericResponse<PagedEnrollmentsResponse>(decodedResponse.Error, null);
            }

            if (decodedResponse.Result.employeeId != employeeId)
            {
                var error = new HttpError("Employer does not have access to view this employee enrollments", HttpStatusCode.Forbidden);
                return new GenericResponse<PagedEnrollmentsResponse>(error, null);
            }

            var now = DateTime.UtcNow;
            var filteredEnrollments = _benefitsUnitOfWork.Enrollments
                .Include(x => x.Plan)
                .Include(x => x.Plan.PlanType)
                .Where(x => x.EmployeeId == employeeId)
                .Where(x => x.Plan.Package.PlanStart < now && x.Plan.Package.PlanEnd > now)
                .Where(x => x.Plan.Package.Initialized != null);

            var totalCount = filteredEnrollments.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var enrollments = await filteredEnrollments
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var enrollmentIds = enrollments.Select(e => e.Id).Distinct().ToList();
            var balancesResponse = await _userBalanceService.GetCurrentBalances(enrollmentIds);

            if (balancesResponse.Result == null)
            {
                return new GenericResponse<PagedEnrollmentsResponse>(balancesResponse.Error, null);
            }

            List<FullEnrollmentWithBalanceDto> fullEnrollmentWithBalanceDtos = [];

            foreach (var enrollment in enrollments)
            {
                var planDto = _mapper.Map<PlanDto>(enrollment.Plan);

                var enrollmentDto = new FullEnrollmentWithBalanceDto(
                    enrollment.Id,
                    enrollment.Plan.Package.Name,
                    planDto,
                    enrollment.Election,
                    enrollment.Plan.Contribution,
                    balancesResponse.Result[enrollment.Id],
                    enrollment.EmployeeId);

                fullEnrollmentWithBalanceDtos.Add(enrollmentDto);
            }

            var response = new PagedEnrollmentsResponse(fullEnrollmentWithBalanceDtos, page, pageSize, totalPages, totalCount);
            return new GenericResponse<PagedEnrollmentsResponse>(null, response);
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> GetUsersEnrollments(List<int> userIds)
        {
            var enrollments = await _benefitsUnitOfWork.Enrollments
                .Include(e => e.Plan)
                .Where(e => userIds.Contains(e.EmployeeId))
                .ToListAsync();

            var enrollmentDtos = enrollments.Select(_mapper.Map<EnrollmentDto>).ToList();
            return new(null, enrollmentDtos);
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> GetEnrollmentsByEmployeeId(int employeeId, int employerId, byte[] encodedEmployerEmployee)
        {
            var decodedResponse = _decodingService.GetDecodedEmployerEmployee(encodedEmployerEmployee);
            if (decodedResponse.Result == null) return new GenericResponse<List<EnrollmentDto>>(decodedResponse.Error, null);

            if (decodedResponse.Result.employerId != employerId || decodedResponse.Result.employeeId != employeeId)
                return new GenericResponse<List<EnrollmentDto>>(new HttpError("Employer does not have access to view this employee enrollments", HttpStatusCode.Forbidden), null);

            var enrollments = await _benefitsUnitOfWork.Enrollments
                .Include(x => x.Plan)
                .Where(x => x.EmployeeId == employeeId)
                .ToListAsync();

            if (enrollments == null) return new GenericResponse<List<EnrollmentDto>>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<List<EnrollmentDto>>(null, enrollments.Select(x => _mapper.Map<EnrollmentDto>(x)).ToList());
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> UpsertEnrollments(int employerId, int employeeId, byte[] encodedEmployerEmployee, List<UpsertEnrollmentDto> enrollments)
        {
            var decodedResponse = _decodingService.GetDecodedEmployerEmployee(encodedEmployerEmployee);
            if (decodedResponse.Result == null) return new GenericResponse<List<EnrollmentDto>>(decodedResponse.Error, null);

            if (decodedResponse.Result.employerId != employerId || decodedResponse.Result.employeeId != employeeId)
                return new GenericResponse<List<EnrollmentDto>>(new HttpError("Employer does not have access to enroll this employee", HttpStatusCode.Forbidden), null);

            if (enrollments.DistinctBy(x => x.PlanId).Count() < enrollments.Count)
            {
                return new GenericResponse<List<EnrollmentDto>>(new HttpError("Employee can not have multiple enrollments for the same plan", HttpStatusCode.BadRequest), null);
            }

            if (enrollments.Any(x => x.Election < 0)) return new GenericResponse<List<EnrollmentDto>>(new HttpError("At least one of the plans contains negative election", HttpStatusCode.BadRequest), null);

            var employerPlans = await _benefitsUnitOfWork.Plans.Include(x => x.Package)
                .Where(x => x.Package.EmployerId == employerId)
                .ToListAsync();

            if (!enrollments.All(dto => employerPlans.Any(x => x.Id == dto.PlanId)))
            {
                return new GenericResponse<List<EnrollmentDto>>(new HttpError("Cannot enroll employees to plans that are not assigned to employer", HttpStatusCode.BadRequest), null);
            }

            var upserted = new List<EnrollmentDto>();

            var existingEnrollments = await _benefitsUnitOfWork.Enrollments.GetSet()
                .Where(x => x.EmployeeId == employeeId)
                .ToListAsync();
            
            var enrollmentPlanIds = enrollments.Select(e => e.PlanId).ToList();
            var plans = await _benefitsUnitOfWork.Plans.GetSet()
                .Include(x => x.Package)
                .Where(x => enrollmentPlanIds.Contains(x.Id))
                .ToListAsync();

            if (plans.Any(x => x.Package.Initialized == null)) return new GenericResponse<List<EnrollmentDto>>(new HttpError("At least one of the packages is not initialized yet", HttpStatusCode.BadRequest), null);
            if (plans.Any(x => x.Package.PlanEnd < DateTime.UtcNow)) return new GenericResponse<List<EnrollmentDto>>(new HttpError("At least one of the plans has expired", HttpStatusCode.BadRequest), null);
            if (plans.Any(x => x.Package.IsRemoved)) return new GenericResponse<List<EnrollmentDto>>(new HttpError("At least one of the packages were removed", HttpStatusCode.BadRequest), null);

            foreach (var e in existingEnrollments)
            {
                var dto = enrollments.FirstOrDefault(x => x.Id == e.Id);
                if (dto == null) continue; //if enrollments could be removed in the future, "continue" should be replaced with remove logic

                e.Election = dto.Election;
                e.PlanId = dto.PlanId;
                e.Plan = plans.First(x => x.Id == dto.PlanId);
                upserted.Add(_mapper.Map<EnrollmentDto>(e));
                _benefitsUnitOfWork.Enrollments.Upsert(e);
            }

            await _benefitsUnitOfWork.CompleteAsync();

            foreach (var e in enrollments.Where(x => x.Id == 0))
            {
                var enrollment = new Enrollment()
                {
                    Election = e.Election,
                    EmployeeId = employeeId,
                    PlanId = e.PlanId,
                    Plan = plans.First(x => x.Id == e.PlanId)
                };
                await _benefitsUnitOfWork.Enrollments.Add(enrollment);
                upserted.Add(_mapper.Map<EnrollmentDto>(enrollment));
            }
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<List<EnrollmentDto>>(null, upserted);
        }
    }
}
