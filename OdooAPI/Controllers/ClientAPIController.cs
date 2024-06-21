using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Threading.Tasks;

namespace Odoo_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientApiController : ControllerBase
    {
        private readonly ILogger<ClientApiController> _logger;
        private readonly OdooService _odooService;
        private readonly ApplicationDbContext _context;

        public ClientApiController(ILogger<ClientApiController> logger, OdooService odooService, ApplicationDbContext context)
        {
            _logger = logger;
            _odooService = odooService;
            _context = context;
        }


        [HttpPost]
        [Route("push")]
        public async Task<IActionResult> PushClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Client is null.");
            }

            _context.Add(client);
            await _context.SaveChangesAsync();

            try
            {
                await _odooService.PushClientAsync(client);
                return Ok("Client pushed to Odoo successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pushing client to Odoo");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
