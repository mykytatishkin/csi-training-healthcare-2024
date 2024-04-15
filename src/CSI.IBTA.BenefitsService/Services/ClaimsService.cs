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
    internal class ClaimsService : IClaimsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserBalanceService _userBalanceService;

        public ClaimsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper, IUserBalanceService userBalanceService)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
            _userBalanceService = userBalanceService;
        }

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "")
        {
            var filteredClaims = _benefitsUnitOfWork.Claims.GetSet()
                .Include(c => c.Enrollment.Plan.Package)
                .Include(c => c.Enrollment.Plan.PlanType)
                .Where(c => claimNumber == "" || c.ClaimNumber.Contains(claimNumber))
                .Where(c => employerId == "" || c.Enrollment.Plan.Package.EmployerId.ToString() == employerId)
                .Where(c => claimStatus == "" || c.Status == Enum.Parse<ClaimStatus>(claimStatus));

            var totalCount = filteredClaims.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var claims = await filteredClaims
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var claimDtos = claims.Select(_mapper.Map<ClaimDto>).ToList();

            var response = new PagedClaimsResponse(claimDtos, page, pageSize, totalPages, totalCount);
            return new GenericResponse<PagedClaimsResponse>(null, response);
        }

        public async Task<GenericResponse<ClaimDto>> GetClaim(int claimId)
        {
            var claim = await _benefitsUnitOfWork.Claims
                .Include(x => x.Enrollment.Plan)
                .Include(x => x.Enrollment.Plan.PlanType)
                .Include(c => c.Enrollment.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null) return new GenericResponse<ClaimDto>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<ClaimDto>(null, _mapper.Map<ClaimDto>(claim));
        }

        public async Task<GenericResponse<bool>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto)
        {
            var claim = await _benefitsUnitOfWork.Claims
                .Include(x => x.Enrollment)
                .Include(x => x.Enrollment.Plan)
                .Include(x => x.Enrollment.Plan.PlanType)
                .Include(c => c.Enrollment.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null)
                return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            var enrollment = await _benefitsUnitOfWork.Enrollments
                .Include(x => x.Plan)
                .FirstOrDefaultAsync(x => x.EmployeeId == claim.Enrollment.EmployeeId && x.PlanId == updateClaimDto.PlanId);

            if (enrollment == null)
                return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            var balance = await _userBalanceService.GetCurrentBalance(enrollment.Id);
            if (balance.Error != null) return new GenericResponse<bool>(balance.Error, false);

            if (balance.Result < updateClaimDto.Amount)
            {
                return new GenericResponse<bool>(new HttpError("Consumer's balance is insufficient to change enrollment", HttpStatusCode.BadRequest), false);
            }

            claim.Enrollment = enrollment;
            claim.DateOfService = updateClaimDto.DateOfService;
            claim.Amount = updateClaimDto.Amount;
            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<bool>(null, true);
        }
        
        public async Task<GenericResponse<bool>> ApproveClaim(int claimId)
        {
            var claim = await _benefitsUnitOfWork.Claims
                .Include(x => x.Enrollment)
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            var res = await _userBalanceService.GetCurrentBalance(claim.Enrollment.Id);
            if (res.Error != null) return new GenericResponse<bool>(res.Error, false);

            if (res.Result < claim.Amount)
            {
                return new GenericResponse<bool>(new HttpError("Consumer's balance is insufficient", HttpStatusCode.BadRequest), false);
            }

            var transaction = new Transaction()
            {
                Amount = claim.Amount,
                Type = TransactionType.Outcome,
                DateTime = DateTime.UtcNow,
                Enrollment = claim.Enrollment
            };

            await _benefitsUnitOfWork.Transactions.Add(transaction);

            claim.Status = ClaimStatus.Approved;
            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();
            return new GenericResponse<bool>(null, true);
        }

        public async Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto)
        {
            var claim = await _benefitsUnitOfWork.Claims.GetById(claimId);

            if (claim == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);
            claim.Status = ClaimStatus.Denied;
            claim.RejectionReason = dto.RejectionReason;

            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<bool>(null, true);
        }

        public async Task<GenericResponse<bool>> FileClaim(int userId, FileClaimDto dto)
        {
            var enrollment = await _benefitsUnitOfWork.Enrollments
                .Include(x => x.Plan)
                .Include(x => x.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == dto.EnrollmentId);

            if (enrollment == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            if (enrollment.EmployeeId != userId) return new GenericResponse<bool>(HttpErrors.Forbidden, false);

            var balanceRes = await _userBalanceService.GetCurrentBalance(enrollment.Id);

            if (balanceRes?.Result == null) return new GenericResponse<bool>(balanceRes?.Error, false);
            if(balanceRes.Result < dto.Amount) return new GenericResponse<bool>(new HttpError("Insufficient balance", HttpStatusCode.BadRequest), false);

            if (dto.Amount < 0) return new GenericResponse<bool>(new HttpError("Amount can not be negative", HttpStatusCode.BadRequest), false);
            if (dto.DateOfService > DateOnly.FromDateTime(DateTime.UtcNow)) return new GenericResponse<bool>(new HttpError("Date of service date must be less than current date", HttpStatusCode.BadRequest), false);
            if (dto.DateOfService < DateOnly.FromDateTime(enrollment.Plan.Package.PlanStart)) return new GenericResponse<bool>(new HttpError("Date of service date must be greater than package start date", HttpStatusCode.BadRequest), false);
            
            if (dto.Receipt == null || dto.Receipt.Length == 0)
                return new GenericResponse<bool>(new HttpError("Receipt file is invalid", HttpStatusCode.BadRequest), false);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await dto.Receipt.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                var encodedReceipt  = Convert.ToBase64String(bytes);

                var claim = new Claim()
                {
                    ClaimNumber = "TempNumber",
                    DateOfService = dto.DateOfService,
                    EnrollmentId = dto.EnrollmentId,
                    Amount = dto.Amount,
                    Status = ClaimStatus.Pending,
                    EncodedReceipt = encodedReceipt
                };

                await _benefitsUnitOfWork.Claims.Add(claim);
                await _benefitsUnitOfWork.CompleteAsync();

                return new GenericResponse<bool>(null, true);
            }
        }
    }
}
