using System;
using System.IO;

class TempFile : IDisposable
{
    private bool disposedValue;

    public string FullName { get; protected set; }
    public string Extension
    {
        get => Path.GetExtension(FullName);
        set => FullName = ChangeExtension(value);
    }
    private string ChangeExtension(string NewExtension)
    {
        var Extension = this.Extension;
        if (!NewExtension.StartsWith("."))
            NewExtension = string.Empty;
        return FullName.Substring(0, FullName.Length - Extension.Length) + NewExtension;
    }
    public TempFile(string Extension)
    {
        FullName = Path.GetTempFileName();
        this.Extension = Extension;
    }
    public bool Exists() => File.Exists(FullName);
    public void Delete()
    {
        if (Exists())
            File.Delete(FullName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // noop
            }
            Delete();
            disposedValue = true;
        }
    }

    ~TempFile()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
