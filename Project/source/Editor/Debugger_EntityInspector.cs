using System.Numerics;
using Flecs;
using ImGuiNET;
using ProjectMono.Core;
using ProjectMono.Graphics;
using ProjectMono.Physics;

namespace ProjectMono.Debugging
{
    public static partial class MonoDebugger
    {
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
                    if(e.Has<C_Position>()) InputFloat2("Position", ref e.GetComponent<C_Position>().Position, 0.1f);
                    if(e.Has<C_Scale>()) InputFloat2("Scale", ref e.GetComponent<C_Scale>().Scale, 0.1f);
                    if(e.Has<C_Rotation>()) ImGui.SliderAngle("Rotation", ref e.GetComponent<C_Rotation>().Angle, 0.0f, 360.0f);
                }

                if(ImGui.CollapsingHeader("Physics"))
                {
                    if(e.Has<C_Velocity>()) InputFloat2("Velocity", ref e.GetComponent<C_Velocity>().Velocity, 0.1f);
                    if(e.Has<C_TerminalVelocity>()) ImGui.InputFloat("Terminal Velocity", ref e.GetComponent<C_TerminalVelocity>().TerminalVelocity, 0.1f);
                }
                
                if(ImGui.CollapsingHeader("Graphics"))
                {
                    if(e.Has<C_Camera>()) ImGui.InputFloat("Camera Zoom", ref e.GetComponent<C_Camera>().Zoom, .1f);
                    if(e.Has<C_Sprite>()) 
                    {
                        ref var sprite = ref e.GetComponent<C_Sprite>();
                        ImGui.SetNextItemWidth(100);
                        ImGui.InputInt("Sprite Texture Index", ref sprite.TextureIndex);
                        ImGui.SetNextItemWidth(100);
                        ImGui.InputInt2("Sprite Size", ref sprite.SpriteWidth);
                        ImGui.SetNextItemWidth(100);
                        if(ImGui.InputInt("Sprite Frame", ref sprite.Frame)) sprite.SetRectangle();
                    }
                    if(e.Has<C_Color>()) InputColor4("Color", ref e.GetComponent<C_Color>().Color);

                }

                ImGui.EndTabBar();
                ImGui.End();
            }

            static void InputFloat2(string label, ref Microsoft.Xna.Framework.Vector2 vector, float speed)
            {
                System.Numerics.Vector2 vec = vector.MonoVec2SysVec();
                if(ImGui.InputFloat2(label, ref vec))
                {
                    vector = vec.SysVec2MonoVec();
                }
            }
/*
            static void InputColor3(string label, ref Microsoft.Xna.Framework.Color color)
            {
                System.Numerics.Vector3 vec = color.MonoCol2SysVec_RGB();
                if(ImGui.ColorEdit3(label, ref vec))
                {
                    color = vec.SysVec2MonoCol_RGB();
                }
            }
*/
            static void InputColor4(string label, ref Microsoft.Xna.Framework.Color color)
            {
                System.Numerics.Vector4 vec = color.MonoCol2SysVec_RGBA();
                if(ImGui.ColorEdit4(label, ref vec))
                {
                    color = vec.SysVec2MonoCol_RGBA();
                }
            }
        }
    }
}