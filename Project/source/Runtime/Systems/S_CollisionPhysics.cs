using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using ProjectMono.Core;

namespace ProjectMono.Physics {

    public class S_CollisionPhysics : EntityProcessingSystem
    {
        ComponentMapper<C_Motion> m_MotionMapper;
        ComponentMapper<C_Transform2> m_TransformMapper;

        public S_CollisionPhysics() : base(Aspect.All(
            typeof(C_Motion),
            typeof(C_Transform2)))
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_MotionMapper = mapperService.GetMapper<C_Motion>();
            m_TransformMapper = mapperService.GetMapper<C_Transform2>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            C_Motion motion = m_MotionMapper.Get(entityID);
            C_Transform2 transform = m_TransformMapper.Get(entityID);

            if(transform.Position.Y >= 10.0f) {
                transform.Position.Y = 10.0f;
                motion.Velocity.Y = MathHelper.Min(motion.Velocity.Y, 0.0f);
                motion.PendingForces.Y = MathHelper.Min(motion.PendingForces.Y, 0.0f);
            }

        }
    }

}