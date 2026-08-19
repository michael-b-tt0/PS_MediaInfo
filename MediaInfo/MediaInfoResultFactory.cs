using System.Globalization;

namespace GetMediaInfo;

internal static class MediaInfoResultFactory
{
    internal static MediaInfoResult Create(MediaInfoReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        GeneralInfo general = CreateGeneralInfo(reader);
        int videoStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Video);
        int audioStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Audio);
        int imageStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Image);

        // A file may expose multiple stream kinds. The dominant playable type
        // wins, so embedded album artwork does not turn an audio file into an
        // image result.
        if (videoStreamCount > 0)
        {
            return CreateVideoResult(reader, general, videoStreamCount, audioStreamCount);
        }

        if (audioStreamCount > 0)
        {
            return CreateAudioResult(reader, general, audioStreamCount, imageStreamCount);
        }

        if (imageStreamCount > 0)
        {
            return CreateImageResult(reader, general, imageStreamCount);
        }

        return new UnknownMediaInfoResult { General = general };
    }

    private static GeneralInfo CreateGeneralInfo(MediaInfoReader reader)
    {
        FileInfo file = new(reader.FilePath);

        return new GeneralInfo
        {
            FullName = file.FullName,
            FileSize = file.Length,
            Duration = ParseDuration(GetInfo(reader, MediaInfoStreamKind.General, 0, "Duration")),
            ContainerFormat = GetInfo(reader, MediaInfoStreamKind.General, 0, "Format/String"),
        };
    }

    private static VideoMediaInfoResult CreateVideoResult(
        MediaInfoReader reader,
        GeneralInfo general,
        int videoStreamCount,
        int audioStreamCount)
    {
        string? videoBitRate = GetInfo(reader, MediaInfoStreamKind.Video, 0, "BitRate") ??
            GetInfo(reader, MediaInfoStreamKind.General, 0, "OverallBitRate");
        int? width = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Video, 0, "Width"));
        int? height = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Video, 0, "Height"));

        return new VideoMediaInfoResult
        {
            General = general,
            VideoStreamCount = videoStreamCount,
            VideoCodec = GetInfo(reader, MediaInfoStreamKind.Video, 0, "Format/String"),
            Resolution = CreateResolution(width, height),
            FrameRateMode = GetInfo(reader, MediaInfoStreamKind.Video, 0, "FrameRate_Mode"),
            FrameRate = ParseDouble(GetInfo(reader, MediaInfoStreamKind.Video, 0, "FrameRate")),
            VideoBitRate = ParseInt64(videoBitRate),
            DisplayAspectRatio = ParseDouble(
                GetInfo(reader, MediaInfoStreamKind.Video, 0, "DisplayAspectRatio")),
            FormatProfile = GetInfo(reader, MediaInfoStreamKind.Video, 0, "Format_Profile"),
            ScanType = GetInfo(reader, MediaInfoStreamKind.Video, 0, "ScanType"),
            ColorSpace = GetInfo(reader, MediaInfoStreamKind.Video, 0, "ColorSpace"),
            ColorRange = GetInfo(reader, MediaInfoStreamKind.Video, 0, "colour_range"),
            ColorPrimaries = GetInfo(reader, MediaInfoStreamKind.Video, 0, "colour_primaries"),
            TransferCharacteristics = GetInfo(
                reader,
                MediaInfoStreamKind.Video,
                0,
                "transfer_characteristics"),
            MatrixCoefficients = GetInfo(
                reader,
                MediaInfoStreamKind.Video,
                0,
                "matrix_coefficients"),
            AudioStreamCount = audioStreamCount,
            AudioCodec = audioStreamCount > 0
                ? GetInfo(reader, MediaInfoStreamKind.Audio, 0, "Format/String")
                : null,
            AudioBitRate = audioStreamCount > 0
                ? ParseInt64(GetInfo(reader, MediaInfoStreamKind.Audio, 0, "BitRate"))
                : null,
            AudioBitRateMode = audioStreamCount > 0
                ? GetInfo(reader, MediaInfoStreamKind.Audio, 0, "BitRate_Mode")
                : null,
            TextStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Text),
            TextFormats = GetInfo(reader, MediaInfoStreamKind.General, 0, "Text_Format_List"),
        };
    }

    private static AudioMediaInfoResult CreateAudioResult(
        MediaInfoReader reader,
        GeneralInfo general,
        int audioStreamCount,
        int imageStreamCount)
    {
        return new AudioMediaInfoResult
        {
            General = general,
            AudioStreamCount = audioStreamCount,
            AudioCodec = GetInfo(reader, MediaInfoStreamKind.Audio, 0, "Format/String"),
            AudioBitRate = ParseInt64(GetInfo(reader, MediaInfoStreamKind.Audio, 0, "BitRate")),
            AudioBitRateMode = GetInfo(reader, MediaInfoStreamKind.Audio, 0, "BitRate_Mode"),
            Channels = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Audio, 0, "Channel(s)")),
            SamplingRate = ParseInt32(
                GetInfo(reader, MediaInfoStreamKind.Audio, 0, "SamplingRate")),
            ArtworkCount = imageStreamCount,
            Title = GetInfo(reader, MediaInfoStreamKind.General, 0, "Track"),
            Album = GetInfo(reader, MediaInfoStreamKind.General, 0, "Album"),
            Artist = GetInfo(reader, MediaInfoStreamKind.General, 0, "Performer"),
            TrackNumber = ParseInt32(
                GetInfo(reader, MediaInfoStreamKind.General, 0, "Track/Position")),
            TrackCount = ParseInt32(
                GetInfo(reader, MediaInfoStreamKind.General, 0, "Track/Position_Total")),
            Genre = GetInfo(reader, MediaInfoStreamKind.General, 0, "Genre"),
            RecordedDate = GetInfo(reader, MediaInfoStreamKind.General, 0, "Recorded_Date"),
        };
    }

    private static ImageMediaInfoResult CreateImageResult(
        MediaInfoReader reader,
        GeneralInfo general,
        int imageStreamCount)
    {
        int? width = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Image, 0, "Width"));
        int? height = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Image, 0, "Height"));

        return new ImageMediaInfoResult
        {
            General = general,
            ImageStreamCount = imageStreamCount,
            ImageFormat = GetInfo(reader, MediaInfoStreamKind.Image, 0, "Format/String"),
            Resolution = CreateResolution(width, height),
            BitDepth = ParseInt32(GetInfo(reader, MediaInfoStreamKind.Image, 0, "BitDepth")),
            ColorSpace = GetInfo(reader, MediaInfoStreamKind.Image, 0, "ColorSpace"),
            Title = GetInfo(reader, MediaInfoStreamKind.Image, 0, "Title"),
        };
    }

    private static MediaResolution? CreateResolution(int? width, int? height) =>
        width is > 0 && height is > 0
            ? new MediaResolution(width.Value, height.Value)
            : null;

    private static string? GetInfo(
        MediaInfoReader reader,
        MediaInfoStreamKind streamKind,
        int streamIndex,
        string parameter)
    {
        string value = reader.GetInfo(streamKind, streamIndex, parameter);
        return string.IsNullOrEmpty(value) ? null : value;
    }

    private static TimeSpan? ParseDuration(string? value)
    {
        double? milliseconds = ParseDouble(value);

        return milliseconds is >= 0
            ? TimeSpan.FromMilliseconds(milliseconds.Value)
            : null;
    }

    private static int? ParseInt32(string? value)
    {
        return int.TryParse(
            value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int result)
                ? result
                : null;
    }

    private static long? ParseInt64(string? value)
    {
        return long.TryParse(
            value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out long result)
                ? result
                : null;
    }

    private static double? ParseDouble(string? value)
    {
        return double.TryParse(
            value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double result)
                ? result
                : null;
    }
}
