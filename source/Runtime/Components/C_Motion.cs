using Microsoft.Xna.Framework;

namespace ProjectMono.Physics {
    public class C_Motion {
        public Vector2 PendingForces;
        public Vector2 Velocity;
        public float TerminalVelocity = 500.0f;
        public float AngularVelocity;
        public float GravityInfluence = 1.0f;
        public float Friction;

        public C_Motion() : this(Vector2.Zero) {}
        public C_Motion(Vector2 velocity) : this(velocity, 0.0f, 10.0f) {}
        public C_Motion(Vector2 velocity, float angularVelocity) : this(velocity, angularVelocity, 10.0f) {}
        public C_Motion(Vector2 velocity, float angularVelocity, float friction) {
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            PendingForces = Vector2.Zero;
            Friction = friction;
        }
        

    }

}