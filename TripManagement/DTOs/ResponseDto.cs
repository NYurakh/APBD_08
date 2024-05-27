using TripManagement.Models;

namespace TripManagement.DTOs
{
    public class YourResponseDto
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<Trip> Trips { get; set; }
    }
}
