using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models
{
    public class Video
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string YoutubeId { get; set; }
        public string Name { get; set; }
        
        public Category Category { get; set; }
        
        public ICollection<Patient> Assigners { get; set; }
        
        public ICollection<AssignmentRecord> Records { get; set; }
    }
}