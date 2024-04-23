using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
//using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EditClaimViewModel
    {
        public ClaimDto Claim { get; set; } = null!;
        public UserDto Consumer { get; set; } = null!;
        public IList<PlanDto> AvailablePlans { get; set; } = new List<PlanDto>();
    }
}
