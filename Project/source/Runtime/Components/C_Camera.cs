using Microsoft.Xna.Framework;
using ImGuiNET;

namespace ProjectMono.Core {
    public struct C_Camera : IGUIDrawable
    {
        public float Zoom;
        public float TargetAdaptionTime;
        public Vector2 TargetAdaptionVelocity;
        public bool FollowTarget;

        public C_Camera(float zoom = 1.0f, float targetAdaptionTime = 0.5f, bool followTarget = true)
        {
            Zoom = zoom;
            TargetAdaptionTime = targetAdaptionTime;
            TargetAdaptionVelocity = Vector2.Zero;
            FollowTarget = followTarget;
        }

        public string Label => "Camera";

        public void GUI_Draw()
        {
            ImGui.DragFloat("Target Adaption Time", ref TargetAdaptionTime, 0.1f, 0.1f, 3f);
            ImGui.DragFloat("Zoom", ref Zoom, 0.001f, 0.01f, 2f);
            ImGui.Checkbox("Follow Target", ref FollowTarget);
        }
    }

}