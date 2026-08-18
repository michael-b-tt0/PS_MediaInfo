using System.Runtime.InteropServices;

namespace GetMediaInfo.Interop;

/// <summary>
/// Owns a native MediaInfo instance returned by MediaInfo_New.
/// </summary>
internal sealed class MediaInfoHandle : SafeHandle
{
    // LibraryImport requires a public parameterless constructor when a
    // SafeHandle-derived type is used as a native return value.
    public MediaInfoHandle()
        : base(IntPtr.Zero, ownsHandle: true)
    {
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        // MediaInfo_Delete destroys the instance and releases any file that
        // remains open. MediaInfo_Close is only needed when reusing an instance.
        MediaInfoNative.Delete(handle);
        return true;
    }
}
