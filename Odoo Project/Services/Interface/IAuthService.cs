using Odoo_Project.Models;

namespace Odoo_Project.Services.Interface
{
    public interface IAuthService
    {
        Task PushClientAsync(Client client);

        Task PushCreditNoteAsync(CreditNote creditnote);
        Task PushInvoiceAsync(Invoice invoice);

        Task PushPaymentAsync(Payment payment);
    }
}
