using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.EntityFrameworkCore;
using Reserva.Contracts.ServiceContracts;
using Reserva.Core.Interfaces;
using Reserva.Core.Managers;
using Reserva.Core.Mapping;
using Reserva.Data;
using Reserva.Host.Extensions;
using Reserva.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>();

builder.Services.AddDbContext<ReservaDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ReservaDBConnection"]));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ReservaMappingProfile>();
});

builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IEventManager, EventManager>();
builder.Services.AddScoped<ITicketCategoryManager, TicketCategoryManager>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddScoped<IWaitlistManager, WaitlistManager>();
builder.Services.AddScoped<IReportManager, ReportManager>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<TicketCategoryService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<WaitlistService>();
builder.Services.AddScoped<ReportService>();

// CoreWCF
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

// CoreWCF Middleware
app.UseServiceModel(serviceBuilder =>
{
    var binding = new BasicHttpBinding();
    var includeExceptionDetails = app.Environment.IsDevelopment();

    serviceBuilder.AddService<UserService>();
    serviceBuilder.EnableDebugBehavior<UserService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<UserService, IUserService>(binding, "/Service/UserService");

    serviceBuilder.AddService<EventService>();
    serviceBuilder.EnableDebugBehavior<EventService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<EventService, IEventService>(binding, "/Service/EventService");

    serviceBuilder.AddService<TicketCategoryService>();
    serviceBuilder.EnableDebugBehavior<TicketCategoryService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<TicketCategoryService, ITicketCategoryService>(binding, "/Service/TicketCategoryService");

    serviceBuilder.AddService<BookingService>();
    serviceBuilder.EnableDebugBehavior<BookingService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<BookingService, IBookingService>(binding, "/Service/BookingService");

    serviceBuilder.AddService<WaitlistService>();
    serviceBuilder.EnableDebugBehavior<WaitlistService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<WaitlistService, IWaitlistService>(binding, "/Service/WaitlistService");

    serviceBuilder.AddService<ReportService>();
    serviceBuilder.EnableDebugBehavior<ReportService>(includeExceptionDetails);
    serviceBuilder.AddServiceEndpoint<ReportService, IReportService>(binding, "/Service/ReportService");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

await app.RunAsync();