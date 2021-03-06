using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models
{
    public class Patient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Nhc { get; set; }
        public string Zip { get; set; }
        
        public Therapist Therapist { get; set; }
        
        public ICollection<Video> Videos { get; set; }
        
        public ICollection<AssignmentRecord> Records { get; set; }
    }
}