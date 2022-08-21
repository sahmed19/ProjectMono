using System;
using Microsoft.Xna.Framework;
using Flecs;
using ProjectMono.Core;

namespace ProjectMono.Physics {

    public static class S_Physics
    {
        public static void ApplyGravityToPendingForces(Iterator it)
        {
            var pdfIter = it.Field<C_PendingForces>(1);
            var gravIter = it.Field<C_Gravity>(2);

            for(int i = 0; i < it.Count; i++)
            {
                ref var pdf = ref pdfIter[i];
                var grav = gravIter[i];
                pdf.PendingForces += grav.Gravity;
            }
        }

        public static void ApplyPendingForcesToVelocity(Iterator it)
        {
            float deltaTime = it.DeltaSystemTime();
            
            var velIter = it.Field<C_Velocity>(1);
            var pdfIter = it.Field<C_PendingForces>(2);

            var frictionIter = it.Field<C_Friction>(3);
            bool hasFriction = it.FieldIsSet(3);
            
            var massIter = it.Field<C_Mass>(4);
            bool hasMass = it.FieldIsSet(4);

            for(int i = 0; i < it.Count; i++)
            {
                ref var vel = ref velIter[i];
                ref var pdf = ref pdfIter[i];
                var mass = hasMass? massIter[i].Mass : 1.0f;
                var friction = hasFriction? frictionIter[i].Friction : 0.0f;

                //friction
                vel.Velocity.X *= 1.0f - MathHelper.Clamp(friction * deltaTime, 0.0f, 1.0f);
                //apply force
                vel.Velocity += deltaTime * pdf.PendingForces / mass;
                //set pending force to zero
                pdf.PendingForces=Vector2.Zero;
            }
        }

        public static void ClampVelocityXToTerminalVelocity(Iterator it)
        {
            float deltaTime = it.DeltaSystemTime();
            
            var velIter = it.Field<C_Velocity>(1);
            var tvIter = it.Field<C_TerminalVelocity>(2);

            for(int i = 0; i < it.Count; i++)
            {
                ref var vel = ref velIter[i];
                var tv = tvIter[i];

                vel.Velocity.X = MathHelper.Clamp(vel.Velocity.X, -tv.TerminalVelocity, tv.TerminalVelocity);
            }
        }

        public static void ApplyVelocityToPosition(Iterator it)
        {
            float deltaTime = it.DeltaSystemTime();
            
            var posIter = it.Field<C_Position>(1);
            var velIter = it.Field<C_Velocity>(2);

            for(int i = 0; i < it.Count; i++)
            {
                ref var pos = ref posIter[i];
                var vel = velIter[i];

                pos.Position += vel.Velocity * deltaTime;
            }
        }
        
        public static void RotateTowardVelocityDirection(Iterator it)
        {

            var rotIter = it.Field<C_Rotation>(1);
            var velIter = it.Field<C_Velocity>(2);

            for(int i=0; i<it.Count; i++) {
                ref var rot = ref rotIter[i];
                var vel = velIter[i].Velocity;

                rot.Angle = (float) Math.Atan2(vel.Y, vel.X);
            }

        }

        public static Entity GetEntityClosestToPoint(World world, Vector2 point)
        {
            var it = world.EntityIterator<C_Position>();

            Entity champ = default;
            float lowestSqrDist = float.MaxValue;

            while(it.HasNext()) {
                var posIter = it.Field<C_Position>(1);

                for(int i = 0; i < it.Count; i++)
                {
                    float dist = Vector2.DistanceSquared(posIter[i].Position, point);
                    if(dist < lowestSqrDist)
                    {
                        lowestSqrDist=dist;
                        champ=it.Entity(i);
                    }
                }
            }

            return champ;
        }
    }

}