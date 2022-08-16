using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;
namespace ProjectMono.Gameplay {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_PlatformerInput : IComponent
    {
        public float HorizontalInput;
        public bool JumpBuffer, ShootBuffer;
        public bool HoldingJump;
    }

}