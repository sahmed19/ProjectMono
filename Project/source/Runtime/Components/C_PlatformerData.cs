using ImGuiNET;

namespace ProjectMono.Gameplay {
    public struct C_PlatformerData : IGUIDrawable
    {
        public float MoveForce;
        public float TopSpeed;
        public float JumpForce;
        public float GravityForce;
        public float GroundFriction;
        public float AirFriction;
        public float HoldingJumpGravInfluence;
        
        public C_PlatformerData(float moveForce = 950000.0f, float topSpeed = 100.0f, float jumpForce = -25000.0f, float gravityForce = 3500.0f, float groundFriction = 999.0f, float airFriction = 0.0f, float holdingJumpGravInfluence = 0.3f)
        {
            MoveForce = moveForce;
            TopSpeed = topSpeed;
            JumpForce = jumpForce;
            GravityForce = gravityForce;
            GroundFriction = groundFriction;
            AirFriction = airFriction;
            HoldingJumpGravInfluence = holdingJumpGravInfluence;
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