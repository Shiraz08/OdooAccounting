using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using Odoo_Project.Services.Interface;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;


        public InvoiceController(ILogger<InvoiceController> logger, IAuthService authService, ApplicationDbContext context)
        {
            _logger = logger;
            _authService = authService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        
        {
            var invoices = await _context.Invoices.ToListAsync();
            return View(invoices);
        }


        public IActionResult Create()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,InvoiceDate,DueDate,Activities,TaxIncluded,Tax,Total,Payment,Status")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _authService.PushInvoiceAsync(invoice);
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

    }
}