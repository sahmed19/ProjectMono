using Microsoft.Xna.Framework;

namespace ProjectMono.Physics {
    public class C_Transform2 {
        public Vector2 Position;
        public Vector2 Scale;
        public float Angle;
        
//CONSTRUCTORS
        public C_Transform2() : this(Vector2.Zero) {}
        public C_Transform2(Vector2 position) : this(position, 0.0f) {}
        public C_Transform2(Vector2 position, float angle) : this(position, angle, Vector2.One) {}
        public C_Transform2(Vector2 position, float angle, Vector2 scale) {
            Position = position;
            Angle = angle;
            Scale = scale;
        }
    }
}