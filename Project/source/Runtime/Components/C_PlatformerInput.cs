using ImGuiNET;
namespace ProjectMono.Gameplay {
    public struct C_PlatformerInput : IGUIDrawable
    {
        public float HorizontalInput;
        public bool JumpBuffer, ShootBuffer;
        public bool HoldingJump;

        public C_PlatformerInput()
        {
            HorizontalInput=0.0f;
            JumpBuffer=false;
            ShootBuffer=false;
            HoldingJump=false;
        }
        public string Label => "Platformer Input";
        public void GUI_Draw()
        {
            ImGui.DragFloat("Horizontal Input", ref HorizontalInput, .1f, -1.0f, 1.0f);
            ImGui.Checkbox("Jump Buffer", ref JumpBuffer);
            ImGui.SameLine();
            ImGui.Checkbox("Shoot Buffer", ref ShootBuffer);
            ImGui.Checkbox("Holding Jump", ref HoldingJump);
        }

    }

}