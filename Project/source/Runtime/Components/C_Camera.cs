using Microsoft.Xna.Framework;
using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Core {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Camera : IComponent
    {
        public float Zoom;
    }

}