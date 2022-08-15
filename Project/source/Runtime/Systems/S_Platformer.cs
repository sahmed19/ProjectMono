using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Physics;
using ProjectMono.Core;
using ProjectMono.Graphics;

namespace ProjectMono.Gameplay {

    public class S_Platformer : EntityProcessingSystem
    {
        ComponentMapper<C_PlatformerData> m_PlatformerDataMapper;
        ComponentMapper<C_PlatformerInput> m_PlayerInputMapper;
        ComponentMapper<C_Sprite> m_SpriteMapper;
        ComponentMapper<C_Motion> m_MotionMapper;
        ComponentMapper<C_Transform2> m_TransformMapper;

        public S_Platformer() : base(Aspect.All(
            typeof(C_PlatformerInput),
            typeof(C_PlatformerData),
            typeof(C_Motion),
            typeof(C_Transform2),
            typeof(C_Sprite)))
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_PlatformerDataMapper = mapperService.GetMapper<C_PlatformerData>();
            m_PlayerInputMapper = mapperService.GetMapper<C_PlatformerInput>();
            m_MotionMapper = mapperService.GetMapper<C_Motion>();
            m_TransformMapper = mapperService.GetMapper<C_Transform2>();
            m_SpriteMapper = mapperService.GetMapper<C_Sprite>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            var input = m_PlayerInputMapper.Get(entityID);
            var data = m_PlatformerDataMapper.Get(entityID);
            var motion = m_MotionMapper.Get(entityID);
            var transform = m_TransformMapper.Get(entityID);
            var sprite = m_SpriteMapper.Get(entityID);

            var gravityInfluence = (input.HoldingJump && motion.Velocity.Y < 0.0f)? 
                data.HoldingJumpGravInfluence : 1.0f;

            motion.TerminalVelocity = data.TopSpeed;
            motion.PendingForces.Y += data.GravityForce * gravityInfluence;

            if(transform.Position.Y >= 10.0f) {
                motion.PendingForces.X += input.HorizontalInput * data.MoveForce * deltaTime;
                if(input.JumpBuffer) {
                    motion.PendingForces.Y += data.JumpForce;
                    input.JumpBuffer = false;
                }

                motion.Friction = data.GroundFriction;
            } else {
                motion.Friction = data.AirFriction;
            }
            
            //flip x of sprite
            if(System.Math.Abs(motion.Velocity.X) > 0.1f) {
                sprite.FlipX = motion.Velocity.X > 0.0f;
            }

        }
    }

}