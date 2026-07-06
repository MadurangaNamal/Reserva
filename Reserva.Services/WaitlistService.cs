using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services;

public class WaitlistService : IWaitlistService
{
    private readonly IWaitlistManager _waitlistManager;

    public WaitlistService(IWaitlistManager waitlistManager)
    {
        _waitlistManager = waitlistManager ?? throw new ArgumentNullException(nameof(waitlistManager));
    }

    public async Task<int> GetWaitlistPositionAsync(Guid waitlistId)
    {
        try
        {
            return await _waitlistManager.GetWaitlistPositionAsync(waitlistId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Waitlist", EntityId = waitlistId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }

    public async Task<WaitlistDto> JoinWaitlistAsync(Guid userId, Guid eventId, Guid categoryId)
    {
        try
        {
            return await _waitlistManager.JoinWaitlistAsync(userId, eventId, categoryId);
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

    public async Task<bool> LeaveWaitlistAsync(Guid waitlistId)
    {
        try
        {
            return await _waitlistManager.LeaveWaitlistAsync(waitlistId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "Waitlist", EntityId = waitlistId.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            throw new FaultException<ReservaFault>(
                new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
        }
    }
}
