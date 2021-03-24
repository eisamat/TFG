using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Nhc { get; set; }
        public string Zip { get; set; }
    }
}