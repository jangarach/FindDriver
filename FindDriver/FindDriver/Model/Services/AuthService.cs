using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FindDriver.Api.Model.Services
{
    public interface IAuthService
    {
        Task<IActionResult> AuthUserAsync(string username, string password);
        Task<string> AuthExternalUserAsync(AuthenticateResult authResult);
    }
    public class AuthService : IAuthService
    {
        private const string callbackScheme = "finddriveapp";
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        public AuthService(IUserService userService, IConfiguration config)
        {
            _config = config;
            _userService = userService;
        }

        public async Task<IActionResult> AuthUserAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new BadRequestObjectResult("Не правильный логин или пароль");

            var user = await _userService.GetUserAsync(e => e.Username == username && e.Password == password);

            if (user == null)
                return new NotFoundObjectResult("Не удалось аутентифицировать пользователя, проверьте логин или пароль");

            var expTime = GetExpiresTime();
            string jwtToken = GenerateToken(user, expTime);

            var result = new
            {
                access_toke = jwtToken,
                id = user.Id,
                expires = expTime
            };
            return new JsonResult(result);
        }

        public async Task<string> AuthExternalUserAsync(AuthenticateResult authResult)
        {
            if (authResult == null)
                throw new ApplicationException("Не удалось получить email или имя пользователя из внешних источников");

            var claims = authResult.Principal?.Identities.FirstOrDefault()?.Claims;
            var fullname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (fullname == null || email == null)
                throw new ApplicationException("Не удалось получить email или имя пользователя из внешних источников");

            var user = await _userService.GetUserAsync(e => e.Username == email && e.Fullname.ToLower() == fullname.ToLower());
            if (user == null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = email,
                    Fullname = fullname,
                    Password = "external"
                };
                await _userService.RegistrationAsync(user);
            }

            var expTime = GetExpiresTime();

            var qs = new Dictionary<string, string>
                {
                    { "access_token", GenerateToken(user, expTime) },
                    { "id", user.Id.ToString()},
                    { "expires", expTime.ToString("dd.MM.yyyyHH:mm:ss") }
                };

            var url = callbackScheme + "://#" + string.Join(
                "&",
                qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            return url;
        }

        private string GenerateToken(User user, DateTime expTime)
        {
            //Создание утверждения
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username));

            //Получение симметричного ключа
            var secKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));

            //Создание JWT токена
            var jwtToken = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    notBefore: DateTime.Now,
                    claims: claims,
                    expires: expTime,
                    signingCredentials: new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private DateTime GetExpiresTime() => DateTime.Now.Add(TimeSpan.FromMinutes(1));
    }
}
