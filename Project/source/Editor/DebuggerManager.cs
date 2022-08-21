using System;
using System.Diagnostics;
using ImGuiNET;
using ProjectMono.Core;
using System.Numerics;
using ProjectMono.Physics;
using ImGuiNET.XNA;

using Flecs;
using ProjectMono.Graphics;

namespace ProjectMono.Debugging {

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
        static bool OPEN_ENTITY_BROWSER = true;
        static bool OPEN_ENTITY_INSPECTOR = true;
        static bool OPEN_DEMO_WINDOW = false;

        public static void Initialize(out ImGuiRenderer renderer, Microsoft.Xna.Framework.Game game) {
            renderer = new ImGuiRenderer(game);   
            renderer.RebuildFontAtlas();
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        }

        public static void GUI_DebuggerUpdate(ProjectMonoApp game, float deltaTime)
        {
            
        }

        public static void GUI_DebuggerDraw(ProjectMonoApp game, float deltaTime)
        {
            //Focus entity
            if(FOCUS_SELECTED && OPEN_ENTITY_BROWSER) CameraFocusEntity(deltaTime);

            //Dockspace
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.NoDockingInCentralNode);
            
            //Submenu
            if(ImGui.BeginMainMenuBar()) {
                if (ImGui.BeginMenu("Debug"))
                {
                    if (ImGui.MenuItem("Settings", "Ctrl-Shift-S"))
                        OPEN_GENERAL_SETTINGS=true;
                    if (ImGui.MenuItem("Entity Browser", "Ctrl-Shift-B"))
                        OPEN_ENTITY_BROWSER=true;
                    if (ImGui.MenuItem("Entity Inspector", "Ctrl-Shift-I"))
                        OPEN_ENTITY_INSPECTOR=true;
                    if(ImGui.MenuItem("Demo Window", "Ctrl-Shift-D"))
                        OPEN_DEMO_WINDOW=true;
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            if(Shortcut(ImGuiKey.S))
                OPEN_GENERAL_SETTINGS=!OPEN_GENERAL_SETTINGS;
            if(Shortcut(ImGuiKey.B))
                OPEN_ENTITY_BROWSER=!OPEN_ENTITY_BROWSER;
            if(Shortcut(ImGuiKey.I))
                OPEN_ENTITY_INSPECTOR=!OPEN_ENTITY_INSPECTOR;
            if(Shortcut(ImGuiKey.D))
                OPEN_DEMO_WINDOW=!OPEN_DEMO_WINDOW;

            if(OPEN_GENERAL_SETTINGS)   GUI_Settings(game, ref OPEN_GENERAL_SETTINGS);
            if(OPEN_ENTITY_BROWSER)     GUI_EntityBrowser(game, ref OPEN_ENTITY_BROWSER);
            if(OPEN_ENTITY_INSPECTOR)     GUI_EntityInspector(game, ref OPEN_ENTITY_INSPECTOR);
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
        static int SELECTED_ENTITY_INDEX = 0;
        static int CURRENT_NUM_ENTITIES = 0;
        public static Entity GetSelectedEntity() => ALL_ENTITIES[SELECTED_ENTITY_INDEX];
        static void RefreshEntityList(ProjectMonoApp game) {  
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
        const float FOCUS_ADJUSTMENT_SPEED = 1.0f;
        public static void CameraFocusEntity(float deltaTime)
        {
            if(SELECTED_ENTITY_INDEX<0) return;
            var targetPos = GetSelectedEntity().GetComponent<C_Position>();
            //S_Camera.SLerp_ActualCameraPosition(targetPos.Position, deltaTime * FOCUS_ADJUSTMENT_SPEED, targetRot.Angle, 3.0f);
            S_Camera.SetActualCameraPosition(targetPos.Position, 0.0f, 3.0f);
        }
        static bool FOCUS_SELECTED;
        static void GUI_EntityBrowser(ProjectMonoApp game, ref bool open) {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);
            
            if(ImGui.Begin("Entity Browser", ref open))
            {
                
                //Refresh button
                if(ImGui.Button("Refresh", new Vector2(70, 30)) || CURRENT_NUM_ENTITIES==0)
                    RefreshEntityList(game);
                ImGui.SameLine();
                if(ImGui.Checkbox("Focus", ref FOCUS_SELECTED)) { }

                Entity clickedEntity = GetSelectedEntity();

                //Navigate selection with up and down
                if(ImGui.IsKeyPressed(ImGuiKey.DownArrow)) SELECTED_ENTITY_INDEX++;
                else if(ImGui.IsKeyPressed(ImGuiKey.UpArrow)) SELECTED_ENTITY_INDEX--;
                SELECTED_ENTITY_INDEX = Math.Clamp(SELECTED_ENTITY_INDEX, 0, CURRENT_NUM_ENTITIES-1);
                //-----

                //ImGui.BeginChild("left pane", new Vector2(200, 0), true);
                for (int i = 0; i < CURRENT_NUM_ENTITIES; i++)
                {    
                    var name = ALL_ENTITIES[i].Name();
                    if (ImGui.Selectable((""+i).PadLeft(3, '0') + ": " + name, SELECTED_ENTITY_INDEX == i))
                        SELECTED_ENTITY_INDEX = i;
                }
                
                ImGui.End();
            }    
        }
        static void GUI_ContentView(ProjectMonoApp game, ref bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);

            if(ImGui.Begin("Content View", ref open))
            {
                
            }
        }

        static void GUI_EntityInspector(ProjectMonoApp game, ref bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);
            
            if(ImGui.Begin("Entity Inspector", ref open))
            {
                Entity e = GetSelectedEntity();

                ImGui.Text(e.Name());
                ImGui.Separator();
                
                if(ImGui.CollapsingHeader("Transform"))
                {
                    if(e.Has<C_Position>()) DragFloat2("Position", ref e.GetComponent<C_Position>().Position, 0.1f);
                    if(e.Has<C_Scale>()) DragFloat2("Scale", ref e.GetComponent<C_Scale>().Scale, 0.1f);
                    if(e.Has<C_Rotation>()) ImGui.SliderAngle("Rotation", ref e.GetComponent<C_Rotation>().Angle, 0.0f, 360.0f);
                }

                if(ImGui.CollapsingHeader("Physics"))
                {
                    if(e.Has<C_Velocity>()) DragFloat2("Velocity", ref e.GetComponent<C_Velocity>().Velocity, 0.1f);
                    if(e.Has<C_TerminalVelocity>()) ImGui.DragFloat("Terminal Velocity", ref e.GetComponent<C_TerminalVelocity>().TerminalVelocity, 0.1f);
                }
                
                if(ImGui.CollapsingHeader("Graphics"))
                {
                    if(e.Has<C_Camera>()) ImGui.DragFloat("Camera Zoom", ref e.GetComponent<C_Camera>().Zoom, 0.1f, 0.01f, 3.0f);

                    if(e.Has<C_Sprite>()) 
                    {
                        ref var sprite = ref e.GetComponent<C_Sprite>();
                        ImGui.InputInt("Sprite Texture Index", ref sprite.TextureIndex);
                        ImGui.InputInt2("Sprite Size", ref sprite.SpriteWidth);
                        if(ImGui.InputInt("Sprite Frame", ref sprite.Frame)) sprite.SetRectangle();
                    }
                }

                ImGui.EndTabBar();
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

}