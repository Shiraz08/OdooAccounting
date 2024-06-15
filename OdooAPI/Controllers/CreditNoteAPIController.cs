using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Threading.Tasks;

namespace Odoo_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditNoteApiController : ControllerBase
    {
        private readonly ILogger<CreditNoteApiController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public CreditNoteApiController(ILogger<CreditNoteApiController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
        }

        [HttpPost]
        [Route("push")]
        public async Task<IActionResult> PushCreditNote([FromBody] CreditNote creditNote)
        {
            if (creditNote == null)
            {
                return BadRequest("Credit Note is null.");
            }

            _context.Add(creditNote);
            await _context.SaveChangesAsync();

            try
            {
                await _odooService.PushCreditNoteAsync(creditNote);
                return Ok("Credit Note pushed to Odoo successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pushing Credit Note to Odoo");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
