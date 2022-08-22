using System;
using System.Diagnostics;
using ImGuiNET;
using ProjectMono.Core;
using System.Numerics;
using ProjectMono.Physics;
using ImGuiNET.XNA;

using Flecs;
using ProjectMono.Graphics;

namespace ProjectMono.Debugging
{

    public static partial class MonoDebugger 
    {

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

        static ImGuiRenderer Renderer;

        static bool CtrlShift =>
            ImGui.GetIO().KeyCtrl && 
            ImGui.GetIO().KeyShift;

        static bool Shortcut(ImGuiKey key) => CtrlShift && ImGui.IsKeyPressed(key);

        static bool 
            OPEN_GENERAL_SETTINGS = false, 
            OPEN_ENTITY_BROWSER = true, 
            OPEN_ENTITY_INSPECTOR = true,
            OPEN_DEMO_WINDOW = false,
            OPEN_CONTENT_VIEW = true;

        public static void Initialize(out ImGuiRenderer renderer, Microsoft.Xna.Framework.Game game)
        {
            renderer = new ImGuiRenderer(game);  
            Renderer = renderer; 
            Renderer.RebuildFontAtlas();
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        }

        public static void GUI_DebuggerUpdate(ProjectMonoApp game, float deltaTime) { }

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
            if(Shortcut(ImGuiKey.C))
                OPEN_CONTENT_VIEW=!OPEN_CONTENT_VIEW;
            if(Shortcut(ImGuiKey.D))
                OPEN_DEMO_WINDOW=!OPEN_DEMO_WINDOW;

            if(OPEN_GENERAL_SETTINGS)   GUI_Settings(game, ref OPEN_GENERAL_SETTINGS);
            if(OPEN_ENTITY_BROWSER)     GUI_EntityBrowser(game, ref OPEN_ENTITY_BROWSER);
            if(OPEN_ENTITY_INSPECTOR)   GUI_EntityInspector(game, ref OPEN_ENTITY_INSPECTOR);
            if(OPEN_CONTENT_VIEW)       GUI_ContentView(game, ref OPEN_CONTENT_VIEW);
            if(OPEN_DEMO_WINDOW)        ImGui.ShowDemoWindow();
            
        }

        #endregion

    }

}