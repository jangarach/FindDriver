using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.UI;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public AuthenticationController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Autentication([FromBody] AutenticationRequest authRequest)
        {
            //Проверка параметра
            if (authRequest == null)
                throw new ArgumentNullException(nameof(authRequest));
            if (string.IsNullOrEmpty(authRequest.UserName) || string.IsNullOrEmpty(authRequest.Password))
                throw new ArgumentException("Не указан логин или пароль", nameof(authRequest));

            //Получение пользователя
            var users = await _userService.FindAllUsersAsync(e => e.Username == authRequest.UserName && e.Password == authRequest.Password);
            if (users == null || users.Count == 0)
                return BadRequest("Не правильный логин или пароль");

            var user = users.FirstOrDefault();

            //Создание утверждения
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Fullname));

            //Получение симметричного ключа
            var secKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));

            //Создание JWT токена
            var jwtToken = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    notBefore: DateTime.UtcNow,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                    signingCredentials: new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256)
                );

            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var response = new
            {
                Access_toke = encodeJwt,
                id = user.Id
            };
            return new JsonResult(response);
        }

        [HttpGet]
        public async Task MobileAuth(string scheme)
        {
            var auth = await Request.HttpContext.AuthenticateAsync(scheme);

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                // Not authenticated, challenge
                await Request.HttpContext.ChallengeAsync(scheme);
            }
            else
            {
                var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;

                var email = string.Empty;
                email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var surName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var nameIdentifier = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string picture = string.Empty;

                Request.HttpContext.Response.Redirect("xamarinapp");
            }
            //if (scheme == "Facebook")
            //{
            //    picture = await _facebookAuthService.GetFacebookProfilePicURL(auth.Properties.GetTokenValue("access_token"));
            //}
            //else
            //    picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

            //var appUser = new AppUser
            //{
            //    Email = email,
            //    FirstName = givenName,
            //    SecondName = surName,
            //    PictureURL = picture
            //};

            //await CreateOrGetUser(appUser);
            //var authToken = GenerateJwtToken(appUser);

            // Get parameters to send back to the callback
            //var qs = new Dictionary<string, string>
            //{
            //    { "access_token", authToken.token },
            //    { "refresh_token",  string.Empty },
            //    { "jwt_token_expires", authToken.expirySeconds.ToString() },
            //    { "email", email },
            //    { "firstName", givenName },
            //    { "picture", picture },
            //    { "secondName", surName },
            //};

            // Build the result url
            //var url = Callback + "://#" + string.Join(
            //    "&",
            //    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
            //    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            // Redirect to final url
            //Request.HttpContext.Response.Redirect("test");
        }
    }
}
