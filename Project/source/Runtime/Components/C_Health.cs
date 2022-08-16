using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Gameplay {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Health : IComponent
    {
        public int Health;
        public int MaxHealth;

    }

}