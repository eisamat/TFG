using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class AddPatientDto
    {
        [Required]
        public string Nhc { get; set; }
        
        [Required]
        public string Zip { get; set; }
    }
}