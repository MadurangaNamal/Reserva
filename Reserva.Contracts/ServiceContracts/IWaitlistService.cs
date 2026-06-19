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
    WaitlistDto JoinWaitlist(Guid userId, Guid eventId, Guid categoryId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    bool LeaveWaitlist(Guid waitlistId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    int GetWaitlistPosition(Guid waitlistId);
}
