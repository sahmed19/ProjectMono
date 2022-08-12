using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.Gameplay {
    public class C_PlatformerData
    {
        public float Acceleration = 950000.0f;
        public float TopSpeed = 100.0f;
        public float JumpForce = 25000.0f;
        public float GravityForce = -3500.0f;
        public float GroundFriction = 1000.0f;
        public float AirFriction = 0.0f;
        public float HoldingJumpGravInfluence = 0.3f;
        
        public C_PlatformerData()
        {
        }

    }

}