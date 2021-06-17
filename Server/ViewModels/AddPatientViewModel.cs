using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class AddPatientViewModel
    {
        [Required]
        public string Nhc { get; set; }
        
        [Required]
        public string Zip { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}