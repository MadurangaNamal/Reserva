using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using Reserva.Core.Managers;
using Reserva.Core.Mapping;
using Reserva.Data;
using Reserva.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.AddDbContext<ReservaDbContext>(options =>
    options.UseSqlServer(configuration["ReservaDBConnection"]));

services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ReservaMappingProfile>();
});

services.AddScoped<IUserManager, UserManager>();
services.AddScoped<IEventManager, EventManager>();
services.AddScoped<ITicketCategoryManager, TicketCategoryManager>();
services.AddScoped<IBookingManager, BookingManager>();
services.AddScoped<IWaitlistManager, WaitlistManager>();
services.AddScoped<IReportManager, ReportManager>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<IEventService, EventService>();
services.AddScoped<ITicketCategoryService, TicketCategoryService>();
services.AddScoped<IBookingService, BookingService>();
services.AddScoped<IWaitlistService, WaitlistService>();
services.AddScoped<IReportService, ReportService>();

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Reserva Host is running...");
Console.ReadLine();