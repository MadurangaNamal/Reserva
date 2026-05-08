using Microsoft.EntityFrameworkCore;
using Reserva.Data.Entities;

namespace Reserva.Data;

public class ReservaDbContext : DbContext
{
    public ReservaDbContext(DbContextOptions<ReservaDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<TicketCategory> TicketCategories { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingItem> BookingItems { get; set; }
    public DbSet<Waitlist> Waitlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Role).HasConversion<string>();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Venue).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Organizer)
                  .WithMany(u => u.OrganizedEvents)
                  .HasForeignKey(e => e.OrganizerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(tc => tc.CategoryId);
            entity.Property(tc => tc.Name).IsRequired().HasMaxLength(100);
            entity.Property(tc => tc.Price).HasColumnType("decimal(18,2)");

            entity.HasOne(tc => tc.Event)
                  .WithMany(e => e.TicketCategories)
                  .HasForeignKey(tc => tc.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingId);
            entity.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(b => b.Status).HasConversion<string>();
            entity.Property(b => b.BookingDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(b => b.User)
                  .WithMany(u => u.Bookings)
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Event)
                  .WithMany(e => e.Bookings)
                  .HasForeignKey(b => b.EventId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BookingItem>(entity =>
        {
            entity.HasKey(bi => bi.BookingItemId);
            entity.Property(bi => bi.UnitPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(bi => bi.Booking)
                  .WithMany(b => b.BookingItems)
                  .HasForeignKey(bi => bi.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(bi => bi.TicketCategory)
                  .WithMany(tc => tc.BookingItems)
                  .HasForeignKey(bi => bi.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Waitlist>(entity =>
        {
            entity.HasKey(w => w.WaitlistId);
            entity.Property(w => w.Status).HasConversion<string>();
            entity.Property(w => w.RequestedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(w => w.User)
                  .WithMany(u => u.WaitlistEntries)
                  .HasForeignKey(w => w.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(w => w.Event)
                  .WithMany(e => e.WaitlistEntries)
                  .HasForeignKey(w => w.EventId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(w => w.TicketCategory)
                  .WithMany(tc => tc.WaitlistEntries)
                  .HasForeignKey(w => w.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
