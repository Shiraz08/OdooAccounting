namespace Odoo_Project.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TaxExcluded { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal Payment { get; set; }
        public string Status { get; set; }
    }

}