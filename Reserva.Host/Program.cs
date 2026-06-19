using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserva.Core.Interfaces;
using Reserva.Core.Managers;
using Reserva.Core.Mapping;
using Reserva.Data;

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

services.BuildServiceProvider();

Console.WriteLine("Reserva Host is running...");
Console.ReadLine();