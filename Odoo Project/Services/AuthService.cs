using Odoo_Project.Models;
using Odoo_Project.Services.Interface;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task PushClientAsync(Client client)
    {
        var response = await _httpClient.PostAsJsonAsync("api/client/push", client);
        response.EnsureSuccessStatusCode();
    }
}
