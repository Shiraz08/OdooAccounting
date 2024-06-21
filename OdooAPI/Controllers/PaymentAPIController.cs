using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Threading.Tasks;

namespace Odoo_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentApiController : ControllerBase
    {
        private readonly ILogger<PaymentApiController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public PaymentApiController(ILogger<PaymentApiController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
        }

        [HttpPost]
        [Route("push")]
        public async Task<IActionResult> PushPayment([FromBody] Payment payment)
        {
            try
            {
                if (payment == null)
                {
                    return BadRequest("Payment is null.");
                }

                _context.Add(payment);
                await _context.SaveChangesAsync();

                try
                {
                    await _odooService.PushPaymentAsync(payment);
                    return Ok("Payment pushed to Odoo successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error pushing Payment to Odoo");
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            catch(Exception ex) { throw; }
        }
    }
}
