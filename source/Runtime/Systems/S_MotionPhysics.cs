using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace ProjectMono.Physics {

    public class S_MotionPhysics : EntityProcessingSystem
    {
        ComponentMapper<C_Transform2> m_Transform2Mapper;
        ComponentMapper<C_Motion> m_MotionMapper;

        public S_MotionPhysics() : base(Aspect.All(
            typeof(C_Transform2),
            typeof(C_Motion)))
        { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_Transform2Mapper = mapperService.GetMapper<C_Transform2>();
            m_MotionMapper = mapperService.GetMapper<C_Motion>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            C_Transform2 transform = m_Transform2Mapper.Get(entityID);
            C_Motion motion = m_MotionMapper.Get(entityID);

            //Angle
            transform.Angle += motion.AngularVelocity * deltaTime;

            //velocity: v += at
            motion.Velocity += motion.PendingForces * deltaTime;
            motion.PendingForces = Vector2.Zero;

            //clamp velocity to terminal velocity
            motion.Velocity.X = MathHelper.Clamp(motion.Velocity.X, -motion.TerminalVelocity, motion.TerminalVelocity);

            //displacement: d += vt
            transform.Position += motion.Velocity * deltaTime;

            //friction: v *= 1 - ft
            if(transform.Position.Y <= 100.0f)
                motion.Velocity.X *= 1.0f - MathHelper.Clamp(motion.Friction * deltaTime, 0.0f, 1.0f);
                
        }
    }

}