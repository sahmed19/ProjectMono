
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.ECS.Components {

    public class C_Sprite {
        public Texture2D Sprite { get; private set; }
        public C_Sprite(Texture2D sprite) {
            Sprite = sprite;
        }
    }

}