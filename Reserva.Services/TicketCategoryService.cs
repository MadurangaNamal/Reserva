using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services;

public class TicketCategoryService : ITicketCategoryService
{
    private readonly ITicketCategoryManager _ticketCategoryManager;

    public TicketCategoryService(ITicketCategoryManager ticketCategoryManager)
    {
        _ticketCategoryManager = ticketCategoryManager ?? throw new ArgumentNullException(nameof(ticketCategoryManager));
    }

    public async Task<TicketCategoryDto> AddCategoryAsync(Guid eventId, string name, decimal price, int totalSeats)
    {
        try
        {
            return await _ticketCategoryManager.AddCategoryAsync(eventId, name, price, totalSeats);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }

    public async Task<List<TicketCategoryDto>> GetCategoriesByEventAsync(Guid eventId)
    {
        try
        {
            return await _ticketCategoryManager.GetCategoriesByEventAsync(eventId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Event", EntityId = eventId.ToString() });
        }
    }

    public async Task<TicketCategoryDto> UpdateCategoryAsync(Guid categoryId, string name, decimal price)
    {
        try
        {
            return await _ticketCategoryManager.UpdateCategoryAsync(categoryId, name, price);
        }
        catch (ArgumentException ex)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "TicketCategory", EntityId = categoryId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }
}
