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
    EventDto CreateEvent(Guid organizerId, string title, string? description, string venue, DateTime eventDate);

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    EventDto UpdateEvent(Guid eventId, string title, string? description, string venue, DateTime eventDate);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    bool CancelEvent(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    EventDto GetEventById(Guid eventId);

    [OperationContract]
    List<EventDto> SearchEvents(string? keyword, DateTime? date, string? status);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    List<EventDto> GetEventsByOrganizer(Guid organizerId);
}
