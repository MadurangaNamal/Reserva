using System.Runtime.Serialization;

namespace Reserva.Contracts.FaultContracts;

[DataContract]
public class ValidationFault
{
    [DataMember]
    public string Message { get; set; } = default!;

    [DataMember]
    public string Code { get; set; } = default!;

    [DataMember]
    public List<string>? Errors { get; set; }
}
