using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Gameplay;
using MonoGame.Extended;

namespace ProjectMono.Core {

    public class S_CameraTargeting : EntityProcessingSystem
    {
        ComponentMapper<C_Camera> m_CameraMapper;
        ComponentMapper<C_Transform2> m_Transform2Mapper;

        public S_CameraTargeting() : base(Aspect.All(
            typeof(C_Camera),
            typeof(C_Transform2)))
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_CameraMapper = mapperService.GetMapper<C_Camera>();
            m_Transform2Mapper = mapperService.GetMapper<C_Transform2>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            var camera = m_CameraMapper.Get(entityID);
            var transform = m_Transform2Mapper.Get(entityID);
            var targetTransform = m_Transform2Mapper.Get(camera.ID_CameraTarget);

            if(!camera.FollowTarget) return;

            Vector2 displacement = targetTransform.Position - transform.Position;
            
            transform.Position += displacement * deltaTime * (1.0f / camera.TargetAdaptionTime);

            /* COME BACK TO SMOOTHDAMP WHEN IT WORKS
            transform.Position = MonoHelper.SmoothDamp(
                transform.Position,
                targetTransform.Position,
                ref camera.TargetAdaptionVelocity,
                camera.TargetAdaptionTime,
                100.0f,
                deltaTime);
            */
            
        }
    }

}