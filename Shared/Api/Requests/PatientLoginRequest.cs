namespace Shared.Api.Requests
{
    public class PatientLoginRequest
    {
        public string Token { get; }

        public PatientLoginRequest(string token)
        {
            Token = token;
        }
    }
}