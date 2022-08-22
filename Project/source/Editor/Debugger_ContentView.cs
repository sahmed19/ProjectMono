using System.IO;
using System.Numerics;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using ProjectMono.Core;

namespace ProjectMono.Debugging
{
    public static partial class MonoDebugger
    {
        static string DIR = "\\bin\\Windows";
        static string SELECTED_FILE_PATH;
        

        static void GUI_ContentView(ProjectMonoApp game, ref bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(800, 440), ImGuiCond.FirstUseEver);

            if(ImGui.Begin("Content View", ref open))
            {
                if(ImGui.BeginChild("Directory View", new Vector2(ImGui.GetWindowWidth(), 300), true))
                {
                    ProcessDirectory(game.RootDir + "\\bin\\Windows", game, true);
                    ImGui.EndChild();
                }

                if(ImGui.BeginChild("File Preview"))
                {
                    if(File.Exists(SELECTED_FILE_PATH))
                    {
                        string contentFile = SELECTED_FILE_PATH
                            .Substring(game.RootDir.Length + DIR.Length + 1);
                        contentFile = contentFile.Split('.', 2, System.StringSplitOptions.RemoveEmptyEntries)[0];

                        string fileName = Path.GetFileName(SELECTED_FILE_PATH);
                        if(fileName.EndsWith(".xnb"))
                        {
                            try
                            {
                                var monoTexture = game.Content.Load<Texture2D>(contentFile);
                                var imguiTexture = Renderer.BindTexture(monoTexture);
                                ImGui.Image(imguiTexture, new Vector2(monoTexture.Width, monoTexture.Height));
                            }
                            catch
                            {
                                throw new System.Exception("No content at " + contentFile);
                            }
                            
                        }
                        else if(fileName.EndsWith(".tmx"))
                        {
                            foreach (string line in System.IO.File.ReadLines(SELECTED_FILE_PATH))
                            {  
                                ImGui.TextUnformatted(line);
                            }  
                        }
                    }
                }

            }
        }

        static void ProcessDirectory(string targetDirectory, ProjectMonoApp game, bool first = false)
        {
            string directoryName = new DirectoryInfo(Path.GetDirectoryName(targetDirectory + "\\")).Name;

            if(first || ImGui.TreeNodeEx(directoryName, ImGuiTreeNodeFlags.Framed))
            {
                // Recurse into subdirectories of this directory.
                string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach(string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, game);
                    
                string [] fileEntries = Directory.GetFiles(targetDirectory);
                foreach(string fileName in fileEntries)
                    ProcessFile(fileName, game);

                ImGui.TreePop();
            }
        }

        static void ProcessFile(string path, ProjectMonoApp game)
        {
            string fileName = Path.GetFileName(path);
            if(ImGui.Selectable(fileName))
            {
                SELECTED_FILE_PATH = path;
            }
        }
    }
}