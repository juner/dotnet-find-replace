using System;
using System.IO;

class TempFile : IDisposable
{
    private bool disposedValue;

    public string Path { get; protected set; }
    public string Extension
    {
        get => System.IO.Path.GetExtension(Path);
        set => Path = ChangeExtension(value);
    }
    public string DirectoryName => System.IO.Path.GetDirectoryName(Path)!;
    public string FileNameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(Path);
    private string ChangeExtension(string NewExtension)
    {
        var Extension = this.Extension;
        if (!NewExtension.StartsWith("."))
            NewExtension = string.Empty;
        if (NewExtension == Extension)
            return this.Path;
        var FullName = this.Path.AsSpan();
        var Separator = System.IO.Path.DirectorySeparatorChar.ToString().AsSpan();
        return string.Concat(System.IO.Path.GetDirectoryName(FullName), Separator, System.IO.Path.GetFileNameWithoutExtension(FullName)) + NewExtension;
    }
    public TempFile(string Extension)
    {
        Path = System.IO.Path.GetTempFileName();
        this.Extension = Extension;
    }
    public bool Exists() => File.Exists(Path);
    public void Delete()
    {
        if (Exists())
            File.Delete(Path);
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
    public override string ToString() => Path;

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
