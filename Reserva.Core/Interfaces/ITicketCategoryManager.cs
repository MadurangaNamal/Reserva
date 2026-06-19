using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface ITicketCategoryManager
{
    Task<TicketCategoryDto> AddCategoryAsync(Guid eventId, string name, decimal price, int totalSeats);
    Task<TicketCategoryDto> UpdateCategoryAsync(Guid categoryId, string name, decimal price);
    Task<List<TicketCategoryDto>> GetCategoriesByEventAsync(Guid eventId);

}
