using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IBenefitsService
    {
        public Task<GenericHttpResponse<IEnumerable<PlanDto>>> GetAllPlans();
        //public Task<GenericHttpResponse<UserDto>> GetUserByAccountId(int accountId);
        //public Task<GenericHttpResponse<UserDto>> GetUser(int userId);
        public Task<GenericHttpResponse<PlanDto>> CreatePlan(CreatePlanDto createPlanDto);
        //public Task<GenericHttpResponse<UpdatedUserDto>> PutUser(int userId, PutUserDto putUserDto);
        //public Task<GenericHttpResponse<bool>> DeleteUser(int userId);
    }
}