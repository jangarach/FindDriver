using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<List<City>> FindAllAsync(List<int> keyIds);
    }
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DbContext dbContext)
            :base(dbContext)
        {

        }
        public async Task<List<City>> FindAllAsync(List<int> keyIds)
        {
            return await Entities.Where(e => keyIds.Contains(e.Id)).ToListAsync();
        }
    }
}
