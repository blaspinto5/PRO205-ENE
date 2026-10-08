namespace Codice.Infrastructure;

public sealed class StorageLease : IDisposable
{
    private readonly FileStream stream;
    private StorageLease(FileStream stream) => this.stream = stream;

    public static StorageLease Acquire(PortableStorage storage) => new(new FileStream(
        Path.Combine(storage.DataDirectory, ".instance"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));

    public void Dispose() => stream.Dispose();
}
