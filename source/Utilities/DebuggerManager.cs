using System;
using System.Diagnostics;
using ImGuiNET;
using ProjectMono.Core;
using ProjectMono.Gameplay;
using ProjectMono.Graphics;
using ProjectMono.Physics;

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

    #region IMGUI

    static bool CtrlShift =>
        ImGui.GetIO().KeyCtrl && 
        ImGui.GetIO().KeyShift;

    static bool Shortcut(ImGuiKey key) => CtrlShift && ImGui.IsKeyDown(key);

    public static void GUI_Debugger(ProjectMonoApp game) {
        var inputManager = game.InputManager;

        ImGui.ShowDemoWindow();

        //Submenu
        string subMenuOpenID="";
        if(ImGui.BeginMainMenuBar()) {
            if (ImGui.BeginMenu("Debug"))
            {
                if (ImGui.MenuItem("Settings", "Ctrl-Shift-A"))
                    subMenuOpenID="Settings";
                if (ImGui.MenuItem("Camera", "Ctrl-Shift-C"))
                    subMenuOpenID="Camera";
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        if(Shortcut(ImGuiKey.A))
            subMenuOpenID="Settings";
        if(Shortcut(ImGuiKey.C)) {
            subMenuOpenID="Camera";
        }

        if(game.World.TryGetFirstComponent<C_Sprite>(out var sprite)) {
            sprite.GUI_Display_Sprite();
        }

        if(subMenuOpenID!="")
            ImGui.OpenPopup(subMenuOpenID);

        if(ImGui.BeginPopup("Settings"))
            GUI_Settings(game);

        if(ImGui.BeginPopup("Camera"))
            GUI_Camera(game);
    }

    static int WINDOW_RESOLUTION_X, WINDOW_RESOLUTION_Y;

    static void GUI_Settings(ProjectMonoApp game) {
        ImGui.DragInt2("Resolution", ref WINDOW_RESOLUTION_X, WINDOW_RESOLUTION_Y, 320, 1920);
        
        if(ImGui.Button("Apply")) {
            
        }

        ImGui.EndPopup();
    }

    static void GUI_Camera(ProjectMonoApp game) {
        ImGui.TextColored(new System.Numerics.Vector4(1.0f, 0.2f, 0.2f, 1.0f), "CAMERA SETTINGS");

        int cameraID = game.World.GetFirstEntityWithComponent<C_Camera>();
        var camera = game.World.GetEntity(cameraID).Get<C_Camera>();
        var cameraTransform = game.World.GetEntity(cameraID).Get<C_Transform2>();

        var camPos = cameraTransform.Position.MonoVec2SysVec();
        if(ImGui.DragFloat2("Camera Position", ref camPos, 10))
            cameraTransform.Position = camPos.SysVec2MonoVec();

        var targetPos = game.World.GetEntity(camera.ID_CameraTarget).Get<C_Transform2>().Position.MonoVec2SysVec();
        if(ImGui.DragFloat2("Target Position", ref targetPos, 10)) {}
        
        
        if(ImGui.Button("Close")) ImGui.CloseCurrentPopup();
        ImGui.EndPopup();
    }



    #endregion

}