using Reserva.Contracts.DataContracts;

namespace Reserva.Core.Interfaces;

public interface IUserManager
{
    Task<UserDto> RegisterAsync(string fullName, string email, string password, string? phone, string role);
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<UserDto> UpdateProfileAsync(Guid userId, string fullName, string? phone);
}
