using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using ProjectMono.Debugging;
using Flecs;

namespace ProjectMono.Graphics {

    public enum SpriteAnchor {
        TOP_LEFT,       TOP_CENTER,     TOP_RIGHT,
        CENTER_LEFT,    CENTERED,       CENTER_RIGHT,
        BOTTOM_LEFT,    BOTTOM_CENTER,  BOTTOM_RIGHT
    }
    public enum SpriteLayer {
        UI,
        FOREGROUND,
        CHARACTER,
        TILEMAP,
        BACKDROP_NEAR,
        BACKDROP_FAR
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_SpriteLayer : IComponent
    {
        static readonly Vector2[] ANCHOR_NORMALIZED_VECTORS = {
            new Vector2(0, 0),      new Vector2(.5f, 0),        new Vector2(1.0f, 0),
            new Vector2(0, .5f),      new Vector2(0.5f, 0.5f),        new Vector2(1.0f, 0.5f),
            new Vector2(0, 1.0f),      new Vector2(.5f, 1.0f),        new Vector2(1.0f, 1.0f),
        };

        public C_SpriteLayer()
        {
            Anchor=SpriteAnchor.CENTERED;
            Layer=SpriteLayer.CHARACTER;
            Depth=0;
        }

        public SpriteAnchor Anchor;
        public SpriteLayer Layer;
        public float Depth;
        public Vector2 GetOrigin(int spriteWidth, int spriteHeight)
        {
            Vector2 res = ANCHOR_NORMALIZED_VECTORS[(int) Anchor];
            res.X *= (float) spriteWidth;
            res.Y *= (float) spriteHeight;
            return res;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C_Sprite : IComponent
    {
        public Texture2D Texture => TextureDatabase.GetTexture(TextureIndex);
        public int TextureIndex;
        public Rectangle Rectangle = new Rectangle(0, 0, 16, 16);
        public int Frame;
        public bool FlipX;
        public int SpriteWidth, SpriteHeight;
        int CellCountX => Texture.Width / SpriteWidth;
        int CellCountY => Texture.Height / SpriteHeight;
        int MaxFrame => CellCountX * CellCountY - 1;
        
        public C_Sprite(string textureName, int spriteWidth=0, int spriteHeight=0)
        : this(TextureDatabase.GetTextureIndex(textureName), spriteWidth, spriteHeight)
        {

        }
        public C_Sprite(int textureIndex, int spriteWidth=0, int spriteHeight=0)
        {
            TextureIndex = textureIndex;
            FlipX = false;
            Frame=0;
            SpriteWidth=0;
            SpriteHeight=0;
            SpriteWidth = spriteWidth==0? Texture.Width : spriteWidth;
            SpriteHeight = spriteHeight==0? Texture.Height : spriteHeight;
            SetRectangle(Frame);
        }

        public C_Sprite SetRectangle(Rectangle r) {
            Rectangle = r;
            return this;
        }

        public C_Sprite SetRectangle(int frame = -1) {
            if(frame > -1)
                Frame = frame;
            if(MaxFrame>0)
                Frame = Frame % MaxFrame;

            int cellX = Frame % CellCountX;
            int cellY = (int)((Frame*1.0f) / (CellCountX*1.0f));

            int x = cellX * SpriteWidth;
            int y = cellY * SpriteHeight;
            Rectangle = new Rectangle(x, y, SpriteWidth, SpriteHeight);
            return this;
        }

    }

}