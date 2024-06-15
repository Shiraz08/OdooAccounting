using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Threading.Tasks;

namespace Odoo_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceApiController : ControllerBase
    {
        private readonly ILogger<InvoiceApiController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public InvoiceApiController(ILogger<InvoiceApiController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
        }

        [HttpPost]
        [Route("push")]
        public async Task<IActionResult> PushInvoice([FromBody] Invoice invoice)
        {
            if (invoice == null)
            {
                return BadRequest("Invoice is null.");
            }

            _context.Add(invoice);
            await _context.SaveChangesAsync();

            try
            {
                await _odooService.PushInvoiceAsync(invoice);
                return Ok("Invoice pushed to Odoo successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pushing Invoice to Odoo");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
