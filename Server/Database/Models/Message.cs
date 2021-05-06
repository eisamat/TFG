using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsFromPatient { get; set; }
        
        public Therapist Therapist { get; set; }
        
        public Patient Patient { get; set; }
    }
}