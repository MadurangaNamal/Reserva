using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class EventDto
{
    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public string Title { get; set; } = default!;

    [DataMember]
    public string Description { get; set; } = default!;

    [DataMember]
    public string Venue { get; set; } = default!;

    [DataMember]
    public DateTime EventDate { get; set; }

    [DataMember]
    public Guid OrganizerId { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }

    [DataMember]
    public string Status = default!;
}
