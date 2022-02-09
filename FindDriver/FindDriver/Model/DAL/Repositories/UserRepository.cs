using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Domain.Interfaces;
using FindFriver.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Api.Model.DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context)
           : base(context)
        { }
    }
}
