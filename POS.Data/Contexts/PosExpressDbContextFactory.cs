using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace POS.Data.Contexts;

// Usado por las herramientas de "dotnet ef" (migrations/database update) para poder construir
// el DbContext sin depender del contenedor de DI de POS.Presentation.
// La cadena de conexión debe coincidir con la registrada en appsettings.json de POS.Presentation.
public class PosExpressDbContextFactory : IDesignTimeDbContextFactory<PosExpressDbContext>
{
    private const string LocalDbConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=PosExpress;Trusted_Connection=True;TrustServerCertificate=True;";

    public PosExpressDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PosExpressDbContext>();
        optionsBuilder.UseSqlServer(LocalDbConnectionString);
        return new PosExpressDbContext(optionsBuilder.Options);
    }
}
