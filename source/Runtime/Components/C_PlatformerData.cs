using ImGuiNET;

namespace ProjectMono.Gameplay {
    public class C_PlatformerData : IGUIDrawable
    {
        public float Acceleration = 950000.0f;
        public float TopSpeed = 100.0f;
        public float JumpForce = -25000.0f;
        public float GravityForce = 3500.0f;
        public float GroundFriction = 999.0f;
        public float AirFriction = 0.0f;
        public float HoldingJumpGravInfluence = 0.3f;
        
        public C_PlatformerData()
        {
        }
        public string Label => "Platformer Data";

        public void GUI_Draw()
        {
            ImGui.SliderFloat("Acceleration", ref Acceleration, 0.0f, 999999.0f);
            ImGui.SliderFloat("Top Speed", ref TopSpeed, 0.0f, 200.0f);
            ImGui.SliderFloat("Jump Force", ref JumpForce, 0.0f, 99999.0f);
            ImGui.SliderFloat("Gravity Force", ref GravityForce, -9999f, 0.0f);
            ImGui.SliderFloat("Ground Friction", ref GroundFriction, 0.0f, 999.0f);
            ImGui.SliderFloat("Air Friction", ref AirFriction, 0.0f, 999.0f);
            ImGui.SliderFloat("Holding Jump Gravity Influence", ref HoldingJumpGravInfluence, 0.0f, 1.0f);
        }

    }

}