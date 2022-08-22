
using System;
using System.Numerics;
using Flecs;
using ImGuiNET;
using ProjectMono.Core;

namespace ProjectMono.Debugging
{
    //Entity Browser
    public static partial class MonoDebugger
    {
        const int MAX_DISPLAYABLE_ENTITIES = 2048;
        static Entity[] ALL_ENTITIES = new Entity[MAX_DISPLAYABLE_ENTITIES];
        static int SELECTED_ENTITY_INDEX=0, CURRENT_NUM_ENTITIES=0;
        public static Entity GetSelectedEntity() => ALL_ENTITIES[SELECTED_ENTITY_INDEX];
        const float FOCUS_ADJUSTMENT_SPEED = 1.0f;
        static bool FOCUS_SELECTED;
        
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

        public static void CameraFocusEntity(float deltaTime)
        {
            if(SELECTED_ENTITY_INDEX<0) return;
            var targetPos = GetSelectedEntity().GetComponent<C_Position>();
            S_Camera.SetActualCameraPosition(targetPos.Position, 0.0f, 3.0f);
        }

        static void GUI_EntityBrowser(ProjectMonoApp game, ref bool open) {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);
            
            if(ImGui.Begin("Entity Browser", ref open))
            {

                if(ImGui.BeginChild("Browser Buttons", new Vector2(ImGui.GetWindowWidth(), 30)))
                {
                    if(ImGui.Button("Refresh", new Vector2(70, 20)) || CURRENT_NUM_ENTITIES==0)
                        RefreshEntityList(game);
                    ImGui.SameLine();
                    if(ImGui.Checkbox("Focus", ref FOCUS_SELECTED)) { }
                    ImGui.EndChild();
                }
                
                
                Entity clickedEntity = GetSelectedEntity();

                //Navigate selection with up and down
                if(ImGui.IsKeyPressed(ImGuiKey.DownArrow)) SELECTED_ENTITY_INDEX++;
                else if(ImGui.IsKeyPressed(ImGuiKey.UpArrow)) SELECTED_ENTITY_INDEX--;
                SELECTED_ENTITY_INDEX = Math.Clamp(SELECTED_ENTITY_INDEX, 0, CURRENT_NUM_ENTITIES-1);

                if(ImGui.BeginChild("Entity List", new Vector2(ImGui.GetWindowWidth()-20, 0), true))
                {
                for (int i = 0; i < CURRENT_NUM_ENTITIES; i++)
                {    
                    var name = ALL_ENTITIES[i].Name();
                    if (ImGui.Selectable((""+i).PadLeft(3, '0') + ": " + name, SELECTED_ENTITY_INDEX == i))
                        SELECTED_ENTITY_INDEX = i;    
                }
                ImGui.EndChild();
                }
                
                ImGui.End();
            }    
        }
    }
}