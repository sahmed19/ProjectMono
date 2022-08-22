using System;
using System.Numerics;
using Flecs;
using ImGuiNET;
using ProjectMono.Core;

namespace ProjectMono.Debugging
{
    //Entity Browser
    public static partial class MonoDebugger
    {
        static void GUI_Settings(ProjectMonoApp game, ref bool open) {
            ImGui.SetNextWindowSize(new Vector2(500, 440), ImGuiCond.FirstUseEver);
            if(ImGui.Begin("General Settings", ref open))
            {
                if(ImGui.BeginTabBar("General Settings"))
                {
                    if(ImGui.BeginTabItem("Graphics"))
                    {
                        ImGui.Text("Set the display and resolution settings.");

                        //SELECT RESOLUTION
                        if (ImGui.Button("Resolution: " + Settings.CURRENT_RESOLUTION.ToString()))
                            ImGui.OpenPopup("select_resolution");
                            
                        if(ImGui.BeginPopup("select_resolution"))
                        {
                            ImGui.Text("Resolution");
                            ImGui.Separator();
                            for (int i = 0; i < Settings.RESOLUTIONS.Length; i++)
                                if (ImGui.Selectable(Settings.RESOLUTIONS[i].ToString()))
                                {
                                    Settings.GRAPHICS_SETTINGS.ResolutionID = i;
                                }
                            ImGui.EndPopup();
                        }

                        //SELECT SCREEN MODE
                        if (ImGui.Button("Screen Mode: " + Settings.SCREEN_MODE_LABEL))
                            ImGui.OpenPopup("select_screen_mode");
                        
                        if(ImGui.BeginPopup("select_screen_mode"))
                        {
                            ImGui.Text("Screen Mode");
                            ImGui.Separator();
                            for (int i = 0; i < 3; i++)
                                if (ImGui.Selectable(Settings.SCREEN_MODE_LABELS[i])) {
                                    Settings.GRAPHICS_SETTINGS.ScreenMode = (Settings.ScreenMode) i;
                                }
                            ImGui.EndPopup();
                        }

                        ImGui.Separator();

                        //APPLY OR RESET
                        ImGui.BeginDisabled(!Settings.GRAPHICS_SETTINGS_HAVE_BEEN_CHANGED);
                        if(ImGui.Button("Apply"))
                            Settings.ApplyGraphics();
                        ImGui.EndDisabled();

                        ImGui.SameLine();

                        if(ImGui.Button("Reset to Defaults"))
                        {
                            Settings.ResetGraphicsSettings();
                        }
                        ImGui.EndTabItem();
                    }
                    if(ImGui.BeginTabItem("Audio"))
                    {
                        ImGui.Text("Set the volume and mix settings.");
                        
                        ImGui.SliderInt("Master Volume", ref Settings.AUDIO_SETTINGS.MasterVolume, 0, 100);
                        ImGui.SliderInt("Effect Volume", ref Settings.AUDIO_SETTINGS.EffectVolume, 0, 100);
                        ImGui.SliderInt("Musics Volume", ref Settings.AUDIO_SETTINGS.MusicsVolume, 0, 100);
                        ImGui.SliderInt("System Volume", ref Settings.AUDIO_SETTINGS.SystemVolume, 0, 100);

                        if(ImGui.Button("Reset to Defaults")) {
                            Settings.ResetAudioSettings();
                        }
                        ImGui.EndTabItem();
                    }
                    if(ImGui.BeginTabItem("Controls"))
                    {
                        ImGui.Text("Reconfigure the controls of the game.");
                        ImGui.EndTabItem();
                    }
                    if(ImGui.BeginTabItem("Gameplay/Accessibility"))
                    {
                        ImGui.Text("Adjust gameplay values to personal preference.");
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }
                ImGui.End();
            }
        }
    }
}