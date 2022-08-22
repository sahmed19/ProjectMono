using Microsoft.Xna.Framework;
using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Physics {

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Velocity : IComponent
    {
        public Vector2 Velocity;
        public C_Velocity() {Velocity=Vector2.Zero;}
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Gravity : IComponent
    {
        public Vector2 Gravity;
        public C_Gravity() {Gravity=Vector2.Zero;}
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_PendingForces : IComponent
    {
        public Vector2 PendingForces;
        public C_PendingForces() { PendingForces=Vector2.Zero; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_TerminalVelocity : IComponent
    {
        public float TerminalVelocity;
        public C_TerminalVelocity() { TerminalVelocity=100.0f; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Mass : IComponent
    {
        public float Mass;
        public C_Mass() { Mass=10.0f; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_AngularVelocity : IComponent
    {
        public float AngularVelocity;
        public C_AngularVelocity() { AngularVelocity=0.0f; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Friction : IComponent
    {
        public float Friction;
        public C_Friction() { Friction=0.0f; }
    }

}