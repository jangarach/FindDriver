using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InternalAuth([FromBody]string username, [FromBody]string password)
        {
            return await _authService.AuthUserAsync(username, password);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task ExternalAuth(string scheme)
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
                var url = await _authService.AuthExternalUserAsync(auth);
                // Redirect to final url
                Request.HttpContext.Response.Redirect(url);

                //Request.HttpContext.Response.Redirect("findDriverApp://");
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
