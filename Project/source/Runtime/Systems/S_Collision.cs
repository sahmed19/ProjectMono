using Microsoft.Xna.Framework;
using ProjectMono.Core;
using Flecs;

namespace ProjectMono.Physics {

    public static class S_Collision
    {



        public static void BounceOffScreenEdge(Iterator it) {

            var posIter = it.Field<C_Position>(1);
            var velIter = it.Field<C_Velocity>(2);

            var cam = ProjectMonoApp.INSTANCE.Camera;

            var topLeft = Vector2.One * -10.0f;
            var bottomRight = Vector2.One * 10.0f;

            for(int i=0; i<it.Count; i++) {
                var pos = posIter[i].Position;
                ref var vel = ref velIter[i];

                if(pos.X <= topLeft.X || pos.X >= bottomRight.X) {
                    vel.Velocity.X *= -1.0f;
                }
                if(pos.Y <= topLeft.Y || pos.Y >= bottomRight.Y) {
                    vel.Velocity.Y *= -1.0f;
                }
            }

        }

    }

} 