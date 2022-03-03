using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;
using System.Linq.Expressions;

namespace FindDriver.Api.Model.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsersAsync();
        User FindUser(Guid id);
        Task<User> FindUserAsync(Guid id);
        Task<IList<User>> FindAllUsersAsync(Expression<Func<User, bool>> expression);
        Task<User> CreateUserAsync(User newUser);
        Task<User> UpdateUserAsync(Guid updateUserId, User user);
        Task DeleteUserAsync(User user);
    }

    public class UserService : IUserService
    {
        IUserRepository _userRepository; 
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public User FindUser(Guid id)
        {
            return _userRepository.Find(id);
        }

        public async Task<User> FindUserAsync(Guid id)
        {
            return await _userRepository.FindAsync(id);
        }
        public async Task<User> CreateUserAsync(User newUser)
        {
            await _userRepository.InsertAsync(newUser);
            return newUser;
        }
        public async Task<User> UpdateUserAsync(Guid updateUserId, User user)
        {
            var updUser = await _userRepository.FindAsync(updateUserId);
            updUser.Fullname = user.Fullname;
            updUser.Username = user.Username;
            updUser.Password = user.Password;
            await (_userRepository as UserRepository).DbContext.SaveChangesAsync();
            return updUser;
        }
        public async Task DeleteUserAsync(User user)
        {
            await _userRepository.DeleteAsync(user);
        }

        public async Task<IList<User>> FindAllUsersAsync(Expression<Func<User, bool>> expression)
        {
            return await _userRepository.FindAllAsync(expression);
        }
    }
}
