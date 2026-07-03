using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IBookingService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    Task<BookingDto> CreateBookingAsync(Guid userId, Guid eventId, List<BookingItemRequest> items);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    Task<bool> CancelBookingAsync(Guid bookingId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<BookingDto> GetBookingByIdAsync(Guid bookingId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<List<BookingDto>> GetBookingHistoryByUserAsync(Guid userId);
}
