using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.Entities;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class EventManager : IEventManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<EventDto> CreateEventAsync(Guid organizerId, string title, string? description, string venue, DateTime eventDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(venue);

        if (eventDate <= DateTime.UtcNow)
            throw new ArgumentException("Event date must be in the future.");

        var organizer = await _dbContext.Users.FindAsync(organizerId);

        if (organizer is null)
            throw new KeyNotFoundException($"Organizer with ID '{organizerId}' was not found.");

        if (organizer.Role != UserRole.Organizer)
            throw new UnauthorizedAccessException("Only users with the Organizer role can create events.");


        var newEvent = new Event
        {
            EventId = Guid.NewGuid(),
            OrganizerId = organizerId,
            Title = title,
            Description = description,
            Venue = venue,
            EventDate = eventDate,
            Status = EventStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Events.AddAsync(newEvent);
        await _dbContext.SaveChangesAsync();

        // Reload with organizer for mapping
        await _dbContext.Entry(newEvent)
            .Reference(e => e.Organizer)
            .LoadAsync();

        return _mapper.Map<EventDto>(newEvent);
    }

    public async Task<EventDto> GetEventByIdAsync(Guid eventId)
    {
        var currentEvent = await _dbContext.Events.FindAsync(eventId);

        if (currentEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        await _dbContext.Entry(currentEvent)
            .Reference(e => e.Organizer)
            .LoadAsync();

        return _mapper.Map<EventDto>(currentEvent);
    }

    public async Task<List<EventDto>> GetEventsByOrganizerAsync(Guid organizerId)
    {
        var events = await _dbContext.Events
            .Where(e => e.OrganizerId == organizerId)
            .ToListAsync();

        return _mapper.Map<List<EventDto>>(events);
    }

    public async Task<List<EventDto>> SearchEventsAsync(string? keyword, DateTime? date, string? status)
    {
        var query = _dbContext.Events
            .Include(e => e.Organizer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(e => e.Title.Contains(keyword) ||
                (e.Description != null && e.Description.Contains(keyword)) ||
                e.Venue.Contains(keyword));
        }

        if (date.HasValue)
            query = query.Where(e => e.EventDate.Date == date.Value.Date);

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<EventStatus>(status, ignoreCase: true, out var eventStatus))
                throw new ArgumentException($"Invalid status '{status}'. Valid values are: Draft, Published, " +
                    $"Cancelled, Completed.");

            query = query.Where(e => e.Status == eventStatus);
        }

        var events = await query.ToListAsync();
        return _mapper.Map<List<EventDto>>(events);
    }

    public async Task<EventDto> UpdateEventAsync(Guid eventId, string title, string? description, string venue, DateTime eventDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(venue);

        if (eventDate <= DateTime.UtcNow)
            throw new ArgumentException("Event date must be in the future.");

        var currentEvent = await _dbContext.Events.FindAsync(eventId);

        if (currentEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        if (currentEvent.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a cancelled event.");

        if (currentEvent.Status == EventStatus.Completed)
            throw new InvalidOperationException("Cannot update a completed event.");

        currentEvent.Title = title;
        currentEvent.Venue = venue;
        currentEvent.EventDate = eventDate;
        currentEvent.Description = description == null ? currentEvent.Description : description;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<EventDto>(currentEvent);
    }

    public async Task<bool> CancelEventAsync(Guid eventId)
    {
        var currentEvent = await _dbContext.Events.FindAsync(eventId);

        if (currentEvent == null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        if (currentEvent.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Event is already cancelled.");

        if (currentEvent.Status == EventStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed event.");

        currentEvent.Status = EventStatus.Cancelled;

        foreach (var booking in currentEvent.Bookings!.Where(b => b.Status != BookingStatus.Cancelled))
        {
            booking.Status = BookingStatus.Cancelled;
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }
}
