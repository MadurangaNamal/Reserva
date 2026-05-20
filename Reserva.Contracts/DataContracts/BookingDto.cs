using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class BookingDto
{
    [DataMember]
    public Guid BookingId { get; set; }

    [DataMember]
    public DateTime BookingDate { get; set; }

    [DataMember]
    public string Status { get; set; } = default!;

    [DataMember]
    public decimal TotalAmount { get; set; }

    [DataMember]
    public Guid UserId { get; set; }

    [DataMember]
    public Guid EventId { get; set; }

    public List<BookingItemDto> Items { get; set; } = [];
}
