using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services;

public class BookingService : IBookingService
{
    private readonly IBookingManager _bookingManager;

    public BookingService(IBookingManager bookingManager)
    {
        _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        try
        {
            return await _bookingManager.CancelBookingAsync(bookingId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Booking", EntityId = bookingId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }

    public async Task<BookingDto> CreateBookingAsync(Guid userId, Guid eventId, List<BookingItemRequest> items)
    {
        try
        {
            return await _bookingManager.CreateBookingAsync(userId, eventId, items);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Resource", EntityId = string.Empty });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }

    public async Task<BookingDto> GetBookingByIdAsync(Guid bookingId)
    {
        try
        {
            return await _bookingManager.GetBookingByIdAsync(bookingId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Booking", EntityId = bookingId.ToString() });
        }
    }

    public async Task<List<BookingDto>> GetBookingHistoryByUserAsync(Guid userId)
    {
        try
        {
            return await _bookingManager.GetBookingHistoryByUserAsync(userId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "User", EntityId = userId.ToString() });
        }
    }
}
