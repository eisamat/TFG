namespace Shared.Api.Responses
{
    public class PatientLoginResponse
    {
        public string Id { get; set; }
        public string Nhc { get; set; }
        public string Zip { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Therapist { get; set; }
    }
}