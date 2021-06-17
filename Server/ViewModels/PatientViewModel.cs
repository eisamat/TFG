using System.Collections.Generic;

namespace Server.ViewModels
{
    public class PatientViewModel
    {
        public string Id { get; set; }
        public string Nhc { get; set; }
        public string Zip { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Therapist { get; set; }
        
        public ICollection<VideoDto> Videos { get; set; }
        public ICollection<PreviouslyAssignedVideosDto> PreviouslyAssignedVideos { get; set; }
    }
}