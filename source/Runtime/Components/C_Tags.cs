using System.Collections.Generic;
using ImGuiNET;

class C_Tags : IGUIDrawable
{
    public HashSet<string> Tags;
    
    public C_Tags(params string[] tags) {
        Tags = new HashSet<string>();
        AddTags(tags);
    }


    public void AddTags(params string[] tags) {
        foreach (var item in tags)
        {
            Tags.Add(item);
        }
    }

    public bool HasTag(string tag) {
        return Tags.Contains(tag);
    }
    
    
    public bool HasTags(params string[] tags) {
        foreach (var item in tags)
        {
            if(!HasTag(item))
                return false;
        }
        return true;
    }

    
    public string Label => "Tags";
    public void GUI_Draw()
    {
        foreach(var tag in Tags) {
            ImGui.BulletText(tag);
        }
    }


}