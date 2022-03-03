using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.UI;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
