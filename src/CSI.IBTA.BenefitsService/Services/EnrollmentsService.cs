using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net;
using CSI.IBTA.UserService.Interfaces;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class EnrollmentsService : IEnrollmentsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IDecodingService _decodingService;

        public EnrollmentsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper, IDecodingService decodingService)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
            _decodingService = decodingService;
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

            if (enrollments.Any(x => x.Election < 0)) return new GenericResponse<List<EnrollmentDto>>(new HttpError("At least one of the plans contains negative contribution", HttpStatusCode.BadRequest), null);

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
