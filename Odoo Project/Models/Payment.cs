using System.ComponentModel.DataAnnotations.Schema;

namespace Odoo_Project.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

}