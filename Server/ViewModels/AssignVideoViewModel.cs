using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class AssignVideoViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public ICollection<string> Videos { get; set; }
    }
}