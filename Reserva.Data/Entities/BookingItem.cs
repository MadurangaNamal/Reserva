using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva.Data.Entities;

public class BookingItem
{
    [Key]
    public Guid BookingItemId { get; set; }
    public Guid BookingId { get; set; }

    [ForeignKey("BookingId")]
    public required Booking Booking { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public required TicketCategory TicketCategory { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
