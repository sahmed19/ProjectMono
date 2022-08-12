using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Physics;

namespace ProjectMono.Graphics {

    public class S_SpriteRendering : EntityDrawSystem
    {
        readonly SpriteBatch m_SpriteBatch;

        ComponentMapper<C_Transform2> m_Transform2Mapper;
        ComponentMapper<C_Sprite> m_SpriteMapper;

        public S_SpriteRendering(SpriteBatch spriteBatch) : base(Aspect.All(
            typeof(C_Transform2),
            typeof(C_Sprite)))
        {
            m_SpriteBatch = spriteBatch;
        }


        public override void Initialize(IComponentMapperService mapperService)
        {
            m_Transform2Mapper = mapperService.GetMapper<C_Transform2>();
            m_SpriteMapper = mapperService.GetMapper<C_Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var transform = m_Transform2Mapper.Get(entity);
                var sprite = m_SpriteMapper.Get(entity);

                Vector2 screenspacePosition = transform.Position;
                screenspacePosition.Y = GraphicsDeviceManager.DefaultBackBufferHeight - screenspacePosition.Y;

                m_SpriteBatch.Draw(
                    sprite.Texture,
                    screenspacePosition,
                    new Rectangle(0, 0, 16, 16),
                    Color.White,
                    transform.Angle,
                    sprite.GetOrigin(),
                    transform.Scale,
                    sprite.FlipX? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0f
                );
            }
        }

    }

}