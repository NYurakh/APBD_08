using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TripManagement.DTOs;
using TripManagement.Models;

namespace TripManagement.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly maksousDbContext _context;

        public TripController(maksousDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTrips(int page = 1, int pageSize = 10)
        {
            var trips = _context.Trips.OrderByDescending(t => t.DateFrom)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .Select(t => new TripDto
                                      {
                                          Name = t.Name,
                                          Description = t.Description,
                                          DateFrom = t.DateFrom.ToString(),
                                          DateTo = t.DateTo.ToString(),
                                          MaxPeople = t.MaxPeople,
                                          Countries = t.IdCountries.Select(c => c.Name).ToList(),
                                          Clients = t.ClientTrips.Select(ct => $"{ct.IdClientNavigation.FirstName} {ct.IdClientNavigation.LastName}").ToList()
                                      })
                                      .ToList();

            var response = new
            {
                pageNum = page,
                pageSize,
                allPages = _context.Trips.Count() / pageSize + 1,
                trips
            };

            return Ok(response);
        }

        // Implement other endpoints as per your requirements
    }
}