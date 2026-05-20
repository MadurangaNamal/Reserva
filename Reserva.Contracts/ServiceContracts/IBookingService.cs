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
    BookingDto CreateBooking(Guid userId, Guid eventId, List<BookingItemRequest> items);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    bool CancelBooking(Guid bookingId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    BookingDto GetBookingById(Guid bookingId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    List<BookingDto> GetBookingHistoryByUser(Guid userId);
}
