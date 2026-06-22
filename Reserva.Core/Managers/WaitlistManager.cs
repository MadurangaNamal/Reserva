using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.Entities;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class WaitlistManager : IWaitlistManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public WaitlistManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<int> GetWaitlistPositionAsync(Guid waitlistId)
    {
        var waitList = await _dbContext.Waitlists
            .FirstOrDefaultAsync(w => w.WaitlistId == waitlistId);

        if (waitList == null)
            throw new KeyNotFoundException($"Waitlist entry with ID '{waitlistId}' was not found.");

        if (waitList.Status != WaitlistStatus.Waiting)
            throw new InvalidOperationException("This waitlist entry is no longer active.");

        var position = await _dbContext.Waitlists
            .Where(w => w.EventId == waitList.EventId
            && w.CategoryId == waitList.CategoryId
            && w.Status == WaitlistStatus.Waiting
            && w.RequestedAt < waitList.RequestedAt)
            .CountAsync();

        return position + 1;
    }

    public async Task<WaitlistDto> JoinWaitlistAsync(Guid userId, Guid eventId, Guid categoryId)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        var existingEvent = await _dbContext.Events
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (existingEvent is null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        if (existingEvent.Status != EventStatus.Published)
            throw new InvalidOperationException("Cannot join the waitlist for an event that is not published.");

        // Validate category exists and belongs to this event
        var category = await _dbContext.TicketCategories
            .FirstOrDefaultAsync(tc => tc.CategoryId == categoryId && tc.EventId == eventId);

        if (category is null)
            throw new KeyNotFoundException($"Ticket category with ID '{categoryId}' was not found for this event.");

        // Only allow joining if the category is actually sold out
        if (category.AvailableSeats > 0)
            throw new InvalidOperationException("Cannot join the waitlist while seats are still available for this category.");

        var alreadyWaiting = await _dbContext.Waitlists
            .AnyAsync(w => w.UserId == userId
                        && w.EventId == eventId
                        && w.CategoryId == categoryId
                        && w.Status == WaitlistStatus.Waiting);

        if (alreadyWaiting)
            throw new InvalidOperationException("You are already on the waitlist for this category.");

        var waitlistEntry = new Waitlist
        {
            WaitlistId = Guid.NewGuid(),
            UserId = userId,
            EventId = eventId,
            CategoryId = categoryId,
            RequestedAt = DateTime.UtcNow,
            Status = WaitlistStatus.Waiting
        };

        _dbContext.Waitlists.Add(waitlistEntry);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<WaitlistDto>(waitlistEntry);
    }

    public async Task<bool> LeaveWaitlistAsync(Guid waitlistId)
    {
        var waitList = await _dbContext.Waitlists
            .FirstOrDefaultAsync(w => w.WaitlistId == waitlistId);

        if (waitList == null)
            throw new KeyNotFoundException($"Waitlist entry with ID '{waitlistId}' was not found.");

        if (waitList.Status != WaitlistStatus.Waiting)
            throw new InvalidOperationException("Only entries that are currently waiting can be left.");

        waitList.Status = WaitlistStatus.Expired;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
