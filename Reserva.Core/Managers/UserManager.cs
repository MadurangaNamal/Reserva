using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.Entities;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class UserManager : IUserManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserDto> GetByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> RegisterAsync(string fullName, string email, string password, string? phone, string role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName, nameof(fullName));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email);

        if (existingUser != null)
            throw new InvalidOperationException($"A user with email '{email}' already exists.");

        if (!Enum.TryParse<UserRole>(role, ignoreCase: true, out var userRole))
            throw new ArgumentException($"Invalid role '{role}'. Valid roles are: Attendee, Organizer, Admin.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = hashedPassword,
            Phone = phone,
            Role = userRole,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateProfileAsync(Guid userId, string fullName, string? phone)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        ArgumentException.ThrowIfNullOrWhiteSpace(fullName, nameof(fullName));

        user.FullName = fullName;
        user.Phone = phone;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }
}
