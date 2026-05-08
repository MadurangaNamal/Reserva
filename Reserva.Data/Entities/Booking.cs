using Reserva.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva.Data.Entities;

public class Booking
{
    [Key]
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }

    public Guid EventId { get; set; }

    [ForeignKey("EventId")]
    public required Event Event { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<BookingItem>? BookingItems { get; set; }
}
