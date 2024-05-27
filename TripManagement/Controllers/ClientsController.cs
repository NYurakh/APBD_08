using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripManagement.Context;

namespace TripManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly MaksousDbContext _context;

        public ClientsController(MaksousDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{idClient}")]
        public async Task<ActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .SingleOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
            {
                return NotFound();
            }

            if (client.ClientTrips.Any())
            {
                return BadRequest("Client has assigned trips and cannot be deleted.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
