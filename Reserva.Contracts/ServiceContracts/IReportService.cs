using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

public interface IReportService
{
    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    int GetTicketsSoldByEvent(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    decimal GetRevenueByEvent(Guid eventId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    BookingSummaryDto GetBookingsSummary(Guid eventId);
}
