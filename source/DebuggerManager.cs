using System.Diagnostics;

public static class DebuggerManager {

    public static bool DEBUG_MESSAGES_ENABLED = true;

    public static void Print(string message) {
        if(!DEBUG_MESSAGES_ENABLED) return;
        Debug.WriteLine(message);
    }

}