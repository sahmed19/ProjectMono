
using ProjectMono.Maps;
using Microsoft.Xna.Framework;
using Flecs;

namespace ProjectMono.Core {

    public static class S_Camera
    {

        static Vector2 camOffset = new Vector2(10.0f, 6.0f);

        public static void SetActualCameraPosition(Vector2 position, float angle = 0.0f, float zoom = 1.0f) {
            var actualCamera = ProjectMonoApp.INSTANCE.Camera;
            
            actualCamera.Zoom = zoom;
            actualCamera.Rotation = angle;

            actualCamera.Position = (position - camOffset) * WorldData.PIXELS_PER_UNIT;
            actualCamera.Position = new Vector2(
                (int) actualCamera.Position.X,
                (int) actualCamera.Position.Y
            );
        }

        public static void UpdateCameraPosition(Iterator it)
        {
            var camIter = it.Field<C_Camera>(1);
            var posIter = it.Field<C_Position>(2);
            var rotIter = it.Field<C_Rotation>(3);
            bool hasRot = it.FieldIsSet(3);

            for(int i=0; i<it.Count; i++){
                var cam = camIter[i];
                var pos = posIter[i];
                var angle = hasRot? rotIter[i].Angle : 0.0f;

                SetActualCameraPosition(pos.Position, angle, cam.Zoom);
            }
        }

        public static Vector2 ScreenToWorldSpace(Vector2 screenspacePosition, ProjectMonoApp game)
        {
            Vector2 vec = game.Camera.ScreenToWorld(screenspacePosition) / WorldData.PIXELS_PER_UNIT;
            vec += camOffset;
            return vec;
        }
    }

}