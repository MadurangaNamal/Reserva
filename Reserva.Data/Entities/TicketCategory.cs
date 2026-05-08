using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva.Data.Entities;

public class TicketCategory
{
    [Key]
    public Guid CategoryId { get; set; }
    public Guid EventId { get; set; }

    [ForeignKey("EventId")]
    public Event Event { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }

    public ICollection<BookingItem> BookingItems { get; set; }
    public ICollection<Waitlist> WaitlistEntries { get; set; }
}
