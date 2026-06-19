using AutoMapper;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;

namespace Reserva.Core.Managers;

public class WaitlistManager : IWaitlistManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public WaitlistManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<int> GetWaitlistPositionAsync(Guid waitlistId)
    {
        throw new NotImplementedException();
    }

    public async Task<WaitlistDto> JoinWaitlistAsync(Guid userId, Guid eventId, Guid categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LeaveWaitlistAsync(Guid waitlistId)
    {
        throw new NotImplementedException();
    }
}
