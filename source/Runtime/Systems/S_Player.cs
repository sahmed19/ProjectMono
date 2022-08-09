using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Gameplay;
using ProjectMono.Input;
using ProjectMono.Graphics;

namespace ProjectMono.Physics {

    public class S_Platformer : EntityProcessingSystem
    {
        ComponentMapper<C_PlatformerData> m_PlayerDataMapper;
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
            m_PlayerDataMapper = mapperService.GetMapper<C_PlatformerData>();
            m_PlayerInputMapper = mapperService.GetMapper<C_PlatformerInput>();
            m_MotionMapper = mapperService.GetMapper<C_Motion>();
            m_TransformMapper = mapperService.GetMapper<C_Transform2>();
            m_SpriteMapper = mapperService.GetMapper<C_Sprite>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            var input = m_PlayerInputMapper.Get(entityID);
            var data = m_PlayerDataMapper.Get(entityID);
            var motion = m_MotionMapper.Get(entityID);
            var transform = m_TransformMapper.Get(entityID);
            var sprite = m_SpriteMapper.Get(entityID);
            

            motion.GravityInfluence = (input.HoldingJump && motion.Velocity.Y > 0.0f)? 
                data.HoldingJumpGravInfluence : 1.0f;

            motion.TerminalVelocity = data.TopSpeed;
            motion.PendingForces.Y += data.GravityForce * motion.GravityInfluence;

            if(transform.Position.Y <= 100.0f) {
                motion.PendingForces.X += input.HorizontalInput * data.Acceleration * deltaTime;
                if(input.JumpBuffer) {
                    motion.PendingForces.Y += data.JumpForce;
                    input.JumpBuffer = false;
                }
            }

            //flip x of sprite
            sprite.FlipX = motion.Velocity.X > 0.0f;

        }
    }

}