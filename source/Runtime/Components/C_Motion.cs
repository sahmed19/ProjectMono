using Microsoft.Xna.Framework;
using ImGuiNET;

namespace ProjectMono.Physics {
    public class C_Motion : IGUIDrawable {
        public Vector2 PendingForces;
        public Vector2 Velocity;
        public float TerminalVelocity = 500.0f;
        public float AngularVelocity;
        public float GravityInfluence = 1.0f;
        public float Friction;

        public C_Motion() : this(Vector2.Zero) {}
        public C_Motion(Vector2 velocity) : this(velocity, 0.0f, 10.0f) {}
        public C_Motion(Vector2 velocity, float angularVelocity) : this(velocity, angularVelocity, 10.0f) {}
        public C_Motion(Vector2 velocity, float angularVelocity, float friction) {
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            PendingForces = Vector2.Zero;
            Friction = friction;
        }

        public string Label => "Motion";

        public void GUI_Draw()
        {
            System.Numerics.Vector2 vel = Velocity.MonoVec2SysVec();
            System.Numerics.Vector2 pdf = Velocity.MonoVec2SysVec();
            
            if(ImGui.DragFloat2("Velocity", ref vel, 10.0f))
                Velocity = vel.SysVec2MonoVec();
            if(ImGui.DragFloat2("Pending Forces", ref pdf, 10.0f))
                PendingForces = pdf.SysVec2MonoVec();

            ImGui.DragFloat("Terminal Velocity", ref TerminalVelocity, 10.0f, 10.0f, 1000.0f);
            ImGui.DragFloat("Friction", ref Friction, 10.0f, 10.0f, 1000.0f);
        }
    }

}