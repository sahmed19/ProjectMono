using System.IO;
using System.Numerics;
using ImGuiNET;
using ProjectMono.Core;

namespace ProjectMono.Debugging
{
    public static partial class MonoDebugger
    {
        static void GUI_ContentView(ProjectMonoApp game, ref bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);

            if(ImGui.Begin("Content View", ref open))
            {
                ProcessDirectory(game.RootDir + "\\bin\\Windows", true);
            }
        }

        static void ProcessDirectory(string targetDirectory, bool first = false)
        {
            string directoryName = new DirectoryInfo(Path.GetDirectoryName(targetDirectory + "\\")).Name;

            if(first || ImGui.TreeNodeEx(directoryName, ImGuiTreeNodeFlags.Framed))
            {
                // Recurse into subdirectories of this directory.
                string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach(string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
                    
                string [] fileEntries = Directory.GetFiles(targetDirectory);
                foreach(string fileName in fileEntries)
                    ProcessFile(fileName);

                ImGui.TreePop();
            }
        }

        static void ProcessFile(string path)
        {
            string fileName = Path.GetFileName(path);
            ImGui.Text(fileName);
        }
    }
}