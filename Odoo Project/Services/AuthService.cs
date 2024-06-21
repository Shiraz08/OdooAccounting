using Microsoft.Extensions.Configuration;
using Odoo_Project.Controllers;
using Odoo_Project.Models;
using Odoo_Project.Services.Interface;
using System.Configuration;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private string odooUrl;
    private readonly ILogger<ClientController> _logger;
    public AuthService(HttpClient httpClient, IConfiguration configuration, ILogger<ClientController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        odooUrl = configuration.GetValue<string>("ServiceUrls:OdooAPI");
    }

    public async Task PushClientAsync(Client client)
    {
        var response = await _httpClient.PostAsJsonAsync($"{odooUrl}/api/ClientApi/push", client);
        response.EnsureSuccessStatusCode();
    }


    public async Task PushCreditNoteAsync(CreditNote creditnote)
    {
        var response = await _httpClient.PostAsJsonAsync($"{odooUrl}/api/CreditNoteApi/push", creditnote);
        response.EnsureSuccessStatusCode();
    }

    public async Task PushInvoiceAsync(Invoice invoice)
    {
        var response = await _httpClient.PostAsJsonAsync($"{odooUrl}/api/InvoiceApi/push", invoice);
        response.EnsureSuccessStatusCode();
    }

    public async Task PushPaymentAsync(Payment payment)
    {
        var response = await _httpClient.PostAsJsonAsync($"{odooUrl}/api/PaymentApi/push", payment);
        response.EnsureSuccessStatusCode();
    }
}
