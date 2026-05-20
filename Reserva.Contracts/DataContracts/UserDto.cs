using System.Runtime.Serialization;

namespace Reserva.Contracts.DataContracts;

[DataContract]
public class UserDto
{
    [DataMember]
    public Guid UserId { get; set; }

    [DataMember]
    public string FullName { get; set; } = default!;

    [DataMember]
    public string Email { get; set; } = default!;

    [DataMember]
    public string Phone { get; set; } = default!;

    [DataMember]
    public string Role { get; set; } = default!;

    [DataMember]
    public DateTime CreatedAt { get; set; }
}
