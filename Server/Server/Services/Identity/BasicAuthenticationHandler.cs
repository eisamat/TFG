using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Database.Models;

namespace Server.Services.Identity
{
    public static class BasicAuthenticationDefaults
    {
        public const string AuthenticationScheme = "BasicAuthentication";
        public const string HeaderName = "Authorization";
    }
    
    public class BasicAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ITherapistService _therapistService;
        
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITherapistService therapistService) : base(options, logger, encoder, clock)
        {
            _therapistService = therapistService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            var headerExists = Context.Request.Headers.TryGetValue(BasicAuthenticationDefaults.HeaderName, out var headers);
            
            if (!headerExists)
            {
                // Returning NoResult will allow other handlers to potentially authenticate the request
                return AuthenticateResult.NoResult();
            }
            
            // Only therapists are allowed to basic authentication for now.
            Therapist therapist;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[BasicAuthenticationDefaults.HeaderName]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                therapist = await _therapistService.ValidateCredentials(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (therapist == null)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, therapist.Id.ToString()),
                new Claim(ClaimTypes.Name, therapist.Username)
            };
            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}