using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;
using System.Linq.Expressions;

namespace FindDriver.Api.Model.Services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(Expression<Func<User, bool>> expression);
        Task<IList<User>?> GetUsersAsync(Expression<Func<User, bool>> expression);
        Task<User?> RegistrationAsync(User user);
    }

    public class UserService : IUserService
    {
        IUserRepository _userRepository; 
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> expression)
        {
            if (expression == null)
                throw new ApplicationException("Выражение для получения пользователя пустое.");
            var result = await _userRepository.FindAllAsync(expression);

            if (result == null || result.Count == 0)
                return null;

            return result.SingleOrDefault();
        }

        public async Task<IList<User>?> GetUsersAsync(Expression<Func<User, bool>> expression)
        {
            if (expression == null)
                throw new ApplicationException("Выражение для получения пользователя пустое.");
            return await _userRepository.FindAllAsync(expression);
        }

        public async Task<User?> RegistrationAsync(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username))
                throw new ApplicationException("Параметры для регистрации пользователя не были переданы.");

            var findedUser = await _userRepository.FindAllAsync(e => e.Username == user.Username);

            if (findedUser != null && findedUser.Count > 0)
                throw new ApplicationException("Пользователь уже существует с таким именем. Измените параметры регистрации.");

            await _userRepository.InsertAsync(user);

            throw new NotImplementedException();
        }

    }
}
