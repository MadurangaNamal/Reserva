# Reserva

Event reservation backend exposed as SOAP services (CoreWCF) over HTTP.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server

## Setup

```bash
git clone <repo-url>
cd Reserva

# Set your SQL Server connection string (user secrets)
dotnet user-secrets set "ReservaDBConnection" "Server=localhost;Database=Reserva;Trusted_Connection=True;TrustServerCertificate=True" --project Reserva.Host

# Apply migrations
dotnet ef database update --project Reserva.Data --startup-project Reserva.Host
```

## Run

```bash
dotnet run --project Reserva.Host
```

Default URL: `http://localhost:5000`

## Services

| Service | Endpoint |
|---------|----------|
| User | `/Service/UserService` |
| Event | `/Service/EventService` |
| Ticket category | `/Service/TicketCategoryService` |
| Booking | `/Service/BookingService` |
| Waitlist | `/Service/WaitlistService` |
| Report | `/Service/ReportService` |

WSDL: append `?wsdl` to any endpoint (e.g. `http://localhost:5000/Service/EventService?wsdl`).

## Sample request

Search events (SOAP):

```bash
curl -X POST http://localhost:5000/Service/EventService \
  -H "Content-Type: text/xml; charset=utf-8" \
  -H 'SOAPAction: "http://tempuri.org/IEventService/SearchEventsAsync"' \
  -d '<?xml version="1.0" encoding="utf-8"?>
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <SearchEventsAsync xmlns="http://tempuri.org/">
      <keyword>concert</keyword>
    </SearchEventsAsync>
  </s:Body>
</s:Envelope>'
```

## Solution layout

- `Reserva.Host` — WCF host
- `Reserva.Services` — service implementations
- `Reserva.Core` — business logic
- `Reserva.Data` — EF Core + migrations
- `Reserva.Contracts` — DTOs and service contracts
