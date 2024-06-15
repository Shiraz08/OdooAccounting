using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Diagnostics;

namespace Odoo_Project.Controllers
{
    public class CreditNoteController : Controller
    {
        private readonly ILogger<CreditNoteController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public CreditNoteController(ILogger<CreditNoteController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
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
                _context.Add(refund);
                await _context.SaveChangesAsync();
                await _odooService.PushCreditNoteAsync(refund);
                return RedirectToAction(nameof(Index));
            }
            return View(refund);
        }

    }
}