using System;
using System.Runtime.InteropServices;

/// <summary>
/// This file contains COM mappings for Windows Core Audio API.
/// MSDN documentation: https://docs.microsoft.com/en-us/windows/win32/coreaudio/core-audio-apis-in-windows-vista
/// Check out the netcoreaudio project for pre-fabbed mappings: https://archive.codeplex.com/?p=netcoreaudio
/// </summary>
namespace IsTheMicInUse
{
    internal enum EDataFlow
    {
        eRender,
        eCapture,
        eAll,
        EDataFlow_enum_count
    }

    internal enum AudioSessionState
    {
        AudioSessionStateInactive = 0,
        AudioSessionStateActive = 1,
        AudioSessionStateExpired = 2
    }

    internal class DEVICE_STATE_XXX
    {
        public const uint DEVICE_STATE_ACTIVE = 0x00000001;
        public const uint DEVICE_STATE_DISABLED = 0x00000002;
        public const uint DEVICE_STATE_NOTPRESENT = 0x00000004;
        public const uint DEVICE_STATE_UNPLUGGED = 0x00000008;
        public const uint DEVICE_STATEMASK_ALL = 0x0000000F;
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    {
    }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        [PreserveSig]
        int EnumAudioEndpoints(EDataFlow dataFlow, UInt32 stateMask, out IMMDeviceCollection devices);
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig]
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
    }

    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceCollection
    {
        [PreserveSig]
        int GetCount(out UInt32 count);

        [PreserveSig]
        int Item(UInt32 index, out IMMDevice device);
    }

    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionManager2
    {
        int NotImpl1();
        int NotImpl2();

        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
    }

    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        [PreserveSig]
        int GetCount(out int SessionCount);

        [PreserveSig]
        int GetSession(int SessionCount, out IAudioSessionControl Session);
    }

    [Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionControl
    {
        [PreserveSig]
        int GetState(out AudioSessionState state);
    }
}
