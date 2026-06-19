using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface IWaitlistManager
{
    Task<WaitlistDto> JoinWaitlistAsync(Guid userId, Guid eventId, Guid categoryId);
    Task<bool> LeaveWaitlistAsync(Guid waitlistId);
    Task<int> GetWaitlistPositionAsync(Guid waitlistId);
}
