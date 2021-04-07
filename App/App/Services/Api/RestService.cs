using System.Threading.Tasks;
using Refit;
using Shared.Api.Requests;
using Shared.Api.Responses;

namespace App.Services.Api
{
    public static class Urls
    {
        public const string BaseUrl = "https://10.0.2.2:5001/api";
        public const string AuthenticateUrl = BaseUrl + "login";
        public const string Me = BaseUrl + "@me";
    }
    
    public interface IRestApi
    {
        [Post("/patient/login")]
        Task<PatientLoginResponse> Authenticate(PatientLoginRequest request);
    }
}