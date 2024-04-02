using AutoMapper;
using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class EnrollmentsService : IEnrollmentService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> GetUsersEnrollments(List<int> userIds)
        {
            var enrollments = await _benefitsUnitOfWork.Enrollments
                .Find(e => userIds.Contains(e.EmployeeId));

            var enrollmentDtos = enrollments.Select(_mapper.Map<EnrollmentDto>).ToList();
            return new(null, enrollmentDtos);
        }
    }
}
