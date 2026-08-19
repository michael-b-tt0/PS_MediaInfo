namespace GetMediaInfo;

/// <summary>
/// Strongly typed metadata returned by Get-MediaFileInfo.
/// </summary>
public sealed class MediaInfoResult
{
    /// <summary>
    /// Gets the fully qualified filesystem path.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Gets the file name, including its extension.
    /// </summary>
    public string Name => Path.GetFileName(FullName);

    /// <summary>
    /// Gets the containing directory.
    /// </summary>
    public string? DirectoryName => Path.GetDirectoryName(FullName);

    /// <summary>
    /// Gets the extension without its leading dot.
    /// </summary>
    public string Extension =>
        Path.GetExtension(FullName).TrimStart('.').ToUpperInvariant();

    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Gets the file size in mebibytes.
    /// </summary>
    public double SizeMiB => FileSize / 1024d / 1024d;

    /// <summary>
    /// Gets the media duration.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    public string? ContainerFormat { get; init; }

    public int VideoStreamCount { get; init; }

    public bool HasVideo => VideoStreamCount > 0;

    public string? VideoCodec { get; init; }

    /// <summary>
    /// Gets the dimensions of the primary video stream.
    /// </summary>
    public MediaResolution? Resolution { get; init; }

    public string? FrameRateMode { get; init; }

    public double? FrameRate { get; init; }

    /// <summary>
    /// Gets the primary video bit rate in bits per second.
    /// </summary>
    public long? VideoBitRate { get; init; }

    public double? DisplayAspectRatio { get; init; }

    public string? FormatProfile { get; init; }

    public string? ScanType { get; init; }

    public string? ColorSpace { get; init; }

    public string? ColorRange { get; init; }

    public string? ColorPrimaries { get; init; }

    public string? TransferCharacteristics { get; init; }

    public string? MatrixCoefficients { get; init; }

    public int AudioStreamCount { get; init; }

    public bool HasAudio => AudioStreamCount > 0;

    public string? AudioCodec { get; init; }

    /// <summary>
    /// Gets the primary audio bit rate in bits per second.
    /// </summary>
    public long? AudioBitRate { get; init; }

    public string? AudioBitRateMode { get; init; }

    public string? Performer { get; init; }

    public string? Track { get; init; }

    public string? Album { get; init; }

    public string? RecordedDate { get; init; }

    public string? Genre { get; init; }

    public int TextStreamCount { get; init; }

    public string? TextFormats { get; init; }
}
