using ImGuiNET;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Gameplay {
    [StructLayout(LayoutKind.Sequential)]
    public struct C_PlatformerData : IComponent
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

    }

}