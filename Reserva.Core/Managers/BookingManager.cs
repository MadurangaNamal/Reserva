using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.Entities;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class BookingManager : IBookingManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public BookingManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<BookingDto> CreateBookingAsync(Guid userId, Guid eventId, List<BookingItemRequest> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("At least one ticket item is required.");

        if (items.Any(i => i.Quantity <= 0))
            throw new ArgumentException("Quantity must be greater than zero for all items.");

        var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == userId);

        if (!userExists)
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        var existingEvent = await _dbContext.Events
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (existingEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        if (existingEvent.Status != EventStatus.Published)
            throw new InvalidOperationException("Bookings can only be made for published events.");

        // Update seat counts and inserting rows together
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var bookingItems = new List<BookingItem>();
            decimal totalAmount = 0;

            foreach (var item in items)
            {
                var category = await _dbContext.TicketCategories
                   .FirstOrDefaultAsync(tc => tc.CategoryId == item.CategoryId && tc.EventId == eventId);

                if (category is null)
                    throw new KeyNotFoundException($"Ticket category with ID '{item.CategoryId}' was not found for this event.");

                if (category.AvailableSeats < item.Quantity)
                    throw new InvalidOperationException(
                        $"Not enough seats available for '{category.Name}'. Requested: {item.Quantity}, Available: {category.AvailableSeats}.");

                // Decrement availability
                category.AvailableSeats -= item.Quantity;

                // Snapshot price at time of booking
                var bookingItem = new BookingItem
                {
                    BookingItemId = Guid.NewGuid(),
                    CategoryId = category.CategoryId,
                    Quantity = item.Quantity,
                    UnitPrice = category.Price
                };

                bookingItems.Add(bookingItem);
                totalAmount += item.Quantity * category.Price;
            }

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                UserId = userId,
                EventId = eventId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed,
                TotalAmount = totalAmount,
                BookingItems = bookingItems
            };

            foreach (var item in bookingItems)
                item.BookingId = booking.BookingId;

            _dbContext.Bookings.Add(booking);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var savedBooking = await _dbContext.Bookings
                .Include(b => b.BookingItems)
                    .ThenInclude(bi => bi.TicketCategory)
                .FirstAsync(b => b.BookingId == booking.BookingId);

            return _mapper.Map<BookingDto>(savedBooking);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<BookingDto> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = await _dbContext.Bookings
            .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.TicketCategory)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
            throw new KeyNotFoundException($"Booking with ID '{bookingId}' was not found.");

        return _mapper.Map<BookingDto>(booking);
    }

    public async Task<List<BookingDto>> GetBookingHistoryByUserAsync(Guid userId)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == userId);

        if (!userExists)
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        var bookings = await _dbContext.Bookings
            .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.TicketCategory)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return _mapper.Map<List<BookingDto>>(bookings);
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        var booking = await _dbContext.Bookings
            .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.TicketCategory)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking is null)
            throw new KeyNotFoundException($"Booking with ID '{bookingId}' was not found.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled.");

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Restore seats
            foreach (var item in booking.BookingItems)
            {
                item.TicketCategory.AvailableSeats += item.Quantity;
            }

            booking.Status = BookingStatus.Cancelled;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
