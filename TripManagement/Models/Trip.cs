using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripManagement.Models;

[Table("Trip")]
public partial class Trip
{
    [Key]
    public int IdTrip { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(220)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    [InverseProperty("IdTripNavigation")]
    public virtual ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();

    [ForeignKey("IdTrip")]
    [InverseProperty("IdTrips")]
    public virtual ICollection<Country> IdCountries { get; set; } = new List<Country>();
}
