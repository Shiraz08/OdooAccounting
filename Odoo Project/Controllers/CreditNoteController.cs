using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using Odoo_Project.Services.Interface;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class CreditNoteController : Controller
    {
        private readonly ILogger<CreditNoteController> _logger;
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public CreditNoteController(ILogger<CreditNoteController> logger, IAuthService authService, ApplicationDbContext context)
        {
            _logger = logger;
            _authService = authService;
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var refund = await _context.CreditNotes.ToListAsync();
            return View(refund);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,InvoiceDate,DueDate,Activities,TaxIncluded,Tax,Total,Payment,Status")] CreditNote refund)
        {
            if (ModelState.IsValid)
            {
                
                await _authService.PushCreditNoteAsync(refund);
                return RedirectToAction(nameof(Index));
            }
            return View(refund);
        }

    }
}