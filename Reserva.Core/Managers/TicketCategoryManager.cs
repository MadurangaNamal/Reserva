using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.DataContracts;
using Reserva.Core.Interfaces;
using Reserva.Data;
using Reserva.Data.Entities;
using Reserva.Data.enums;

namespace Reserva.Core.Managers;

public class TicketCategoryManager : ITicketCategoryManager
{
    private readonly ReservaDbContext _dbContext;
    private readonly IMapper _mapper;

    public TicketCategoryManager(ReservaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TicketCategoryDto> AddCategoryAsync(Guid eventId, string name, decimal price, int totalSeats)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (totalSeats <= 0)
            throw new ArgumentException("Total seats must be greater than zero.");

        var existingEvent = await _dbContext.Events
           .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (existingEvent is null)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        if (existingEvent.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Cannot add a ticket category to a cancelled event.");

        if (existingEvent.Status == EventStatus.Completed)
            throw new InvalidOperationException("Cannot add a ticket category to a completed event.");

        var nameExists = await _dbContext.TicketCategories
            .AnyAsync(tc => tc.EventId == eventId && tc.Name == name);

        if (nameExists)
            throw new InvalidOperationException($"A category named '{name}' already exists for this event.");

        var category = new TicketCategory
        {
            CategoryId = Guid.NewGuid(),
            EventId = eventId,
            Name = name,
            Price = price,
            TotalSeats = totalSeats,
            AvailableSeats = totalSeats
        };

        _dbContext.TicketCategories.Add(category);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<TicketCategoryDto>(category);
    }

    public async Task<List<TicketCategoryDto>> GetCategoriesByEventAsync(Guid eventId)
    {
        var eventExists = await _dbContext.Events
            .AnyAsync(e => e.EventId == eventId);

        if (!eventExists)
            throw new KeyNotFoundException($"Event with ID '{eventId}' was not found.");

        var categories = await _dbContext.TicketCategories
            .Where(tc => tc.EventId == eventId)
            .ToListAsync();

        return _mapper.Map<List<TicketCategoryDto>>(categories);
    }

    public async Task<TicketCategoryDto> UpdateCategoryAsync(Guid categoryId, string name, decimal price)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        var category = await _dbContext.TicketCategories
            .Include(c => c.Event)
            .FirstOrDefaultAsync(e => e.CategoryId == categoryId);

        if (category == null)
            throw new KeyNotFoundException($"Ticket Category with ID '{categoryId}' was not found.");

        if (category.Event.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a category on a cancelled event.");

        if (category.Event.Status == EventStatus.Completed)
            throw new InvalidOperationException("Cannot update a category on a completed event.");

        var nameExists = await _dbContext.TicketCategories
            .AnyAsync(tc => tc.EventId == category.EventId && tc.Name == name && tc.CategoryId != categoryId);

        if (nameExists)
            throw new InvalidOperationException($"A category named '{name}' already exists for this event.");

        category.Name = name;
        category.Price = price;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TicketCategoryDto>(category);
    }
}
