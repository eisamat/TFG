using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken();
    }

    internal class TokenService : ITokenService
    {
        private const int TokenLength = 8;
        
        public Task<string> GenerateToken()
        {
            var builder = new StringBuilder();
            
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(TokenLength)
                .ToList().ForEach(e => builder.Append(e));

            return Task.FromResult(builder.ToString());
        }
    }
}