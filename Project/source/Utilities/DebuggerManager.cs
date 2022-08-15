using System;
using System.Diagnostics;
using ImGuiNET;
using ProjectMono.Core;
using System.Numerics;

using System.Linq;
using System.Collections.Generic;

using ProjectMono.Graphics;
using ProjectMono.Gameplay;
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

    static IEnumerable<Type> GUI_DRAWABLE_TYPES;

    public static void Initialize() {
        GUI_DRAWABLE_TYPES = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IGUIDrawable).IsAssignableFrom(p));
    }

    #region IMGUI

    static bool CtrlShift =>
        ImGui.GetIO().KeyCtrl && 
        ImGui.GetIO().KeyShift;

    static bool Shortcut(ImGuiKey key) => CtrlShift && ImGui.IsKeyDown(key);

    static bool OPEN_GENERAL_SETTINGS = false;
    static bool OPEN_ENTITY_BROWSER = false;

    public static void GUI_Debugger(ProjectMonoApp game) {

        //Submenu
        if(ImGui.BeginMainMenuBar()) {
            if (ImGui.BeginMenu("Debug"))
            {
                if (ImGui.MenuItem("Settings", "Ctrl-Shift-A"))
                    OPEN_GENERAL_SETTINGS=true;
                if (ImGui.MenuItem("Entity Browser", "Ctrl-Shift-C"))
                    OPEN_ENTITY_BROWSER=true;
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        if(Shortcut(ImGuiKey.A))
            OPEN_GENERAL_SETTINGS=true;
        if(Shortcut(ImGuiKey.C))
            OPEN_ENTITY_BROWSER=true;

        if(OPEN_GENERAL_SETTINGS)   GUI_Settings(game, ref OPEN_GENERAL_SETTINGS);
        if(OPEN_ENTITY_BROWSER)     GUI_EntityBrowser(game, ref OPEN_ENTITY_BROWSER);
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

    static int SELECTED_ENTITY = 0;
    static void GUI_EntityBrowser(ProjectMonoApp game, ref bool open) {
        ImGui.SetNextWindowSize(new Vector2(500, 440), ImGuiCond.FirstUseEver);
        /*
        if(ImGui.Begin("Entity Browser", ref open))
        {
            // Left
            {
                //Navigate selection with up and down
                if(ImGui.IsKeyPressed(ImGuiKey.DownArrow))
                    SELECTED_ENTITY++;
                else if(ImGui.IsKeyPressed(ImGuiKey.UpArrow))
                    SELECTED_ENTITY--;
                SELECTED_ENTITY = Math.Clamp(SELECTED_ENTITY, 0, game.World.EntityCount-1);
                //-----

                ImGui.BeginChild("left pane", new Vector2(150, 0), true);
                for (int i = 0; i < game.World.EntityCount; i++)
                {
                    var name = game.World.GetEntityName(i);
                    
                    if (ImGui.Selectable((""+i).PadLeft(3, '0') + ": " + name, SELECTED_ENTITY == i))
                        SELECTED_ENTITY = i;
                }
                ImGui.EndChild();
            }
            ImGui.SameLine();
            // Right
            {   

                ImGui.BeginGroup();
                ImGui.BeginChild("item view", new Vector2(0, -ImGui.GetFrameHeightWithSpacing())); // Leave room for 1 line below us
                ImGui.Text(game.World.GetEntityName(SELECTED_ENTITY));
                ImGui.Separator();
                {
                    Entity e = game.World.GetEntity(SELECTED_ENTITY);
                    DrawComponent<C_Transform2>(e);
                    DrawComponent<C_Tags>(e);
                    DrawComponent<C_Camera>(e);
                    DrawComponent<C_Player>(e);
                    DrawComponent<C_Motion>(e);
                    DrawComponent<C_PlatformerData>(e);
                    DrawComponent<C_PlatformerInput>(e);
                    DrawComponent<C_Sprite>(e);
                    
                    ImGui.EndTabBar();
                }
                ImGui.EndChild();
                ImGui.EndGroup();
            }
            ImGui.End();
        }

        void DrawComponent<T>(Entity e) where T : class, IGUIDrawable {
            if (!e.Has<T>()) return;
            var component = e.Get<T>();
            if(ImGui.CollapsingHeader(component.Label))
                component.GUI_Draw();
            ImGui.Separator();
        }*/
    }

    #endregion

}