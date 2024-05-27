namespace TripManagement.DTOs
{
    public class TripDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Clients { get; set; }
    }
}