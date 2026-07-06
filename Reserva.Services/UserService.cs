using Reserva.Contracts.DataContracts;
using Reserva.Contracts.FaultContracts;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using System.ServiceModel;

namespace Reserva.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;

        public UserService(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserDto> GetByIdAsync(Guid userId)
        {
            try
            {
                return await _userManager.GetByIdAsync(userId);
            }
            catch (KeyNotFoundException ex)
            {
                throw new FaultException<NotFoundFault>(
                    new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "User", EntityId = userId.ToString() });
            }
        }

        public async Task<UserDto> RegisterAsync(string fullName, string email, string password, string? phone, string role)
        {
            try
            {
                return await _userManager.RegisterAsync(fullName, email, password, phone, role);
            }
            catch (ArgumentException ex)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<ReservaFault>(
                    new ReservaFault { Message = ex.Message, Code = "OPERATION_ERROR" });
            }
        }

        public async Task<UserDto> UpdateProfileAsync(Guid userId, string fullName, string? phone)
        {
            try
            {
                return await _userManager.UpdateProfileAsync(userId, fullName, phone);
            }
            catch (KeyNotFoundException ex)
            {
                throw new FaultException<NotFoundFault>(
                    new NotFoundFault { Message = ex.Message, Code = "NOT_FOUND", EntityType = "User", EntityId = userId.ToString() });
            }
            catch (ArgumentException ex)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { Message = ex.Message, Code = "VALIDATION_ERROR", Errors = new List<string> { ex.Message } });
            }
        }
    }
}
