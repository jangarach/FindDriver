using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.UI;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        IUserService UserService;
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<User?> RegistrationUser([FromBody]User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) 
                || string.IsNullOrEmpty(user.Fullname) 
                || string.IsNullOrEmpty(user.Password))
                throw new ApplicationException("Переданные данные пустые");
            user.Id = Guid.NewGuid();
            return await UserService.RegistrationAsync(user);
        }
    }
}
