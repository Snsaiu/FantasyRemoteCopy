namespace AirTransfer.Models;

/// <summary>
/// 带有进度的<see cref="Stream"/>
/// </summary>
public class ProgressStream : Stream
{
    private readonly Stream stream;

    private long totalBytesRead;

    public event Action<long, long> ProgressChanged;

    public ProgressStream(Stream stream)
    {
        this.stream = stream;
    }

    public override void Flush()
    {
        stream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var bytesRead = stream.Read(buffer, offset, count);
        totalBytesRead += bytesRead;
        ProgressChanged?.Invoke(totalBytesRead, Length);
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        stream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        stream.Write(buffer, offset, count);
        totalBytesRead += count;
        ProgressChanged?.Invoke(totalBytesRead, Length);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            stream.Dispose();
        }

        base.Dispose(disposing);
    }

    public override long Length => stream.Length;
    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;

    public override long Position
    {
        get => stream.Position;
        set => stream.Position = value;
    }
}