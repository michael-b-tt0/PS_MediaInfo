using System.Reflection;
using System.Runtime.InteropServices;

namespace GetMediaInfo.Interop;

internal static class NativeDependencyResolver
{
    internal static void Register(Assembly assembly)
    {
        NativeLibrary.SetDllImportResolver(
            assembly,
            Resolve);
    }

    private static nint Resolve(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath)
    {
        if (!string.Equals(
                libraryName,
                MediaInfoNative.LibraryName,
                StringComparison.Ordinal))
        {
            return IntPtr.Zero;
        }

        if (!OperatingSystem.IsWindows() ||
            RuntimeInformation.ProcessArchitecture != Architecture.X64)
        {
            throw new PlatformNotSupportedException(
                "This build of Get-MediaInfo includes MediaInfo only for Windows x64.");
        }

        string? assemblyDirectory = Path.GetDirectoryName(assembly.Location);

        if (string.IsNullOrEmpty(assemblyDirectory))
        {
            throw new DllNotFoundException(
                "The Get-MediaInfo module directory could not be determined.");
        }

        string nativeLibraryPath = Path.Combine(
            assemblyDirectory,
            "runtimes",
            "win-x64",
            "native",
            "MediaInfo.dll");

        if (!File.Exists(nativeLibraryPath))
        {
            throw new DllNotFoundException(
                $"The bundled MediaInfo library was not found at '{nativeLibraryPath}'.");
        }

        return NativeLibrary.Load(nativeLibraryPath);
    }
}
