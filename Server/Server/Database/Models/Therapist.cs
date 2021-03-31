using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models
{
    public class Therapist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string FullName { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public ICollection<Patient> Patients { get; set; }
    }
}