using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class BookingItemRequest
{
    [DataMember]
    public Guid CategoryId { get; set; }

    [DataMember]
    public int Quantity { get; set; }
}
