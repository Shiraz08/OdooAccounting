using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public ClientController(ILogger<ClientController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
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
                _context.Add(client);
                await _context.SaveChangesAsync();
              
                await _odooService.PushClientAsync(client);

                return RedirectToAction(nameof(Index));
           
        }


    }
}