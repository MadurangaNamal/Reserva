using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserva.Data;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.AddDbContext<ReservaDbContext>(options =>
    options.UseSqlServer(configuration["ReservaDBConnection"]));

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Reserva Host is running...");
Console.ReadLine();