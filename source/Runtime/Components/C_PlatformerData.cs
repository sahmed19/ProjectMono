using ImGuiNET;

namespace ProjectMono.Gameplay {
    public class C_PlatformerData : IGUIDrawable
    {
        public float MoveForce = 950000.0f;
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
            ImGui.DragFloat("Acceleration",                         ref MoveForce,                      1.0f);
            ImGui.DragFloat("Top Speed",                            ref TopSpeed,                       1.0f);
            ImGui.DragFloat("Jump Force",                           ref JumpForce,                      1.0f);
            ImGui.DragFloat("Gravity Force",                        ref GravityForce,                   1.0f);
            ImGui.DragFloat("Ground Friction",                      ref GroundFriction,                 1.0f);
            ImGui.DragFloat("Air Friction",                         ref AirFriction,                    1.0f);
            ImGui.DragFloat("Holding Jump Gravity Influence",       ref HoldingJumpGravInfluence,       1.0f);
        }

    }

}