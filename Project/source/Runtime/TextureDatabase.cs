using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ProjectMono.Debugging;

namespace ProjectMono.Graphics {
    public static class TextureDatabase
    {
        const int MAX_TEXTURES = 256;
        static Texture2D[] m_TextureArray;
        static Dictionary<string, int> m_TextureName2Index;
        static int m_TextureCount=0;

        public static void Initialize()
        {
            m_TextureArray = new Texture2D[MAX_TEXTURES];
            m_TextureName2Index = new Dictionary<string, int>();
            m_TextureCount = 0;
        }

        //exclusive
        public static void UnloadPastIndex(int pastIndex)
        {
            for(int i = pastIndex+1; i < MAX_TEXTURES; i++)
            {
                m_TextureArray = null;
            }

            foreach(var entry in m_TextureName2Index)
            {
                if(entry.Value > pastIndex)
                    m_TextureName2Index.Remove(entry.Key); 
            }
        }

        public static void RegisterTexture(string name, Texture2D texture)
        {
            m_TextureName2Index.Add(name, m_TextureCount);
            m_TextureArray[m_TextureCount]=texture;
            m_TextureCount++;
        }

        public static int GetTextureIndex(string name)
        {
            if(m_TextureName2Index.TryGetValue(name, out var index))
                return index;
            throw new System.Exception("No texture matches the name: " + name);
        }

        public static Texture2D GetTexture(string name)
        {
            DebuggerManager.Print(name);
            return GetTexture(GetTextureIndex(name));
        }

        public static Texture2D GetTexture(int index)
        {
            if(index >= m_TextureCount || index < 0)
                throw new System.Exception("Index " + index + " out of range exception! Make sure texture index is correct.");
            return m_TextureArray[index];
        }

    }
}