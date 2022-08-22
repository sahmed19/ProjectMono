
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Flecs;

namespace ProjectMono.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C_Color : IComponent
    {
        public Color Color;
    }
}