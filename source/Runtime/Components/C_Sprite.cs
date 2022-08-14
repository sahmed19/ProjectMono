using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMono.Graphics {

    public enum SpriteAnchor {
        TOP_LEFT,       TOP_CENTER,     TOP_RIGHT,
        CENTER_LEFT,    CENTERED,       CENTER_RIGHT,
        BOTTOM_LEFT,    BOTTOM_CENTER,  BOTTOM_RIGHT
    }

    public class C_Sprite : IGUIDrawable {
        public Texture2D Texture;
        public Rectangle Rectangle = new Rectangle(0, 0, 16, 16);
        public SpriteAnchor Anchor;
        public bool FlipX;

        int m_SpriteWidth, m_SpriteHeight, m_Frame;
        int TotalImageWidth => Texture.Width;
        int TotalImageHeight => Texture.Height;
        int CellCountX => TotalImageWidth / m_SpriteWidth;
        int CellCountY => TotalImageHeight / m_SpriteHeight;
        int MaxFrame => CellCountX * CellCountY - 1;

        readonly Vector2[] ANCHOR_NORMALIZED_VECTORS = {
            new Vector2(0, 0),      new Vector2(.5f, 0),        new Vector2(1.0f, 0),
            new Vector2(0, .5f),      new Vector2(0.5f, 0.5f),        new Vector2(1.0f, 0.5f),
            new Vector2(0, 1.0f),      new Vector2(.5f, 1.0f),        new Vector2(1.0f, 1.0f),
        };

        public C_Sprite(Texture2D texture, int spriteWidth=0, int spriteHeight=0, int frame = 0, SpriteAnchor anchor = SpriteAnchor.CENTERED)
        {
            Texture = texture;
            Anchor = anchor;
            FlipX = false;
            m_SpriteWidth = spriteWidth==0? TotalImageWidth : spriteWidth;
            m_SpriteHeight = spriteHeight==0? TotalImageHeight : spriteHeight;
            m_Frame = frame;

            DebuggerManager.Print("CellX: " + CellCountX);
            DebuggerManager.Print("CellY: " + CellCountY);
        }

        public static implicit operator Texture2D(C_Sprite sprite) => sprite.Texture;

        public Vector2 GetOrigin()
        {
            Vector2 res = ANCHOR_NORMALIZED_VECTORS[(int) Anchor];
            res.X *= (float) Texture.Width;
            res.Y *= (float) Texture.Height;
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
            ImGui.Text("Source: " + Texture.Name);
            ImGui.Text("Tex Dimensions: " + TotalImageWidth + "px X " + TotalImageHeight + "px");
            ImGui.Text("Cell Dimensions: " + CellCountX + " X " + CellCountY);
            ImGui.Text("Max Frame: " + MaxFrame);

            ImGui.Separator();
            
            if(ImGui.DragInt2("Sprite Dimensions", ref m_SpriteWidth, 1, 1, TotalImageWidth) ||
               ImGui.SliderInt("Current Frame", ref m_Frame, 0, MaxFrame))
                RecalculateRectangle();

            ImGui.Checkbox("FlipX", ref FlipX);
            
        }

    }

}