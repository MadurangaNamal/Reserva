using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class TicketCategoryDto
{
    [DataMember]
    public Guid CategoryId { get; set; }

    [DataMember]
    public string Name { get; set; } = default!;

    [DataMember]
    public Guid EventId { get; set; } = default!;

    [DataMember]
    public decimal Price { get; set; }

    [DataMember]
    public int TotalSeats { get; set; }

    [DataMember]
    public int AvailableSeats { get; set; }
}
