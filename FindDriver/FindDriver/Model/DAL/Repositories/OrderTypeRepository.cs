using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface IOrderTypeRepository : IRepository<OrderType>
    {
        Task<List<OrderType>> FindAllAsync(List<int> keyIds);
    }
    public class OrderTypeRepository : Repository<OrderType>, IOrderTypeRepository
    {
        public OrderTypeRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<List<OrderType>> FindAllAsync(List<int> keyIds)
        {
            return await Entities.Where(e => keyIds.Contains(e.Id)).ToListAsync();
        }
    }
}
