using System.Collections.Generic;

namespace Server.ViewModels
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
        
        public ICollection<VideoDto> Videos;
    }
}