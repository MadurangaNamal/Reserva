using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IWaitlistService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ReservaFault))]
    Task<WaitlistDto> JoinWaitlistAsync(Guid userId, Guid eventId, Guid categoryId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<bool> LeaveWaitlistAsync(Guid waitlistId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<int> GetWaitlistPositionAsync(Guid waitlistId);
}
