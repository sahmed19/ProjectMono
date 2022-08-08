using Microsoft.Xna.Framework;

namespace ProjectMono.ECS.Components {
    public class C_Transform2 {
        public Vector2 Position { get; private set; }
        public Vector2 Scale { get; private set; }
        public float Angle {get; private set; }
        
//CONSTRUCTORS
        public C_Transform2() : this(Vector2.Zero) {}
        public C_Transform2(Vector2 position) : this(position, 0.0f) {}
        public C_Transform2(Vector2 position, float angle) : this(position, angle, Vector2.One) {}
        public C_Transform2(Vector2 position, float angle, Vector2 scale) {
            Position = position;
            Angle = angle;
            Scale = scale;
        }
        public void SetPosition(Vector2 position) => Position = position;
        public void AddPosition(Vector2 deltaPos) => Position += deltaPos;
        public void SetAngle(float angle) => Angle = angle;
        public void AddAngle(float deltaAngle) => Angle += deltaAngle;
    }
}