using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Database.Models;
using Server.Services;

namespace Server.Identity
{
    public static class TokenAuthenticationDefaults
    {
        public const string AuthenticationScheme = "TokenAuthentication";
        public const string HeaderName = "Authorization";
    }
    
    public class TokenAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private static readonly Regex TokenHeaderRegex = new Regex(@"Bearer (.*)");
        
        private readonly IPatientService _patientService;
        
        public TokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IPatientService patientService) : base(options, logger, encoder, clock)
        {
            _patientService = patientService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headerExists = Context.Request.Headers.TryGetValue(TokenAuthenticationDefaults.HeaderName, out var headers);
            
            if (!headerExists)
            {
                // Returning NoResult will allow other handlers to potentially authenticate the request
                return AuthenticateResult.NoResult();
            }
            
            // Get authorization key
            var authorizationHeader = headers[0];

            if (!TokenHeaderRegex.IsMatch(authorizationHeader))
            {
                // Returning NoResult will allow other handlers to potentially authenticate the request
                return AuthenticateResult.NoResult();
            }
            
            // Only patients are allowed to token authentication for now.
            Patient patient;
            try
            {
                var token = TokenHeaderRegex.Replace(authorizationHeader, "$1");
                patient = await _patientService.GetPatientByToken(token);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (patient == null)
            {
                return AuthenticateResult.Fail("Invalid Token");
            }
            
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
                new Claim(ClaimTypes.Name, patient.FullName),
            };
            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}