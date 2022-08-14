using Microsoft.Xna.Framework;
using ImGuiNET;

namespace ProjectMono.Core {
    public class C_Camera : IGUIDrawable
    {
        public int ID_CameraTarget;
        public float Zoom;
        public float TargetAdaptionTime;
        public Vector2 TargetAdaptionVelocity;
        public bool FollowTarget;

        public C_Camera()
        {
            Zoom = 1.0f;
            TargetAdaptionTime = .3f;
            FollowTarget = true;
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