using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface IBookingManager
{
    Task<BookingDto> CreateBookingAsync(Guid userId, Guid eventId, List<BookingItemRequest> items);
    Task<bool> CancelBookingAsync(Guid bookingId);
    Task<BookingDto> GetBookingByIdAsync(Guid bookingId);
    Task<List<BookingDto>> GetBookingHistoryByUserAsync(Guid userId);
}
