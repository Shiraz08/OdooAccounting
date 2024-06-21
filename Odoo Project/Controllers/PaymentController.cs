using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using Odoo_Project.Services.Interface;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IAuthService _odooService;
        private readonly ApplicationDbContext _context;


        public PaymentController(ILogger<PaymentController> logger, IAuthService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var payments = await _context.Payments.ToListAsync();
                if (payments == null || !payments.Any())
                {
                    _logger.LogWarning("No payments found.");
                }
                return View(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching payments.");
                return View(new List<Payment>());
            }
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
                await _odooService.PushPaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

    }
}