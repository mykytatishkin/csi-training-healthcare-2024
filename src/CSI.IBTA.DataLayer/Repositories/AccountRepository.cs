using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Repositories
{
    internal class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(CsiHealthcare2024Context context, ILogger logger) : base(context, logger) { }

        public override async Task<bool> Upsert(Account entity)
        {
            try
            {
                var existingAccount = await dbSet
                    .Where(x => x.Id == entity.Id)
                    .FirstOrDefaultAsync();

                if (existingAccount == null)
                {
                    return await Add(entity);
                }

                existingAccount.Username = entity.Username;
                existingAccount.Password = entity.Password;
                existingAccount.Role = entity.Role;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(AccountRepository));
                return false;
            }
        }
    }
}
