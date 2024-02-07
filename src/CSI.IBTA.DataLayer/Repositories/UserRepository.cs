using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CsiHealthcare2024Context context, ILogger logger) : base(context, logger) { }

        public override async Task<IEnumerable<User>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return new List<User>();
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (exist == null)
                {
                    return false;
                }

                dbSet.Remove(exist);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(UserRepository));
                return false;
            }
        }

        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existingUser = await dbSet
                    .Where(x => x.Id == entity.Id)
                    .FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    return await Add(entity);
                }

                existingUser.Firstname = entity.Firstname;
                existingUser.Lastname = entity.Lastname;
                existingUser.Account = entity.Account;
                existingUser.Employer = entity.Employer;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(UserRepository));
                return false;
            }
        }
    }
}
