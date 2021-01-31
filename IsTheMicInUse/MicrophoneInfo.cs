using System;
using System.Runtime.InteropServices;

namespace IsTheMicInUse
{
    /// <summary>
    /// This class uses Windows Core Audio API to get information about the system's microphones.
    /// </summary>
    class MicrophoneInfo
    {
        /// <summary>
        /// Returns true if an active microphone session is found.
        /// </summary>
        public bool IsAnyMicInUse()
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDeviceCollection microphoneCollection = null;

            try
            {
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                deviceEnumerator.EnumAudioEndpoints(EDataFlow.eCapture, DEVICE_STATE_XXX.DEVICE_STATE_ACTIVE, out microphoneCollection);

                microphoneCollection.GetCount(out uint microphoneCount);

                for (uint i = 0; i < microphoneCount; i++)
                {
                    IMMDevice microphone = null;

                    try
                    {
                        microphoneCollection.Item(i, out microphone);

                        if(microphoneHasActiveSession(microphone))
                        {
                            return true;
                        }
                    }
                    finally
                    {
                        if (microphone != null)
                        {
                            Marshal.ReleaseComObject(microphone);
                        }
                    }
                }
            }
            finally
            {
                if (microphoneCollection != null)
                {
                    Marshal.ReleaseComObject(microphoneCollection);
                }
                if (deviceEnumerator != null)
                {
                    Marshal.ReleaseComObject(deviceEnumerator);
                }
            }

            return false;
        }

        private bool microphoneHasActiveSession(IMMDevice microphone)
        {
            IAudioSessionManager2 sessionManager2 = null;
            IAudioSessionEnumerator sessionEnumerator = null;

            try
            {
                sessionManager2 = activateMicSessionManager(microphone);
                sessionManager2.GetSessionEnumerator(out sessionEnumerator);

                sessionEnumerator.GetCount(out int sessionCount);

                for (int i = 0; i < sessionCount; i++)
                {
                    if (sessionIsActive(sessionEnumerator, i))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                if (sessionEnumerator != null)
                {
                    Marshal.ReleaseComObject(sessionEnumerator);
                }
                if (sessionManager2 != null)
                {
                    Marshal.ReleaseComObject(sessionManager2);
                }
            }

            return false;
        }

        private IAudioSessionManager2 activateMicSessionManager(IMMDevice microphone)
        {
            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            microphone.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out object intermediary);
            IAudioSessionManager2 sessionManager2 = (IAudioSessionManager2)intermediary;

            return sessionManager2;
        }
        private bool sessionIsActive(IAudioSessionEnumerator sessionEnumerator, int sessionIndex)
        {
            IAudioSessionControl sessionControl = null;

            try
            {
                sessionEnumerator.GetSession(sessionIndex, out sessionControl);
                AudioSessionState sessionState = AudioSessionState.AudioSessionStateInactive;
                sessionControl.GetState(out sessionState);

                return sessionState == AudioSessionState.AudioSessionStateActive;
            }
            finally
            {
                if (sessionControl != null)
                {
                    Marshal.ReleaseComObject(sessionControl);
                }
            }
        }
    }
}
