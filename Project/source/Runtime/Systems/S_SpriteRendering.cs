using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMono.Core;
using Flecs;

namespace ProjectMono.Graphics {

    public static class S_SpriteRendering
    {
        struct PendingSprite {
            public int TextureID;
            public Vector2 Position;
            public Rectangle SourceRectangle;
            public Color Color;
            public float Angle;
            public Vector2 Origin;
            public Vector2 Scale;
            public SpriteEffects SpriteEffects;
            public float LayerDepth;
        }
        
        static PendingSprite[] m_PendingSprites = new PendingSprite[MAX_PENDING_SPRITES];

        const int MAX_PENDING_SPRITES = 65536;
        
        static int CURRENT_NUM_SPRITES;

        public static void PendSpritesForDraw(Iterator it)
        {
            CURRENT_NUM_SPRITES = it.Count;

            bool rotationSet = false, scaleSet = false;

            var spriteIter = it.Field<C_Sprite>(1);
            var posIter = it.Field<C_Position>(2);
            var rotIter = it.Field<C_Rotation>(3);
            var scaleIter = it.Field<C_Scale>(4);

            if(it.FieldIsSet(3)) rotationSet=true;
            if(it.FieldIsSet(4)) scaleSet=true;
            
            //var transformIter = it.Field<C_Transform2>(2);

            for(int i = 0; i < CURRENT_NUM_SPRITES; i++)
            {
                var sprite = spriteIter[i];
                Vector2 pos = posIter[i].Position;
                float rot = rotationSet? rotIter[i].Angle : 0.0f;
                Vector2 scl = scaleSet? scaleIter[i].Scale : Vector2.One;

                m_PendingSprites[i] = new PendingSprite{
                    TextureID=sprite.TextureIndex,
                    Position=pos * WorldData.PIXELS_PER_UNIT,
                    SourceRectangle=sprite.Rectangle,
                    Color=Color.White,
                    Angle=rot,
                    Origin=sprite.GetOrigin(),
                    Scale=scl,
                    SpriteEffects=sprite.FlipX?SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    LayerDepth=0.1f
                };
            }
        }

        public static void DrawPendingSprites(ProjectMonoApp game) {
            
            for(var i = 0; i < CURRENT_NUM_SPRITES; i++) {
                var pendingSprite = m_PendingSprites[i];
                game.SpriteBatch.Draw(
                    TextureDatabase.GetTexture(pendingSprite.TextureID),
                    pendingSprite.Position,
                    pendingSprite.SourceRectangle,
                    pendingSprite.Color,
                    pendingSprite.Angle,
                    pendingSprite.Origin,
                    pendingSprite.Scale,
                    pendingSprite.SpriteEffects,
                    pendingSprite.LayerDepth
                );
            }

        }

    }

}