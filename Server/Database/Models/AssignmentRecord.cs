using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Server.Database.Models
{
    public class AssignmentRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Video Video { get; set; }
        
        public Patient Patient { get; set; }
        
        public Guid AssignmentId { get; set; }
        
        public DateTime Date { get; set; }
    }
}