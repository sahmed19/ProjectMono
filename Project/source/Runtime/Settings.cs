using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using ProjectMono.Core;

public static class Settings
{
#region Graphics
    public readonly static Resolution[] RESOLUTIONS = {
        new Resolution(320, 180),
        new Resolution(640, 360),
        new Resolution(1024, 576),
        new Resolution(1280, 720),
        new Resolution(1600, 900),
        new Resolution(1920, 1080),
        new Resolution(2560, 1440),
        new Resolution(3840, 2160)
    };
    public readonly static string[] SCREEN_MODE_LABELS = {
        "Fullscreen",
        "Windowed",
        "Borderless Windowed"
    };
    public static GraphicsSettings DEFAULT_GRAPHICS_SETTINGS = new GraphicsSettings(){
        ResolutionID=3,
        ScreenMode=ScreenMode.WINDOWED
    };
    public static GraphicsSettings GRAPHICS_SETTINGS;
    static GraphicsSettings m_LastAppliedGraphicsSettings;
    public static Resolution CURRENT_RESOLUTION => RESOLUTIONS[GRAPHICS_SETTINGS.ResolutionID];
    public static bool IS_BORDERLESS => GRAPHICS_SETTINGS.ScreenMode == ScreenMode.WINDOWED_BORDERLESS;
    public static bool IS_FULLSCREEN => GRAPHICS_SETTINGS.ScreenMode == ScreenMode.FULLSCREEN;
    public static bool GRAPHICS_SETTINGS_HAVE_BEEN_CHANGED => !GRAPHICS_SETTINGS.Equals(m_LastAppliedGraphicsSettings);
    public static string SCREEN_MODE_LABEL => SCREEN_MODE_LABELS[(int) GRAPHICS_SETTINGS.ScreenMode];
#endregion
#region Audio
    public static AudioSettings DEFAULT_AUDIO_SETTINGS = new AudioSettings(){
        MasterVolume=100,
        MusicsVolume=100,
        EffectVolume=100,
        SystemVolume=100
    };
    public static AudioSettings AUDIO_SETTINGS;

#endregion

    
    static ProjectMonoApp m_Game;
    public static void ResetGraphicsSettings() {
        GRAPHICS_SETTINGS=DEFAULT_GRAPHICS_SETTINGS;
    }
    public static void ResetAudioSettings() {
        AUDIO_SETTINGS=DEFAULT_AUDIO_SETTINGS;
    }
    public static void Load(ProjectMonoApp game) {
        ResetGraphicsSettings();
        ResetAudioSettings();
        m_Game = game;
        ApplyGraphics();
    }
    public static void ApplyGraphics() {
        m_LastAppliedGraphicsSettings = GRAPHICS_SETTINGS;
        m_Game.Window.IsBorderless = IS_BORDERLESS;
        m_Game.GraphicsDeviceManager.IsFullScreen = IS_FULLSCREEN;

        m_Game.GraphicsDeviceManager.PreferredBackBufferWidth=CURRENT_RESOLUTION.Width;
        m_Game.GraphicsDeviceManager.PreferredBackBufferHeight=CURRENT_RESOLUTION.Height;
        m_Game.GraphicsDeviceManager.ApplyChanges();
    }

    public struct AudioSettings {
        public int MasterVolume;
        public int MusicsVolume;
        public int EffectVolume;
        public int SystemVolume;
    }
    
    public struct GraphicsSettings {
        public int ResolutionID;
        public ScreenMode ScreenMode;
    }

    public enum ScreenMode {
        FULLSCREEN,
        WINDOWED,
        WINDOWED_BORDERLESS
    }

    public struct Resolution {
        public int Width;
        public int Height;
        public Resolution(int w, int h) {
            Width = w;
            Height = h;
        }
        public override string ToString() => Width + " X " + Height;
        
    }

}
