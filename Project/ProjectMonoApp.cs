using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using ProjectMono.Input;
using ProjectMono.Physics;
using ProjectMono.Graphics;
using ProjectMono.Gameplay;
using ProjectMono.Maps;
using ProjectMono.Debugging;

using MonoGame.Extended.ViewportAdapters;

using Flecs;
using ImGuiNET.XNA;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        ImGuiRenderer m_IMGUI;
        public SpriteBatch SpriteBatch {get; private set; }
        public OrthographicCamera Camera { get; private set; }
        public InputManager InputManager {get; private set; }
        public World World { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager {get; private set; }
        public static int TOTAL_FRAME_COUNT {get; private set;}
        public static ProjectMonoApp INSTANCE;
        public const int BASE_RESOLUTION_WIDTH = 320;
        public const int BASE_RESOLUTION_HEIGHT = 180;
        public float TotalGameTime {get; private set;}
        public string RootDir => Content.RootDirectory;

        //int m_PlayerID;

        Effect effect;

        public ProjectMonoApp()
        {
            INSTANCE=this;
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            InputManager = new InputManager(this);

            Settings.Load(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, BASE_RESOLUTION_WIDTH, BASE_RESOLUTION_HEIGHT);
            Camera = new OrthographicCamera(viewportAdapter);
            Camera.Zoom=0.5f;
            
            PrimitiveDrawer.Initialize(this);
            Debugging.MonoDebugger.Initialize(out m_IMGUI, this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            World = new World(new string[]{""});
            effect = Content.Load<Effect>("graphics/shaders/character");
            TextureDatabase.Initialize();

            //Register Textures
            void RegisterTexture(string name, string path) => TextureDatabase.RegisterTexture(name, Content.Load<Texture2D>(path + name));
            RegisterTexture("spritesheet_player", "graphics/characters/");
            RegisterTexture("icon_pochita", "graphics/characters/");
            RegisterTexture("tileset_twilight_castle","graphics/maps/");

            WorldData.RegisterComponents(this);
            WorldData.RegisterSystems(this);

            
            var camera = World.CreateEntity("Camera");
            camera.Set(new C_Position(){Position=Vector2.Zero});
            camera.Set(new C_Rotation());
            camera.Set(new C_Camera(){Zoom=0.5f});

            //Register tilemap
            S_Tilemap.InitializeMap(this, "test_map", -15, -15);

            Random random = new Random();
            var pochitaPrefab = World.CreatePrefab("PochitaPrefab");

            for(int i = 0; i < 1000; i++) {
                Entity pochita = World.CreateEntity("Pochita " + i);

                Vector2 position = new Vector2(
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()),
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()));

                random.NextUnitVector(out var velocity);

                pochita.Set(new C_PendingForces());
                pochita.Set(new C_Velocity() {Velocity = velocity});
                pochita.Set(new C_Sprite(1, spriteWidth: 190, spriteHeight: 190));
                pochita.Set(new C_Color(){Color=Color.White});
                pochita.Set(new C_SpriteLayer());
                pochita.Set(new C_Position() {Position=position});
                pochita.Set(new C_Scale() {Scale=Vector2.One*.03f});
                pochita.Set(new C_Rotation());
            }
            /*
            player.Attach(new C_Name("Player"));
            player.Attach(new C_Transform2(new Vector2(1,10)));
            player.Attach(new C_Sprite(playerSprite, spriteWidth: 16, spriteHeight: 16, orderInLayer: 50));
            player.Attach(new C_Motion(new Vector2(0, 0), mass: 50));
            player.Attach(new C_PlatformerData());
            player.Attach(new C_PlatformerInput());
            player.Attach(new C_Player());*/
        }

        protected override void Update(GameTime gameTime)
        {
            TOTAL_FRAME_COUNT++;
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            TotalGameTime = (float) gameTime.TotalGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Tick(gameTime);
            World.Progress(deltaTime);
            Debugging.MonoDebugger.GUI_DebuggerUpdate(this, deltaTime);
            InputManager.LateTick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.DarkTurquoise);
            m_IMGUI.BeforeLayout(gameTime);
            Debugging.MonoDebugger.GUI_DebuggerDraw(this, deltaTime);
            var transformMatrix = Camera.GetViewMatrix();
            SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                transformMatrix: transformMatrix,
                samplerState: SamplerState.PointWrap);
            
            S_SpriteRendering.DrawPendingSprites(this);
            SpriteBatch.End();
            m_IMGUI.AfterLayout();
        }
    }
}