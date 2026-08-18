namespace GetMediaInfo.Interop;

/// <summary>
/// Values from MediaInfo_info_C in MediaInfoDLL.h.
/// </summary>
internal enum MediaInfoValueKind
{
    Name = 0,
    Text = 1,
    Measure = 2,
    Options = 3,
    NameText = 4,
    MeasureText = 5,
    Info = 6,
    HowTo = 7,
}
