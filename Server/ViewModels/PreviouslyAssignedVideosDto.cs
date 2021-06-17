using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class PreviouslyAssignedVideosDto
    {
        public Guid Id { get; set; }
        
        public ICollection<VideoDto> Videos { get; set; }
        
        public DateTime Date { get; set; }
    }
}