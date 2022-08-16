using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Gameplay {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Player : IComponent, IGUIDrawable
    {
        public int Health;
        public int MaxHealth;
        public C_Player(int maxHealth = 100)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }
        public string Label => "Player";

        public void GUI_Draw()
        {
            ImGui.SliderInt("Health", ref Health, 0, MaxHealth);
        }

    }

}