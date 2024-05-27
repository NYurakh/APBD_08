using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripManagement.Models;

[PrimaryKey("IdClient", "IdTrip")]
[Table("Client_Trip")]
public partial class ClientTrip
{
    [Key]
    public int IdClient { get; set; }

    [Key]
    public int IdTrip { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegisteredAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [ForeignKey("IdClient")]
    [InverseProperty("ClientTrips")]
    public virtual Client IdClientNavigation { get; set; } = null!;

    [ForeignKey("IdTrip")]
    [InverseProperty("ClientTrips")]
    public virtual Trip IdTripNavigation { get; set; } = null!;
}
