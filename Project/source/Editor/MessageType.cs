using System;

namespace ProjectMono.Debugging
{

    [Flags]
    public enum MessageType
    {
        SYSTEM = 1 << 0,
        GAMEPLAY_DEBUG = 1 << 1,
        INPUT_DEBUG = 1 << 2,
        AUDIO_DEBUG = 1 << 3,
    }
}