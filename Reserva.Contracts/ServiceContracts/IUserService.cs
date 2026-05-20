using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IUserService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    UserDto Register(string fullName, string email, string password, string? phone, string role);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    UserDto GetById(Guid userId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ValidationFault))]
    UserDto UpdateProfile(Guid userId, string fullName, string? phone);
}
