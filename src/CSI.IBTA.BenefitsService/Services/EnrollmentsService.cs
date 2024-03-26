using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class EnrollmentsService : IEnrollmentsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> GetEnrollmentsByEmployeeId(int employeeId)
        {
            var enrollments = await _benefitsUnitOfWork.Enrollments
                .Include(x => x.Plan)
                .Where(x => x.EmployeeId == employeeId)
                .ToListAsync();

            if (enrollments == null) return new GenericResponse<List<EnrollmentDto>>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<List<EnrollmentDto>>(null, enrollments.Select(x => _mapper.Map<EnrollmentDto>(x)).ToList());
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, List<UpsertEnrollmentDto> enrollments)
        {
            if(enrollments.DistinctBy(x => x.PlanId).Count() < enrollments.Count) 
            {
                return new GenericResponse<List<EnrollmentDto>>(new HttpError("Employee can not have multiple enrollments for the same plan", HttpStatusCode.BadRequest), null);
            }

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
                .Where(x => enrollmentPlanIds.Contains(x.Id))
                .ToListAsync();

            foreach (var e in existingEnrollments) 
            {
                var dto = enrollments.FirstOrDefault(x => x.Id == e.Id);
                if(dto == null) return new GenericResponse<List<EnrollmentDto>>(HttpErrors.ResourceNotFound, null);

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
                await _benefitsUnitOfWork.CompleteAsync();
                upserted.Add(_mapper.Map<EnrollmentDto>(enrollment));
            }

            return new GenericResponse<List<EnrollmentDto>>(null, upserted);
        }
    }
}
