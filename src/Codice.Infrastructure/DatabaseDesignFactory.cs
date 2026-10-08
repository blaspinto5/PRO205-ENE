using Microsoft.EntityFrameworkCore.Design;

namespace Codice.Infrastructure;

public sealed class DatabaseDesignFactory : IDesignTimeDbContextFactory<CodiceDbContext>
{
    public CodiceDbContext CreateDbContext(string[] args) =>
        new PortableStorage(Path.Combine(Path.GetTempPath(), "Codice-schema-design")).CreateContext();
}
