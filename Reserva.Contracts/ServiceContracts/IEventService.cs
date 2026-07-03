using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IEventService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    Task<EventDto> CreateEventAsync(Guid organizerId, string title, string? description, string venue, DateTime eventDate);

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    Task<EventDto> UpdateEventAsync(Guid eventId, string title, string? description, string venue, DateTime eventDate);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    Task<bool> CancelEventAsync(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<EventDto> GetEventByIdAsync(Guid eventId);

    [OperationContract]
    Task<List<EventDto>> SearchEventsAsync(string? keyword, DateTime? date, string? status);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<List<EventDto>> GetEventsByOrganizerAsync(Guid organizerId);
}
