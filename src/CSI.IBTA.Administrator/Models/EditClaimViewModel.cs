using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
//using System.ComponentModel.DataAnnotations;

namespace CSI.IBTA.Administrator.Models
{
    public class EditClaimViewModel
    {
        public int ClaimId { get; set; }
        public UserInfoDto Employee { get; set; } = null!;
        public string ClaimNumber { get; set; } = null!;
        public DateOnly DateOfService { get; set; }
        public int PlanId { get; set; }
        public PlanIdAndNameDto Plan { get; set; } = null!;
        public decimal Amount { get; set; }
        public IList<PlanIdAndNameDto> AvailablePlans { get; set; } = new List<PlanIdAndNameDto>();
    }
}
