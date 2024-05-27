namespace TripManagement.DTOs
{
    public class TripDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<CountryDto> Countries { get; set; } = new List<CountryDto>();
        public List<ClientDto> Clients { get; set; } = new List<ClientDto>();
    }
}
