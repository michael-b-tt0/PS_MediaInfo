namespace GetMediaInfo;

/// <summary>
/// Contains file and container information shared by all media types.
/// </summary>
public sealed class GeneralInfo
{
    /// <summary>
    /// Gets the filesystem object that was inspected.
    /// </summary>
    public required FileInfo File { get; init; }

    public required string FullName { get; init; }

    public string Name => Path.GetFileName(FullName);

    /// <summary>
    /// Gets the file name without its extension.
    /// </summary>
    public string BaseName => Path.GetFileNameWithoutExtension(FullName);

    public string? DirectoryName => Path.GetDirectoryName(FullName);

    public string Extension =>
        Path.GetExtension(FullName).TrimStart('.').ToUpperInvariant();

    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    public long Length { get; init; }

    /// <summary>
    /// Gets the filesystem last-write time at inspection.
    /// </summary>
    public DateTime LastWriteTime { get; init; }

    public double SizeMiB => Length / 1024d / 1024d;

    public TimeSpan? Duration { get; init; }

    public string? ContainerFormat { get; init; }
}
