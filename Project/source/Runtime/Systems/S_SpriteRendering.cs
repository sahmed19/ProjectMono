using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMono.Core;
using Flecs;

namespace ProjectMono.Graphics {

    public static class S_SpriteRendering
    {

        public static void Draw(ProjectMonoApp game)
        {
            var it = game.World.EntityIterator<C_Sprite>();
            

            while (it.HasNext()) 
            {
                var spriteIter = it.Field<C_Sprite>(1);
                var transformIter = it.Field<C_Transform2>(2);
                for(int i = 0; i < it.Count; i++)
                {
                    DebuggerManager.Print("index is " + i);
                    var sprite = spriteIter[i];
                    var transform = it.Entity(i).GetComponent<C_Transform2>();

                    Vector2 screenspacePosition = transform.Position * WorldData.PIXELS_PER_UNIT;
                    DebuggerManager.Print(it.Entity(i).Name() + ": texindex: " + sprite.TextureIndex);

                    game.SpriteBatch.Draw(
                        sprite.Texture,
                        screenspacePosition,
                        sprite.Rectangle,
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

}