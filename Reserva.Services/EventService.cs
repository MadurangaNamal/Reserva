using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services;

public class EventService : IEventService
{
    private readonly IEventManager _eventManager;

    public EventService(IEventManager eventManager)
    {
        _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
    }

    public async Task<bool> CancelEventAsync(Guid eventId)
    {
        try
        {
            return await _eventManager.CancelEventAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }

    public async Task<EventDto> CreateEventAsync(Guid organizerId, string title, string? description, string venue, DateTime eventDate)
    {
        try
        {
            return await _eventManager.CreateEventAsync(organizerId, title, description, venue, eventDate);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "User", EntityId = organizerId.ToString() });
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "UNAUTHORIZED" });
        }
    }

    public async Task<EventDto> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            return await _eventManager.GetEventByIdAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
    }

    public async Task<List<EventDto>> GetEventsByOrganizerAsync(Guid organizerId)
    {
        try
        {
            return await _eventManager.GetEventsByOrganizerAsync(organizerId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "User", EntityId = organizerId.ToString() });
        }
    }

    public async Task<List<EventDto>> SearchEventsAsync(string? keyword, DateTime? date, string? status)
    {
        try
        {
            return await _eventManager.SearchEventsAsync(keyword, date, status);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
    }

    public async Task<EventDto> UpdateEventAsync(Guid eventId, string title, string? description, string venue, DateTime eventDate)
    {
        try
        {
            return await _eventManager.UpdateEventAsync(eventId, title, description, venue, eventDate);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }
}
