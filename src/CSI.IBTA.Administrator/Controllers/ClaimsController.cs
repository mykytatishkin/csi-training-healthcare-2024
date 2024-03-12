using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Claims")]
    public class ClaimsController : Controller
    {
        private readonly IClaimsClient _claimsClient;
        private readonly IUserServiceClient _usersClient;

        public ClaimsController(IClaimsClient claimsClient, IUserServiceClient usersClient)
        {
            _claimsClient = claimsClient;
            _usersClient = usersClient;
        }

        [HttpGet("Details")]
        public async Task<IActionResult> ClaimDetails(int claimId)
        {
            var claimsRes = await _claimsClient.GetClaimDetails(claimId);

            if (claimsRes.Result == null)
            {
                throw new Exception("Failed to retrieve claim details");
            }

            var userRes = await _usersClient.GetUser(claimsRes.Result.EmployeeId);
            if (userRes.Result == null)
            {
                throw new Exception("Failed to retrieve claim details");
            }

            var viewModel = new ClaimDetailsViewModel
            {
                Claim = claimsRes.Result,
                Consumer = userRes.Result
            };

            return PartialView("_ClaimDetails", viewModel);
        }

    }
}
