using Microsoft.Xna.Framework;

namespace ProjectMono.Physics {


    public class C_Motion {
        public Vector2 PendingForces;
        public Vector2 Velocity;
        public float TerminalVelocity = 500.0f;
        public float AngularVelocity;

        public float Friction {get; private set; }

        public C_Motion() : this(Vector2.Zero) {}
        public C_Motion(Vector2 velocity) : this(velocity, 0.0f) {}
        public C_Motion(Vector2 velocity, float angularVelocity) {
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            PendingForces = Vector2.Zero;
            Friction = .1f;
        }

        public C_Motion WithFriction(float friction) {
            Friction = friction;
            return this;
        }
        

    }

}