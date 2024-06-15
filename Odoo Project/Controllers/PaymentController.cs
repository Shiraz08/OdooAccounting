using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;


        public PaymentController(ILogger<PaymentController> logger, OdooService odooService, ApplicationDbContext context)
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
        public async Task<IActionResult> Create([Bind("Id,Number,PaymentDate,Amount,PaymentMethod,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                await _odooService.PushPaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

    }
}