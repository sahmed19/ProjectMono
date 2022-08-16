using Microsoft.Xna.Framework;
using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Core {

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Position : IComponent {
        public Vector2 Position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Scale : IComponent {
        public Vector2 Scale;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Rotation : IComponent {
        public float Angle;
    }
}