using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> FindAllAsync(params object[] keyValues);
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<List<Order>> FindAllAsync(params object[] keyValues)
        {
            return await Entities.Where(e => keyValues.Contains(e.Id)).ToListAsync();
        }
    }
}
