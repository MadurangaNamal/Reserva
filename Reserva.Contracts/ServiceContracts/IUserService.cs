using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface IUserService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    Task<UserDto> RegisterAsync(string fullName, string email, string password, string? phone, string role);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<UserDto> GetByIdAsync(Guid userId);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    [FaultContract(typeof(ValidationFault))]
    Task<UserDto> UpdateProfileAsync(Guid userId, string fullName, string? phone);
}
