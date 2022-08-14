using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Gameplay;
using MonoGame.Extended;

namespace ProjectMono.Core {

    public class S_CameraSystem : EntityProcessingSystem
    {
        ComponentMapper<C_Camera> m_CameraMapper;
        ComponentMapper<C_Transform2> m_Transform2Mapper;
        OrthographicCamera m_Camera;

        public S_CameraSystem(OrthographicCamera camera) : base(Aspect.All(
            typeof(C_Camera),
            typeof(C_Transform2)))
        {
            m_Camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_CameraMapper = mapperService.GetMapper<C_Camera>();
            m_Transform2Mapper = mapperService.GetMapper<C_Transform2>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            var camera = m_CameraMapper.Get(entityID);
            var transform = m_Transform2Mapper.Get(entityID);
            
            m_Camera.Zoom = camera.Zoom;
            m_Camera.Position = transform.Position + new Vector2(-260.0f, -120.0f);
            m_Camera.Position = new Vector2(
                (int) m_Camera.Position.X,
                (int) m_Camera.Position.Y
            );
            m_Camera.Rotation = transform.Angle;
            
        }
    }

}