using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.Gameplay {

    public class C_Player
    {
        public const float ACCELERATION = 450000.0f;
        public const float TOP_SPEED = 500000.0f;
        public const float JUMP_FORCE = 45000.0f;
        public const float GRAVITY_FORCE = -3500.0f;
        public const float GROUND_FRICTION = 15.0f;
        public const float AIR_FRICTION = 15.0f;

        public const float HOLDING_JUMP_GRAV_INFLUENCE = 0.3f;
        
        public C_Player()
        {
        }

    }

}