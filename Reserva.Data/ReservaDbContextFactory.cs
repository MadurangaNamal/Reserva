using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Reserva.Data;

public class ReservaDbContextFactory : IDesignTimeDbContextFactory<ReservaDbContext>
{
    public ReservaDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ReservaDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ReservaDbContext>();
        optionsBuilder.UseSqlServer(configuration["ReservaDBConnection"]);

        return new ReservaDbContext(optionsBuilder.Options);
    }
}
