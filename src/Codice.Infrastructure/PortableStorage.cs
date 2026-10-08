using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Codice.Infrastructure;

public sealed class PortableStorage
{
    public string Root { get; }
    public string DataDirectory => Path.Combine(Root, "datos");
    public string DatabasePath => Path.Combine(DataDirectory, "codice.db");

    public PortableStorage(string root) => Root = Path.GetFullPath(root);

    public void PrepareDirectory()
    {
        Directory.CreateDirectory(DataDirectory);
        var probe = Path.Combine(DataDirectory, $".write-{Guid.NewGuid():N}");
        using var stream = new FileStream(probe, FileMode.CreateNew, FileAccess.Write, FileShare.None,
            1, FileOptions.DeleteOnClose);
        stream.WriteByte(0);
    }

    public CodiceDbContext CreateContext()
    {
        var connection = new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath, ForeignKeys = true, Pooling = false, DefaultTimeout = 5
        };
        return new CodiceDbContext(new DbContextOptionsBuilder<CodiceDbContext>()
            .UseSqlite(connection.ToString()).Options);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var context = CreateContext();
        await context.Database.MigrateAsync(cancellationToken);
    }
}
