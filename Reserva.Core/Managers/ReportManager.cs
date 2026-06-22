using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class ReportManager : IReportManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public ReportManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<BookingSummaryDto> GetBookingsSummaryAsync(Guid eventId)
    {
        var existingEvent = await _dbContext.Events.FindAsync(eventId);

        if (existingEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        var bookings = await _dbContext.Bookings
            .Where(b => b.EventId == eventId)
            .ToListAsync();

        var totalRevenue = await _dbContext.BookingItems
           .Where(bi => bi.Booking.EventId == eventId
                     && bi.Booking.Status == BookingStatus.Confirmed)
           .SumAsync(bi => (decimal?)(bi.Quantity * bi.UnitPrice)) ?? 0;

        return new BookingSummaryDto
        {
            EventId = existingEvent.EventId,
            EventTitle = existingEvent.Title,
            TotalBookings = bookings.Count,
            ConfirmedBookings = bookings.Count(b => b.Status == BookingStatus.Confirmed),
            CancelledBookings = bookings.Count(b => b.Status == BookingStatus.Cancelled),
            TotalRevenue = totalRevenue
        };
    }

    public async Task<decimal> GetRevenueByEventAsync(Guid eventId)
    {
        var existingEvent = await _dbContext.Events.FindAsync(eventId);

        if (existingEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        return await _dbContext.BookingItems
            .Where(bi => bi.Booking.EventId == eventId
                      && bi.Booking.Status == BookingStatus.Confirmed)
            .SumAsync(bi => (decimal?)(bi.Quantity * bi.UnitPrice)) ?? 0;
    }

    public async Task<int> GetTicketsSoldByEventAsync(Guid eventId)
    {
        var existingEvent = await _dbContext.Events.FindAsync(eventId);

        if (existingEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        var ticketsSold = await _dbContext.BookingItems
            .Where(bi => bi.Booking.EventId == eventId
                && bi.Booking.Status == BookingStatus.Confirmed)
            .SumAsync(bi => bi.Quantity);

        return ticketsSold;
    }
}
