namespace GetMediaInfo;

/// <summary>
/// Represents the pixel dimensions of a video or image stream.
/// </summary>
public readonly record struct MediaResolution(int Width, int Height)
{
    /// <summary>
    /// Returns the conventional width-by-height representation.
    /// </summary>
    public override string ToString() => $"{Width}x{Height}";
}
