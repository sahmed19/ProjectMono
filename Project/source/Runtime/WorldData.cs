using ProjectMono.Core;
using ProjectMono.Gameplay;
using ProjectMono.Physics;
using ProjectMono.Graphics;
using static flecs_hub.flecs;


namespace ProjectMono.Maps
{
    public static class WorldData
    {
        public readonly static int PIXELS_PER_UNIT = 16;

        public static void RegisterComponents(ProjectMonoApp game)
        {
            var world = game.World;
            world.RegisterComponent<C_Position>();
            world.RegisterComponent<C_Rotation>();
            world.RegisterComponent<C_Scale>();

            world.RegisterComponent<C_PendingForces>();
            world.RegisterComponent<C_Velocity>();
            world.RegisterComponent<C_Gravity>();
            world.RegisterComponent<C_TerminalVelocity>();
            world.RegisterComponent<C_AngularVelocity>();
            world.RegisterComponent<C_Friction>();
            world.RegisterComponent<C_Mass>();

            world.RegisterComponent<C_Camera>();
            
            world.RegisterComponent<C_PlatformerData>();
            world.RegisterComponent<C_PlatformerInput>();

            world.RegisterComponent<C_Health>();
            world.RegisterComponent<C_Sprite>();
            world.RegisterComponent<C_Color>();
            world.RegisterComponent<C_SpriteLayer>();
        }

        public static void RegisterSystems(ProjectMonoApp game)
        {
            var world = game.World;
            world.RegisterSystem(S_Physics.ApplyGravityToPendingForces, EcsOnUpdate,
              $"{typeof(C_PendingForces)}, {typeof(C_Gravity)}");

            world.RegisterSystem(S_Physics.ApplyPendingForcesToVelocity, EcsOnUpdate,
              $"{typeof(C_Velocity)}, {typeof(C_PendingForces)}, ?{typeof(C_Friction)}, ?{typeof(C_Mass)}");

            world.RegisterSystem(S_Physics.ClampVelocityXToTerminalVelocity, EcsOnUpdate,
              $"{typeof(C_Velocity)}, {typeof(C_TerminalVelocity)}");

            world.RegisterSystem(S_Physics.ApplyVelocityToPosition, EcsOnUpdate,
              $"{typeof(C_Position)}, {typeof(C_Velocity)}");
            
            world.RegisterSystem(S_Collision.BounceOffScreenEdge, EcsOnUpdate,
              $"{typeof(C_Position)}, {typeof(C_Velocity)}");
              
            world.RegisterSystem(S_Physics.RotateTowardVelocityDirection, EcsOnUpdate,
              $"{typeof(C_Rotation)}, {typeof(C_Velocity)}");

            world.RegisterSystem(S_Camera.UpdateCameraPosition, EcsPostUpdate,
              $"{typeof(C_Camera)}, {typeof(C_Position)}, ?{typeof(C_Rotation)}");
  
            world.RegisterSystem(S_SpriteRendering.PendSpritesForDraw, EcsPostUpdate, 
                $"{typeof(C_Sprite)}, {typeof(C_SpriteLayer)}, {typeof(C_Position)}, ?{typeof(C_Rotation)}, ?{typeof(C_Scale)}, ?{typeof(C_Color)}");
        }

    }
}