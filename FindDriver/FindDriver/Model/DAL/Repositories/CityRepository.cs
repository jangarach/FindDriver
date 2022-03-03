using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
    }
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
