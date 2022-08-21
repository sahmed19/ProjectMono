using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMono.Core;

namespace ProjectMono.Graphics {

    public static class PrimitiveDrawer
    {
        static Texture2D WhitePixel;
        static Color Clear;

        public static void Initialize(ProjectMonoApp game)
        {
            WhitePixel = new Texture2D(game.GraphicsDevice, 1, 1);
            WhitePixel.SetData(new Color[] {Color.White});
            Clear = new Color(1,1,1,0);
        }

        public static void DrawPulsingOutline(ProjectMonoApp game, Rectangle rect, Color color, Vector2 origin, float pulseSpeed=10.0f)
        {
            float sin = (float) Math.Sin(game.TotalGameTime * pulseSpeed);
            float sinFactor = (sin+1.0f) * 0.2f;

            Color lerpedCol = Color.Lerp(color, Clear, sinFactor);

            
            rect.X -= (int) (origin.X * rect.Width);
            rect.Y -= (int) (origin.Y * rect.Height);

            var vec2zero = Vector2.Zero;
            game.SpriteBatch.Draw(WhitePixel, new Rectangle(rect.Left, rect.Top, rect.Width, 1),  //top
            null, lerpedCol, 0.0f, vec2zero, SpriteEffects.None, 0.0f);
            game.SpriteBatch.Draw(WhitePixel, new Rectangle(rect.Right, rect.Top, 1, rect.Height), //right
            null, lerpedCol, 0.0f, vec2zero, SpriteEffects.None, 0.0f);
            game.SpriteBatch.Draw(WhitePixel, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), //bottom
            null, lerpedCol, 0.0f, vec2zero, SpriteEffects.None, 0.0f);
            game.SpriteBatch.Draw(WhitePixel, new Rectangle(rect.Left, rect.Top, 1, rect.Height), //left
            null, lerpedCol, 0.0f, vec2zero, SpriteEffects.None, 0.0f);
        }
    }



}