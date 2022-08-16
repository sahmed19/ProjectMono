using Microsoft.Xna.Framework;

using ProjectMono.Gameplay;
using MonoGame.Extended;
using Flecs;

namespace ProjectMono.Core {

    public static class S_Camera
    {
        public static void UpdateCameraPosition(Iterator it)
        {
            var camIter = it.Field<C_Camera>(1);
            var posIter = it.Field<C_Position>(2);
            var rotIter = it.Field<C_Rotation>(3);
            bool hasRot = it.FieldIsSet(3);
            var actualCamera = ProjectMonoApp.INSTANCE.Camera;
            
            for(int i=0; i<it.Count; i++){
                var cam = camIter[i];
                var pos = posIter[i];
                var angle = hasRot? rotIter[i].Angle : 0.0f;

                actualCamera.Zoom = cam.Zoom;

                actualCamera.Position = (pos.Position - new Vector2(10.0f, 8.0f)) * WorldData.PIXELS_PER_UNIT;
                actualCamera.Position = new Vector2(
                    (int) actualCamera.Position.X,
                    (int) actualCamera.Position.Y
                );
                actualCamera.Rotation = angle;
            }
        }
    }

}