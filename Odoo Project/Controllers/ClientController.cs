using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using Odoo_Project.Services.Interface;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public ClientController(ILogger<ClientController> logger, OdooService odooService, ApplicationDbContext context,IAuthService authService)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var client = await _context.Clients.ToListAsync();
            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
                byte[] photoBytes = null;

                if (client.PhotoFile != null && client.PhotoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await client.PhotoFile.CopyToAsync(memoryStream);
                        photoBytes = memoryStream.ToArray();
                    }
                }
            client.Photo = photoBytes;
            client.PhotoFile = null;

            await _authService.PushClientAsync(client);

                return RedirectToAction(nameof(Index));
           
        }


    }
}