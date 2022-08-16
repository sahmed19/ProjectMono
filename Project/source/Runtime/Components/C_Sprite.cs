using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
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
    public struct C_Sprite : IComponent, IGUIDrawable {
        public Texture2D Texture => TextureDatabase.GetTexture(TextureIndex);
        public int TextureIndex;
        public Rectangle Rectangle = new Rectangle(0, 0, 16, 16);
        public SpriteAnchor Anchor;
        public SpriteLayer Layer;
        public int OrderInLayer {
            get { return m_orderInLayer; }
            set { m_orderInLayer = Math.Clamp(value, -9999, 9999); }
        }
        public int RenderOrder => ((int) Layer * 10000) + OrderInLayer;
        public bool FlipX;
        int m_orderInLayer;
        int m_SpriteWidth, m_SpriteHeight, m_Frame;
        int TotalImageWidth => Texture.Width;
        int TotalImageHeight => Texture.Height;
        int CellCountX => TotalImageWidth / m_SpriteWidth;
        int CellCountY => TotalImageHeight / m_SpriteHeight;
        int MaxFrame => CellCountX * CellCountY - 1;

        static readonly Vector2[] ANCHOR_NORMALIZED_VECTORS = {
            new Vector2(0, 0),      new Vector2(.5f, 0),        new Vector2(1.0f, 0),
            new Vector2(0, .5f),      new Vector2(0.5f, 0.5f),        new Vector2(1.0f, 0.5f),
            new Vector2(0, 1.0f),      new Vector2(.5f, 1.0f),        new Vector2(1.0f, 1.0f),
        };

        public C_Sprite(string textureName, int spriteWidth=0, int spriteHeight=0, int frame = 0, SpriteAnchor anchor = SpriteAnchor.CENTERED, SpriteLayer layer = SpriteLayer.CHARACTER, int orderInLayer = 0) :
        this(TextureDatabase.GetTextureIndex(textureName), spriteWidth, spriteHeight, frame, anchor, layer, orderInLayer) 
        {
            DebuggerManager.Print("Constructing! " + textureName + " has index of " + TextureIndex);
        }
        
        public C_Sprite(int textureIndex=0, int spriteWidth=0, int spriteHeight=0, int frame = 0, SpriteAnchor anchor = SpriteAnchor.CENTERED, SpriteLayer layer = SpriteLayer.CHARACTER, int orderInLayer = 0)
        {
            TextureIndex = textureIndex;
            Anchor = anchor;
            FlipX = false;
            Layer = layer;
            m_orderInLayer=orderInLayer;
            m_Frame = frame;

            m_SpriteWidth=0;
            m_SpriteHeight=0;
            m_SpriteWidth = spriteWidth==0? Texture.Width : spriteWidth;
            m_SpriteHeight = spriteHeight==0? Texture.Height : spriteHeight;
            RecalculateRectangle();
        }


        public static implicit operator Texture2D(C_Sprite sprite) => sprite.Texture;

        public Vector2 GetOrigin()
        {
            Vector2 res = ANCHOR_NORMALIZED_VECTORS[(int) Anchor];
            res.X *= (float) m_SpriteWidth;
            res.Y *= (float) m_SpriteHeight;
            return res;
        }
        void RecalculateRectangle() {
            int cellX = m_Frame % CellCountX;
            int cellY = (int)((m_Frame*1.0f) / (CellCountX*1.0f));

            int x = cellX * m_SpriteWidth;
            int y = cellY * m_SpriteHeight;
            Rectangle = new Rectangle(x, y, m_SpriteWidth, m_SpriteHeight);
        }

        public void SetFrame(int frame)
        {
            m_Frame = frame % MaxFrame;
            RecalculateRectangle();
        }
        public void IncrementFrame(int amt) => SetFrame(m_Frame + amt);


        public string Label => "Sprite";
        public void GUI_Draw() {
            ImGui.Text("## Texture Info");
            ImGui.Text("Source: " + Texture.Name);
            ImGui.Text("Tex Dimensions: " + TotalImageWidth + "px X " + TotalImageHeight + "px");
            ImGui.Text("Cell Dimensions: " + CellCountX + " X " + CellCountY);
            ImGui.Text("Max Frame: " + MaxFrame);

            ImGui.Separator();
            
            if(ImGui.DragInt2("Sprite Dimensions", ref m_SpriteWidth, 1, 1, TotalImageWidth) ||
               ImGui.SliderInt("Current Frame", ref m_Frame, 0, MaxFrame))
                RecalculateRectangle();

            ImGui.Checkbox("FlipX", ref FlipX);

            ImGui.Separator();

            ImGui.Text("## Sprite Layering");
            //SELECT Sprite Layer
            if (ImGui.Button("Sprite Layer: " + Layer.ToString()))
                ImGui.OpenPopup("sprite_layer");
                
            if(ImGui.BeginPopup("sprite_layer"))
            {
                ImGui.Text("Sprite Layer");
                ImGui.Separator();
                for (int i = 0; i < 6; i++)
                    if (ImGui.Selectable(((SpriteLayer) i).ToString())) {
                        Layer = (SpriteLayer) i;
                    }
                ImGui.EndPopup();
            }

            ImGui.DragInt("Sprite Order in Layer", ref m_orderInLayer, 10, -999, 999);
            
        }

    }

}