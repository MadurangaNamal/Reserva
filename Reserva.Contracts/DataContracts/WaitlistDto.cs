using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class WaitlistDto
{
    [DataMember]
    public Guid WaitlistId { get; set; }

    [DataMember]
    public Guid UserId { get; set; }

    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public Guid CategoryId { get; set; }

    [DataMember]
    public DateTime RequestedAt { get; set; }

    [DataMember]
    public string Status { get; set; } = default!;
}
