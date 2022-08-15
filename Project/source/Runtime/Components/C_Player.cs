using ImGuiNET;

namespace ProjectMono.Gameplay {
    public struct C_Player : IGUIDrawable
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