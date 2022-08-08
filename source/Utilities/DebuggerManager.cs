using System;
using System.Diagnostics;

[Flags]
public enum MessageType {
    SYSTEM = 1 << 0,
    GAMEPLAY_DEBUG = 1 << 1,
    INPUT_DEBUG = 1 << 2,
    AUDIO_DEBUG = 1 << 3,
}

public static class DebuggerManager {

    public static bool DEBUG_MESSAGES_ENABLED = true;
    static MessageType m_EnabledMessageTypes => 
    MessageType.SYSTEM
    | MessageType.GAMEPLAY_DEBUG
    //| MessageType.INPUT_DEBUG
    //| MessageType.AUDIO_DEBUG
    ;

    public static void Print(string message, MessageType messageType = MessageType.SYSTEM) {
        if(!DEBUG_MESSAGES_ENABLED) return;
        if(!m_EnabledMessageTypes.HasFlag(messageType)) return;
        Debug.WriteLine(message);
    }

}