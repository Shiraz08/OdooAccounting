using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;


        public InvoiceController(ILogger<InvoiceController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
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
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                await _odooService.PushInvoiceAsync(invoice);
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

    }
}