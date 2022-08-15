using Microsoft.Xna.Framework;
using ImGuiNET;

namespace ProjectMono.Core {
    public class C_Transform2 : IGUIDrawable {
        public Vector2 Position;
        public Vector2 Scale;
        public float Angle;
        
//CONSTRUCTORS
        public C_Transform2() : this(Vector2.Zero) {}
        public C_Transform2(Vector2 position) : this(position, 0.0f) {}
        public C_Transform2(Vector2 position, float angle) : this(position, angle, Vector2.One) {}
        public C_Transform2(Vector2 position, float angle, Vector2 scale) {
            Position = position;
            Angle = angle;
            Scale = scale;
        }

        public string Label => "Transform";
        public void GUI_Draw()
        {
            System.Numerics.Vector2 pos = Position.MonoVec2SysVec();
            System.Numerics.Vector2 scl = Scale.MonoVec2SysVec();
            if(ImGui.DragFloat2("Position", ref pos, 10.0f))
                Position = pos.SysVec2MonoVec();
            if(ImGui.DragFloat2("Scale", ref scl, 10.0f))
                Scale = scl.SysVec2MonoVec();
            ImGui.DragFloat("Angle", ref Angle);
        }
    }
}