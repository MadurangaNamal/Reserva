using Reserva.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace Reserva.Data.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    public required string FullName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? Phone { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Event>? OrganizedEvents { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
    public ICollection<Waitlist>? WaitlistEntries { get; set; }
}
