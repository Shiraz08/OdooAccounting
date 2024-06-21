using System.ComponentModel.DataAnnotations.Schema;

namespace Odoo_Project.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Country { get; set; }

        [NotMapped]
        public IFormFile? PhotoFile { get; set; } 
        
    }
}