using Reserva.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva.Data.Entities;

public class Waitlist
{
    [Key]
    public Guid WaitlistId { get; set; }
    public Guid EventId { get; set; }

    [ForeignKey("EventId")]
    public Event Event { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public TicketCategory TicketCategory { get; set; }

    public DateTime RequestedAt { get; set; }
    public WaitlistStatus Status { get; set; }
}
