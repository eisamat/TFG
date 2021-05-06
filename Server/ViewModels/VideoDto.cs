using System;

namespace Server.ViewModels
{
    public class VideoDto
    {
        public Guid Id { get; set; }
        public string YoutubeId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
    }
}