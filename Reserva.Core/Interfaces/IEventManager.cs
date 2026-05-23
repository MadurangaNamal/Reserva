using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface IEventManager
{
    Task<EventDto> CreateEventAsync(Guid organizerId, string title, string? description, string venue, DateTime eventDate);
    Task<EventDto> UpdateEventAsync(Guid eventId, string title, string? description, string venue, DateTime eventDate);
    Task<bool> CancelEventAsync(Guid eventId);
    Task<EventDto> GetEventByIdAsync(Guid eventId);
    Task<List<EventDto>> SearchEventsAsync(string? keyword, DateTime? date, string? status);
    Task<List<EventDto>> GetEventsByOrganizerAsync(Guid organizerId);
}
