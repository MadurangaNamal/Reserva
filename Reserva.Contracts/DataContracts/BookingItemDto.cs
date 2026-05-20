using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class BookingItemDto
{
    [DataMember]
    public Guid BookingItemId { get; set; }

    [DataMember]
    public string CategoryName { get; set; } = default!;

    [DataMember]
    public int Quantity { get; set; }

    [DataMember]
    public decimal Price { get; set; }

    [DataMember]
    public Guid BookingId { get; set; }

    [DataMember]
    public Guid CategoryId { get; set; }
}
