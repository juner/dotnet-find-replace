using System;
using System.IO;

class TempDirectory : IDisposable
{
    private bool disposedValue;
    public string FullName { get; protected set; }
    public TempDirectory(bool CreateNew = true)
    {
        FullName = Path.GetTempFileName();
        File.Delete(FullName);
        if (CreateNew)
            Directory.CreateDirectory(FullName);
    }
    public bool Exists() => Directory.Exists(FullName);
    public void Delete(bool Recursive = false)
    {
        if (Exists())
            Directory.Delete(FullName, Recursive);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // noop
            }
            Delete(true);
            disposedValue = true;
        }
    }

    ~TempDirectory()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
