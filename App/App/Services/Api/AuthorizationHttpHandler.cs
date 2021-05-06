using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using App.Database;

namespace App.Services.Api
{
    public class AuthorizationHttpHandler : HttpClientHandler
    {
        private readonly AppDatabase _database;
            
        public AuthorizationHttpHandler(AppDatabase database)
        {
            _database = database;
            ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var user = await _database.GetUserAsync();

            if (user != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }
                
            return await base.SendAsync(request, cancellationToken);
        }
    }
}