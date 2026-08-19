using System.Globalization;

namespace GetMediaInfo;

internal static class MediaInfoResultFactory
{
    internal static MediaInfoResult Create(MediaInfoReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        int videoStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Video);
        int audioStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Audio);
        int textStreamCount = reader.GetStreamCount(MediaInfoStreamKind.Text);

        int? width = videoStreamCount > 0
            ? ParseInt32(reader.GetInfo(MediaInfoStreamKind.Video, 0, "Width"))
            : null;
        int? height = videoStreamCount > 0
            ? ParseInt32(reader.GetInfo(MediaInfoStreamKind.Video, 0, "Height"))
            : null;

        string? videoBitRateText = videoStreamCount > 0
            ? NullIfEmpty(reader.GetInfo(MediaInfoStreamKind.Video, 0, "BitRate"))
            : null;

        if (videoStreamCount > 0 && videoBitRateText is null)
        {
            videoBitRateText = NullIfEmpty(
                reader.GetInfo(MediaInfoStreamKind.General, 0, "OverallBitRate"));
        }

        FileInfo file = new(reader.FilePath);

        return new MediaInfoResult
        {
            FullName = file.FullName,
            FileSize = file.Length,
            Duration = ParseDuration(
                reader.GetInfo(MediaInfoStreamKind.General, 0, "Duration")),
            ContainerFormat = NullIfEmpty(
                reader.GetInfo(MediaInfoStreamKind.General, 0, "Format/String")),
            VideoStreamCount = videoStreamCount,
            VideoCodec = videoStreamCount > 0
                ? NullIfEmpty(
                    reader.GetInfo(MediaInfoStreamKind.Video, 0, "Format/String"))
                : null,
            Resolution = width is > 0 && height is > 0
                ? new MediaResolution(width.Value, height.Value)
                : null,
            FrameRateMode = videoStreamCount > 0
                ? NullIfEmpty(
                    reader.GetInfo(MediaInfoStreamKind.Video, 0, "FrameRate_Mode"))
                : null,
            FrameRate = videoStreamCount > 0
                ? ParseDouble(
                    reader.GetInfo(MediaInfoStreamKind.Video, 0, "FrameRate"))
                : null,
            VideoBitRate = ParseInt64(videoBitRateText),
            DisplayAspectRatio = videoStreamCount > 0
                ? ParseDouble(
                    reader.GetInfo(
                        MediaInfoStreamKind.Video,
                        0,
                        "DisplayAspectRatio"))
                : null,
            FormatProfile = GetVideoInfo(reader, videoStreamCount, "Format_Profile"),
            ScanType = GetVideoInfo(reader, videoStreamCount, "ScanType"),
            ColorSpace = GetVideoInfo(reader, videoStreamCount, "ColorSpace"),
            ColorRange = GetVideoInfo(reader, videoStreamCount, "colour_range"),
            ColorPrimaries = GetVideoInfo(
                reader,
                videoStreamCount,
                "colour_primaries"),
            TransferCharacteristics = GetVideoInfo(
                reader,
                videoStreamCount,
                "transfer_characteristics"),
            MatrixCoefficients = GetVideoInfo(
                reader,
                videoStreamCount,
                "matrix_coefficients"),
            AudioStreamCount = audioStreamCount,
            AudioCodec = audioStreamCount > 0
                ? NullIfEmpty(
                    reader.GetInfo(MediaInfoStreamKind.Audio, 0, "Format"))
                : null,
            AudioBitRate = audioStreamCount > 0
                ? ParseInt64(
                    reader.GetInfo(MediaInfoStreamKind.Audio, 0, "BitRate"))
                : null,
            AudioBitRateMode = audioStreamCount > 0
                ? NullIfEmpty(
                    reader.GetInfo(MediaInfoStreamKind.Audio, 0, "BitRate_Mode"))
                : null,
            Performer = GetGeneralInfo(reader, "Performer"),
            Track = GetGeneralInfo(reader, "Track"),
            Album = GetGeneralInfo(reader, "Album"),
            RecordedDate = GetGeneralInfo(reader, "Recorded_Date"),
            Genre = GetGeneralInfo(reader, "Genre"),
            TextStreamCount = textStreamCount,
            TextFormats = GetGeneralInfo(reader, "Text_Format_List"),
        };
    }

    private static string? GetGeneralInfo(MediaInfoReader reader, string parameter)
    {
        return NullIfEmpty(
            reader.GetInfo(MediaInfoStreamKind.General, 0, parameter));
    }

    private static string? GetVideoInfo(
        MediaInfoReader reader,
        int videoStreamCount,
        string parameter)
    {
        return videoStreamCount > 0
            ? NullIfEmpty(
                reader.GetInfo(MediaInfoStreamKind.Video, 0, parameter))
            : null;
    }

    private static TimeSpan? ParseDuration(string value)
    {
        double? milliseconds = ParseDouble(value);

        return milliseconds is >= 0
            ? TimeSpan.FromMilliseconds(milliseconds.Value)
            : null;
    }

    private static int? ParseInt32(string value)
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

    private static double? ParseDouble(string value)
    {
        return double.TryParse(
            value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double result)
                ? result
                : null;
    }

    private static string? NullIfEmpty(string value) =>
        string.IsNullOrEmpty(value) ? null : value;
}
