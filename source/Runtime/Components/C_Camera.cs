using Microsoft.Xna.Framework;

namespace ProjectMono.Core {
    public class C_Camera
    {
        public int ID_CameraTarget;
        public float Zoom;
        public float TargetAdaptionTime = .3f;
        public Vector2 TargetAdaptionVelocity;

        public C_Camera()
        {
            Zoom = 1.0f;
        }
    }

}