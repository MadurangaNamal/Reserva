using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;

namespace Reserva.Core.Managers;

public class ReportManager : IReportManager
{
    public Task<BookingSummaryDto> GetBookingsSummaryAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetRevenueByEventAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTicketsSoldByEventAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}
