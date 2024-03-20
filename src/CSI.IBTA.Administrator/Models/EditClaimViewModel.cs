using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
//using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EditClaimViewModel
    {
        public ClaimDto Claim { get; set; }
        public UserDto Consumer { get; set; }
        public IList<PlanDto> AvailablePlans { get; set; } = new List<PlanDto>();
    }
}
