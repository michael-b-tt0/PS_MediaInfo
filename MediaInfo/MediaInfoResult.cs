namespace GetMediaInfo;

/// <summary>
/// Base class for strongly typed metadata returned by Get-MediaFileInfo.
/// </summary>
public abstract class MediaInfoResult
{
    /// <summary>
    /// Gets information shared by every supported media type.
    /// </summary>
    public required GeneralInfo General { get; init; }

    /// <summary>
    /// Gets the dominant media type used to select this result shape.
    /// </summary>
    public abstract MediaType MediaType { get; }
}

/// <summary>
/// Describes a file whose dominant content is video.
/// </summary>
public sealed class VideoMediaInfoResult : MediaInfoResult
{
    public override MediaType MediaType => MediaType.Video;

    public int VideoStreamCount { get; init; }

    public string? VideoCodec { get; init; }

    public MediaResolution? Resolution { get; init; }

    public string? FrameRateMode { get; init; }

    public double? FrameRate { get; init; }

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

    public string? AudioCodec { get; init; }

    public long? AudioBitRate { get; init; }

    public string? AudioBitRateMode { get; init; }

    public int TextStreamCount { get; init; }

    public string? TextFormats { get; init; }
}

/// <summary>
/// Describes a file whose dominant content is audio.
/// </summary>
public sealed class AudioMediaInfoResult : MediaInfoResult
{
    public override MediaType MediaType => MediaType.Audio;

    public int AudioStreamCount { get; init; }

    public string? AudioCodec { get; init; }

    public long? AudioBitRate { get; init; }

    public string? AudioBitRateMode { get; init; }

    public int? Channels { get; init; }

    public int? SamplingRate { get; init; }

    public int ArtworkCount { get; init; }

    public string? Title { get; init; }

    public string? Album { get; init; }

    public string? Artist { get; init; }

    public int? TrackNumber { get; init; }

    public int? TrackCount { get; init; }

    public string? Genre { get; init; }

    public string? RecordedDate { get; init; }
}

/// <summary>
/// Describes a file whose dominant content is one or more images.
/// </summary>
public sealed class ImageMediaInfoResult : MediaInfoResult
{
    public override MediaType MediaType => MediaType.Image;

    public int ImageStreamCount { get; init; }

    public string? ImageFormat { get; init; }

    public MediaResolution? Resolution { get; init; }

    public int? BitDepth { get; init; }

    public string? ColorSpace { get; init; }

    public string? Title { get; init; }
}

/// <summary>
/// Describes a file that MediaInfo opened but could not classify as video,
/// audio, or image media.
/// </summary>
public sealed class UnknownMediaInfoResult : MediaInfoResult
{
    public override MediaType MediaType => MediaType.Unknown;
}
