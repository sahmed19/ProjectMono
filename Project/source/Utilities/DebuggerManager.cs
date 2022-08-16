using System;
using System.Diagnostics;
using ImGuiNET;
using ProjectMono.Core;
using System.Numerics;
using ProjectMono.Graphics;
using ProjectMono.Gameplay;
using ProjectMono.Physics;

using Flecs;

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

    static bool Shortcut(ImGuiKey key) => CtrlShift && ImGui.IsKeyPressed(key);

    static bool OPEN_GENERAL_SETTINGS = false;
    static bool OPEN_ENTITY_BROWSER = false;
    static bool OPEN_DEMO_WINDOW = false;

    public static void GUI_Debugger(ProjectMonoApp game) {

        

        //Submenu
        if(ImGui.BeginMainMenuBar()) {
            if (ImGui.BeginMenu("Debug"))
            {
                if (ImGui.MenuItem("Settings", "Ctrl-Shift-A"))
                    OPEN_GENERAL_SETTINGS=true;
                if (ImGui.MenuItem("Entity Browser", "Ctrl-Shift-C"))
                    OPEN_ENTITY_BROWSER=true;
                if(ImGui.MenuItem("Demo Window", "Ctrl-Shift-W"))
                    OPEN_DEMO_WINDOW=true;
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        if(Shortcut(ImGuiKey.A))
            OPEN_GENERAL_SETTINGS=!OPEN_GENERAL_SETTINGS;
        if(Shortcut(ImGuiKey.C))
            OPEN_ENTITY_BROWSER=!OPEN_ENTITY_BROWSER;
        if(Shortcut(ImGuiKey.Z))
            OPEN_DEMO_WINDOW=!OPEN_DEMO_WINDOW;

        if(OPEN_GENERAL_SETTINGS)   GUI_Settings(game, ref OPEN_GENERAL_SETTINGS);
        if(OPEN_ENTITY_BROWSER)     GUI_EntityBrowser(game, ref OPEN_ENTITY_BROWSER);
        if(OPEN_DEMO_WINDOW)        ImGui.ShowDemoWindow();
    }

    static void GUI_Settings(ProjectMonoApp game, ref bool open) {
        ImGui.SetNextWindowSize(new Vector2(500, 440), ImGuiCond.FirstUseEver);
        if(ImGui.Begin("General Settings", ref open))
        {
            if(ImGui.BeginTabBar("General Settings"))
            {
                if(ImGui.BeginTabItem("Graphics"))
                {
                    ImGui.Text("Set the display and resolution settings.");

                    //SELECT RESOLUTION
                    if (ImGui.Button("Resolution: " + Settings.CURRENT_RESOLUTION.ToString()))
                        ImGui.OpenPopup("select_resolution");
                        
                    if(ImGui.BeginPopup("select_resolution"))
                    {
                        ImGui.Text("Resolution");
                        ImGui.Separator();
                        for (int i = 0; i < Settings.RESOLUTIONS.Length; i++)
                            if (ImGui.Selectable(Settings.RESOLUTIONS[i].ToString())) {
                                Settings.GRAPHICS_SETTINGS.ResolutionID = i;
                            }
                        ImGui.EndPopup();
                    }

                    //SELECT SCREEN MODE
                    if (ImGui.Button("Screen Mode: " + Settings.SCREEN_MODE_LABEL))
                        ImGui.OpenPopup("select_screen_mode");
                    
                    if(ImGui.BeginPopup("select_screen_mode"))
                    {
                        ImGui.Text("Screen Mode");
                        ImGui.Separator();
                        for (int i = 0; i < 3; i++)
                            if (ImGui.Selectable(Settings.SCREEN_MODE_LABELS[i])) {
                                Settings.GRAPHICS_SETTINGS.ScreenMode = (Settings.ScreenMode) i;
                            }
                        ImGui.EndPopup();
                    }

                    ImGui.Separator();

                    //APPLY OR RESET
                    ImGui.BeginDisabled(!Settings.GRAPHICS_SETTINGS_HAVE_BEEN_CHANGED);
                    if(ImGui.Button("Apply"))
                        Settings.ApplyGraphics();
                    ImGui.EndDisabled();

                    ImGui.SameLine();

                    if(ImGui.Button("Reset to Defaults")) {
                        Settings.ResetGraphicsSettings();
                    }
                    ImGui.EndTabItem();
                }
                if(ImGui.BeginTabItem("Audio"))
                {
                    ImGui.Text("Set the volume and mix settings.");
                    
                    ImGui.SliderInt("Master Volume", ref Settings.AUDIO_SETTINGS.MasterVolume, 0, 100);
                    ImGui.SliderInt("Effect Volume", ref Settings.AUDIO_SETTINGS.EffectVolume, 0, 100);
                    ImGui.SliderInt("Musics Volume", ref Settings.AUDIO_SETTINGS.MusicsVolume, 0, 100);
                    ImGui.SliderInt("System Volume", ref Settings.AUDIO_SETTINGS.SystemVolume, 0, 100);

                    if(ImGui.Button("Reset to Defaults")) {
                        Settings.ResetAudioSettings();
                    }
                    ImGui.EndTabItem();
                }
                if(ImGui.BeginTabItem("Controls"))
                {
                    ImGui.Text("Reconfigure the controls of the game.");
                    ImGui.EndTabItem();
                }
                if(ImGui.BeginTabItem("Gameplay/Accessibility"))
                {
                    ImGui.Text("Adjust gameplay values to personal preference.");
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }

            ImGui.End();
        }
    }

    const int MAX_DISPLAYABLE_ENTITIES = 2048;
    static Entity[] ALL_ENTITIES = new Entity[MAX_DISPLAYABLE_ENTITIES];
    static int SELECTED_ENTITY = 0;
    static int CURRENT_NUM_ENTITIES = 0;
    static void GUI_EntityBrowser(ProjectMonoApp game, ref bool open) {
        ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);
        
        if(ImGui.Begin("Entity Browser", ref open))
        {
            if(ImGui.Button("Refresh", Vector2.One * 50) || CURRENT_NUM_ENTITIES==0)
            {
                
                var it = game.World.EntityIterator<C_Position>();
                CURRENT_NUM_ENTITIES=0;
                while(it.HasNext()) {
                    for(int i = 0; i < it.Count; i++)
                    {
                        ALL_ENTITIES[CURRENT_NUM_ENTITIES] = it.Entity(i);
                        CURRENT_NUM_ENTITIES++;
                        if(CURRENT_NUM_ENTITIES>=MAX_DISPLAYABLE_ENTITIES) break;
                    }
                    if(CURRENT_NUM_ENTITIES>=MAX_DISPLAYABLE_ENTITIES) break;
                }
            }

            // Left
            {
                //Navigate selection with up and down
                if(ImGui.IsKeyPressed(ImGuiKey.DownArrow))
                    SELECTED_ENTITY++;
                else if(ImGui.IsKeyPressed(ImGuiKey.UpArrow))
                    SELECTED_ENTITY--;
                SELECTED_ENTITY = Math.Clamp(SELECTED_ENTITY, 0, CURRENT_NUM_ENTITIES-1);
                //-----

                ImGui.BeginChild("left pane", new Vector2(200, 0), true);
                for (int i = 0; i < CURRENT_NUM_ENTITIES; i++)
                {
                    var name = ALL_ENTITIES[i].Name();
                    
                    if (ImGui.Selectable((""+i).PadLeft(3, '0') + ": " + name, SELECTED_ENTITY == i))
                        SELECTED_ENTITY = i;
                }
                ImGui.EndChild();
            }
            ImGui.SameLine();
            // Right
            {

                Entity e = ALL_ENTITIES[SELECTED_ENTITY];

                ImGui.BeginGroup();
                ImGui.BeginChild("item view", new Vector2(0, -ImGui.GetFrameHeightWithSpacing())); // Leave room for 1 line below us
                ImGui.Text(e.Name());
                ImGui.Separator();
                {
                    if(ImGui.CollapsingHeader("Transform"))
                    {
                        if(e.HasComponent<C_Position>()) DragFloat2("Position", ref e.GetComponent<C_Position>().Position, 0.1f);
                        if(e.HasComponent<C_Scale>()) DragFloat2("Scale", ref e.GetComponent<C_Scale>().Scale, 0.1f);
                        if(e.HasComponent<C_Rotation>()) ImGui.SliderAngle("Rotation", ref e.GetComponent<C_Rotation>().Angle, 0.0f, 360.0f);
                    }

                    if(ImGui.CollapsingHeader("Physics"))
                    {
                        if(e.HasComponent<C_Velocity>()) DragFloat2("Position", ref e.GetComponent<C_Position>().Position, 0.1f);
                        if(e.HasComponent<C_TerminalVelocity>()) DragFloat2("Scale", ref e.GetComponent<C_Scale>().Scale, 0.1f);
                        if(e.HasComponent<C_Rotation>()) ImGui.SliderAngle("Rotation", ref e.GetComponent<C_Rotation>().Angle, 0.0f, 360.0f);
                    }
                    
                    if(ImGui.CollapsingHeader("Graphics"))
                    {
                        if(e.HasComponent<C_Camera>()) ImGui.DragFloat("Camera Zoom", ref e.GetComponent<C_Camera>().Zoom, 0.1f, 0.01f, 3.0f);
                    }

                    /*
                    if(ImGui.CollapsingHeader("Physics"))
                    {
                        if(e.HasComponent<C_Velocity>()) DragFloat2("Velocity", ref e.GetComponent<C_Velocity>().Velocity, 1.0f);
                        if(e.HasComponent<C_TerminalVelocity>()) ImGui.DragFloat("Terminal Velocity", ref e.GetComponent<C_TerminalVelocity>().TerminalVelocity, 1.0f);
                        if(e.HasComponent<C_Gravity>()) DragFloat2("Gravity", ref e.GetComponent<C_Gravity>().Gravity, 1.0f);
                    }*/

                    /*
                    if(ImGui.CollapsingHeader("TRANSFORM"))
                    {
                        DrawComponent<C_Position>(e);
                        DrawComponent<C_Rotation>(e);
                        DrawComponent<C_Scale>(e);
                        ImGui.Separator();
                    }
                    
                    if(ImGui.CollapsingHeader("PHYSICS"))
                    {
                        DrawComponent<C_Motion>(e);
                        ImGui.Separator();
                    }

                    if(ImGui.CollapsingHeader("GRAPHICS"))
                    {
                        DrawComponent<C_Camera>(e);
                        DrawComponent<C_Sprite>(e);
                        ImGui.Separator();
                    }

                    if(ImGui.CollapsingHeader("GAMEPLAY"))
                    {
                        DrawComponent<C_Health>(e);
                        DrawComponent<C_PlatformerData>(e);
                        DrawComponent<C_PlatformerInput>(e);
                    }*/

                    ImGui.EndTabBar();
                }
                ImGui.EndChild();
                ImGui.EndGroup();
            }
            
            ImGui.End();
        }    
    }

    static void DragFloat2(string label, ref Microsoft.Xna.Framework.Vector2 vector, float speed) {
        System.Numerics.Vector2 vec = vector.MonoVec2SysVec();
        if(ImGui.DragFloat2(label, ref vec, speed)) {
            vector = vec.SysVec2MonoVec();
        }

    }


    #endregion

}