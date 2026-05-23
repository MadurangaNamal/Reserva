using AutoMapper;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        ArgumentException.ThrowIfNullOrWhiteSpace(venue, nameof(venue));

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

    public Task<EventDto> GetEventByIdAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task<List<EventDto>> GetEventsByOrganizerAsync(Guid organizerId)
    {
        throw new NotImplementedException();
    }

    public Task<List<EventDto>> SearchEventsAsync(string? keyword, DateTime? date, string? status)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateEventAsync(Guid eventId, string title, string? description, string venue, DateTime eventDate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CancelEventAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}
