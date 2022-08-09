using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.Graphics {

    public enum SpriteAnchor {
        TOP_LEFT,       TOP_CENTER,     TOP_RIGHT,
        CENTER_LEFT,    CENTERED,       CENTER_RIGHT,
        BOTTOM_LEFT,    BOTTOM_CENTER,  BOTTOM_RIGHT
    }

    public class C_Sprite {
        public Texture2D Texture;
        public SpriteAnchor Anchor;
        public bool FlipX;

        readonly Vector2[] ANCHOR_NORMALIZED_VECTORS = {
            new Vector2(0, 0),      new Vector2(.5f, 0),        new Vector2(1.0f, 0),
            new Vector2(0, .5f),      new Vector2(0.5f, 0.5f),        new Vector2(1.0f, 0.5f),
            new Vector2(0, 1.0f),      new Vector2(.5f, 1.0f),        new Vector2(1.0f, 1.0f),
        };

        public C_Sprite(Texture2D texture, SpriteAnchor anchor = SpriteAnchor.CENTERED) {
            Texture = texture;
            Anchor = anchor;
            FlipX = false;
        }

        public static implicit operator Texture2D(C_Sprite sprite) => sprite.Texture;

        public Vector2 GetOrigin() {
            Vector2 res = ANCHOR_NORMALIZED_VECTORS[(int) Anchor];
            res.X *= (float) Texture.Width;
            res.Y *= (float) Texture.Height;
            return res;
        }

    }

}