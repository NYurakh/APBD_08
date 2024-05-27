using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripManagement.Context;
using TripManagement.DTOs;
using TripManagement.Models;

namespace TripManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly MaksousDbContext _context;

        public TripsController(MaksousDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTrips(int page = 1, int pageSize = 10)
        {
            var tripsQuery = _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                    .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom);

            var trips = await tripsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalTrips = await tripsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

            var result = new
            {
                pageNum = page,
                pageSize = pageSize,
                allPages = totalPages,
                trips = trips.Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new CountryDto { Name = c.Name }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<ActionResult> AssignClientToTrip(int idTrip, [FromBody] CreateClientRequestDto request)
        {
            
            var existingClient = await _context.Clients
                .SingleOrDefaultAsync(c => c.Pesel == request.Pesel);

            if (existingClient != null)
            {
                return BadRequest("A client with the provided PESEL already exists.");
            }


            var isClientAlreadyAssigned = await _context.ClientTrips
                .Include(ct => ct.IdClientNavigation)
                .AnyAsync(ct => ct.IdClientNavigation.Pesel == request.Pesel && ct.IdTrip == idTrip);

            if (isClientAlreadyAssigned)
            {
                return BadRequest("Client is already assigned to this trip.");
            }


            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return BadRequest("Invalid trip. The trip either does not exist or has already occurred.");
            }


            if (request.PaymentDate.HasValue && request.PaymentDate <= DateTime.Now)
            {
                return BadRequest("Invalid PaymentDate. PaymentDate should be in the future.");
            }


            if (existingClient == null)
            {
                existingClient = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };
                _context.Clients.Add(existingClient);
                await _context.SaveChangesAsync();
            }


            var clientTrip = new ClientTrip
            {
                IdClient = existingClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
