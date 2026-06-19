using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface IReportManager
{
    Task<int> GetTicketsSoldByEventAsync(Guid eventId);
    Task<decimal> GetRevenueByEventAsync(Guid eventId);
    Task<BookingSummaryDto> GetBookingsSummaryAsync(Guid eventId);
}
