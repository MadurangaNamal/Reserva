using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using System.ServiceModel;

namespace Reserva.Contracts.ServiceContracts;

[ServiceContract]
public interface ITicketCategoryService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    Task<TicketCategoryDto> AddCategoryAsync(Guid eventId, string name, decimal price, int totalSeats);

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    Task<TicketCategoryDto> UpdateCategoryAsync(Guid categoryId, string name, decimal price);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Task<List<TicketCategoryDto>> GetCategoriesByEventAsync(Guid eventId);
}
