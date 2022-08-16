using Microsoft.Xna.Framework;
using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Physics {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Motion : IComponent, IGUIDrawable
    {
        public Vector2 PendingForces;
        public Vector2 Velocity;
        public float TerminalVelocity = 500.0f;
        public float Mass = 10.0f;
        public float AngularVelocity;
        public float Friction;

        public C_Motion(Vector2 velocity = default, float angularVelocity = 0.0f, float friction = 10.0f, float terminalVelocity = 500.0f, float mass = 10.0f) {
            PendingForces = Vector2.Zero;
            Velocity = velocity;
            TerminalVelocity=terminalVelocity;
            Mass = mass;
            AngularVelocity = angularVelocity;
            Friction = friction;
        }

        public string Label => "Motion";

        public void GUI_Draw()
        {
            System.Numerics.Vector2 vel = Velocity.MonoVec2SysVec();
            System.Numerics.Vector2 pdf = PendingForces.MonoVec2SysVec();
            
            if(ImGui.DragFloat2("Velocity", ref vel, 10.0f))
                Velocity = vel.SysVec2MonoVec();
            if(ImGui.DragFloat2("Pending Forces", ref pdf, 10.0f))
                PendingForces = pdf.SysVec2MonoVec();

            ImGui.DragFloat("Terminal Velocity", ref TerminalVelocity, 10.0f, 10.0f, 1000.0f);
            ImGui.DragFloat("Friction", ref Friction, 10.0f, 10.0f, 1000.0f);
        }
    }

}