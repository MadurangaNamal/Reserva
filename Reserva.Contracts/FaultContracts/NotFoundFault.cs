using System.Runtime.Serialization;

namespace Reserva.Contracts.FaultContracts;

[DataContract]
public class NotFoundFault
{
    [DataMember]
    public string Message { get; set; } = default!;

    [DataMember]
    public string Code { get; set; } = default!;

    [DataMember]
    public string EntityType { get; set; } = default!;

    [DataMember]
    public string EntityId { get; set; } = default!;
}
