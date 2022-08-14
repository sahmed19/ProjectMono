using ImGuiNET;

namespace ProjectMono.Gameplay {
    public class C_Player : IGUIDrawable
    {
        public int Health = 10;
        public int MaxHealth = 100;
        public C_Player()
        {
        }
        public string Label => "Player";

        public void GUI_Draw()
        {
            ImGui.SliderInt("Health", ref Health, 0, MaxHealth);
        }

    }

}