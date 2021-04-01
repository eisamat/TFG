using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class AddPatientViewModel
    {
        [Required]
        public string Nhc { get; set; }
        
        [Required]
        public string Zip { get; set; }
    }
}