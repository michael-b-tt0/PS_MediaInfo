namespace GetMediaInfo;

/// <summary>
/// Contains file and container information shared by all media types.
/// </summary>
public sealed class GeneralInfo
{
    public required string FullName { get; init; }

    public string Name => Path.GetFileName(FullName);

    public string? DirectoryName => Path.GetDirectoryName(FullName);

    public string Extension =>
        Path.GetExtension(FullName).TrimStart('.').ToUpperInvariant();

    public long FileSize { get; init; }

    public double SizeMiB => FileSize / 1024d / 1024d;

    public TimeSpan? Duration { get; init; }

    public string? ContainerFormat { get; init; }
}
