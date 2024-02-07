using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Repositories
{
    internal class EmployerRepository : GenericRepository<Employer>, IEmployerRepository
    {
        public EmployerRepository(CsiHealthcare2024Context context, ILogger logger) : base(context, logger) { }

        public override async Task<bool> Upsert(Employer entity)
        {
            try
            {
                var existingEmployer = await dbSet
                    .Where(x => x.Id == entity.Id)
                    .FirstOrDefaultAsync();

                if (existingEmployer == null)
                {
                    return await Add(entity);
                }

                existingEmployer.Street = entity.Street;
                existingEmployer.State = entity.State;
                existingEmployer.City = entity.City;
                existingEmployer.Code = entity.Code;
                existingEmployer.Logo = entity.Logo;
                existingEmployer.Name = entity.Name;
                existingEmployer.Zip = entity.Zip;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(EmployerRepository));
                return false;
            }
        }
    }
}
