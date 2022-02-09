using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controller
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        IUserService UserService;
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpGet]  
        public async Task<IList<User>> GetAllUsers()
        {
            return await UserService.GetAllUsersAsync();
        }

        [HttpGet]
        public async Task<User> GetUserById(Guid id)
        {
            return await UserService.FindUserAsync(id);
        }

        [HttpPost]
        public async Task<User> CreateUser(User user)
        {
            if (user == null)
                throw new ApplicationException("Добавляемый пользователь пустой");
            user.Id = Guid.NewGuid();
            return await UserService.CreateUserAsync(user);
        }
    }
}
