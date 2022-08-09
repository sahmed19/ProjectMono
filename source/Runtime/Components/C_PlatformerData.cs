using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.Gameplay {
    public class C_PlatformerData
    {
        public float Acceleration = 450000.0f;
        public float TopSpeed = 500000.0f;
        public float JumpForce = 45000.0f;
        public float GravityForce = -4500.0f;
        public float GroundFriction = 15.0f;
        public float AirFriction = 15.0f;
        public float HoldingJumpGravInfluence = 0.3f;
        
        public C_PlatformerData()
        {
        }

    }

}