using Reserva.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva.Data.Entities;

public class Event
{
    [Key]
    public Guid EventId { get; set; }
    public Guid OrganizerId { get; set; }

    [ForeignKey("OrganizerId")]
    public User Organizer { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Venue { get; set; }
    public DateTime EventDate { get; set; }
    public EventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<TicketCategory> TicketCategories { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Waitlist> WaitlistEntries { get; set; }
}
