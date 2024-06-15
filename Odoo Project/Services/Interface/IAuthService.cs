using Odoo_Project.Models;

namespace Odoo_Project.Services.Interface
{
    public interface IAuthService
    {
        Task PushClientAsync(Client client);
    }
}
