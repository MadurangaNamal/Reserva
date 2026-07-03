using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IReportService
{
    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<int> GetTicketsSoldByEventAsync(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<decimal> GetRevenueByEventAsync(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<BookingSummaryDto> GetBookingsSummaryAsync(Guid eventId);
}
