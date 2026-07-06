using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services;

public class ReportService : IReportService
{
    private readonly IReportManager _reportManager;

    public ReportService(IReportManager reportManager)
    {
        _reportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));
    }

    public async Task<BookingSummaryDto> GetBookingsSummaryAsync(Guid eventId)
    {
        try
        {
            return await _reportManager.GetBookingsSummaryAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
    }

    public async Task<decimal> GetRevenueByEventAsync(Guid eventId)
    {
        try
        {
            return await _reportManager.GetRevenueByEventAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
    }

    public async Task<int> GetTicketsSoldByEventAsync(Guid eventId)
    {
        try
        {
            return await _reportManager.GetTicketsSoldByEventAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
    }
}
