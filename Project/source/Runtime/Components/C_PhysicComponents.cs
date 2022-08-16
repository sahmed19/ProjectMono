using Microsoft.Xna.Framework;
using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Physics {

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Velocity : IComponent
    {
        public Vector2 Velocity;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Gravity : IComponent
    {
        public Vector2 Gravity;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_PendingForces : IComponent
    {
        public Vector2 PendingForces;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_TerminalVelocity : IComponent
    {
        public float TerminalVelocity;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Mass : IComponent
    {
        public float Mass;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_AngularVelocity : IComponent
    {
        public float AngularVelocity;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Friction : IComponent
    {
        public float Friction;
    }

}