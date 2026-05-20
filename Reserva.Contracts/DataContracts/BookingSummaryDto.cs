using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class BookingSummaryDto
{
    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public string EventTitle { get; set; } = default!;

    [DataMember]
    public int TotalBookings { get; set; }

    [DataMember]
    public int ConfirmedBookings { get; set; }

    [DataMember]
    public int CancelledBookings { get; set; }

    [DataMember]
    public decimal TotalRevenue { get; set; }
}
