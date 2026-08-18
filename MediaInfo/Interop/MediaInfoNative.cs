using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GetMediaInfo.Interop;

/// <summary>
/// The subset of the MediaInfo C ABI used by this module.
/// </summary>
internal static partial class MediaInfoNative
{
    // This is deliberately not "MediaInfo": the managed module is also named
    // MediaInfo.dll. NativeDependencyResolver maps this logical name to the
    // RID-specific native binary.
    internal const string LibraryName = "GetMediaInfo.MediaInfo.Native";

    static MediaInfoNative()
    {
        NativeDependencyResolver.Register(typeof(MediaInfoNative).Assembly);
    }

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_New")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial MediaInfoHandle New();

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial void Delete(nint handle);

    [LibraryImport(
        LibraryName,
        EntryPoint = "MediaInfo_Open",
        StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial nuint Open(MediaInfoHandle handle, string fileName);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Close")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial void Close(MediaInfoHandle handle);

    [LibraryImport(
        LibraryName,
        EntryPoint = "MediaInfo_Get",
        StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial nint Get(
        MediaInfoHandle handle,
        MediaInfoStreamKind streamKind,
        nuint streamNumber,
        string parameter,
        MediaInfoValueKind kindOfInfo,
        MediaInfoValueKind kindOfSearch);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Inform")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial nint Inform(MediaInfoHandle handle, nuint reserved);

    [LibraryImport(
        LibraryName,
        EntryPoint = "MediaInfo_Option",
        StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial nint Option(
        MediaInfoHandle handle,
        string option,
        string value);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Count_Get")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    internal static partial nuint CountGet(
        MediaInfoHandle handle,
        MediaInfoStreamKind streamKind,
        nuint streamNumber);
}
