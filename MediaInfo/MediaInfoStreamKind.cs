namespace GetMediaInfo;

/// <summary>
/// Identifies a MediaInfo stream category.
/// Values must remain aligned with MediaInfo_stream_C in MediaInfoDLL.h.
/// </summary>
public enum MediaInfoStreamKind
{
    General = 0,
    Video = 1,
    Audio = 2,
    Text = 3,
    Other = 4,
    Image = 5,
    Menu = 6,
}
