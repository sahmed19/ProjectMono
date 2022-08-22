using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMono.Core;
using ProjectMono.Maps;
using ProjectMono.Debugging;
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
        
        static int NUM_SPRITES=0;

        static int SELECTED_SPRITE=-1;

        public static void PendSpritesForDraw(Iterator it)
        {

            bool colSet = false, rotationSet = false, scaleSet = false;

            var spriteIter = it.Field<C_Sprite>(1);
            var sprLayerIter = it.Field<C_SpriteLayer>(2);
            var posIter = it.Field<C_Position>(3);
            var colIter = it.Field<C_Color>(4);
            var rotIter = it.Field<C_Rotation>(5);
            var scaleIter = it.Field<C_Scale>(6);

            if(it.FieldIsSet(4)) colSet=true;
            if(it.FieldIsSet(5)) rotationSet=true;
            if(it.FieldIsSet(6)) scaleSet=true;

            for(int i = 0; i < it.Count; i++)
            {
                var sprite = spriteIter[i];
                var sprLayer = sprLayerIter[i];
                Vector2 pos = posIter[i].Position;
                Color col = colSet? colIter[i].Color : Color.White;
                float rot = rotationSet? rotIter[i].Angle : 0.0f;
                Vector2 scl = scaleSet? scaleIter[i].Scale : Vector2.One;

                bool selected = it.Entity(i).Equals(MonoDebugger.GetSelectedEntity());
                if(selected)
                    SELECTED_SPRITE=NUM_SPRITES;

                m_PendingSprites[NUM_SPRITES] = new PendingSprite{
                    TextureID=sprite.TextureIndex,
                    Position=pos * WorldData.PIXELS_PER_UNIT,
                    SourceRectangle=sprite.Rectangle,
                    Color=col,
                    Angle=rot,
                    Origin=sprLayer.GetOrigin(sprite.SpriteWidth, sprite.SpriteHeight),
                    Scale=scl,
                    SpriteEffects=sprite.FlipX? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    LayerDepth=0.9f
                };
                
                NUM_SPRITES++;
            }

        }

        public static void DrawPendingSprites(ProjectMonoApp game)
        {
            
            for(var i = 0; i < NUM_SPRITES; i++) {
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
                if(SELECTED_SPRITE==i)
                {
                    PrimitiveDrawer.DrawPulsingOutline(game, new Rectangle(
                        (int) pendingSprite.Position.X, 
                        (int) pendingSprite.Position.Y, 
                        (int) (pendingSprite.SourceRectangle.Width * pendingSprite.Scale.X), 
                        (int) (pendingSprite.SourceRectangle.Height * pendingSprite.Scale.Y)
                        ), Color.Red, 
                        new Vector2(pendingSprite.Origin.X / pendingSprite.SourceRectangle.Width, pendingSprite.Origin.Y / pendingSprite.SourceRectangle.Height));
                }
            }
            NUM_SPRITES=0;

        }

    }

}