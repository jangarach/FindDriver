namespace FindDriver.Api.Model.Services
{
    public interface IAuthenticationService
    {
        string AuthenticateJWT();
    }
    public class AuthenticationService : IAuthenticationService
    {
        public string AuthenticateJWT()
        {
            throw new NotImplementedException();
        }
    }
}
