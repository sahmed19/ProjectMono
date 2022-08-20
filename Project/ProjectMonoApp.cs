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
using MonoGame.Extended.ViewportAdapters;

using Flecs;
using static flecs_hub.flecs;
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
            
            m_IMGUI = new ImGuiRenderer(this);
            m_IMGUI.RebuildFontAtlas();
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

            //Register components
            World.RegisterComponent<C_Position>();
            World.RegisterComponent<C_Rotation>();
            World.RegisterComponent<C_Scale>();

            World.RegisterComponent<C_PendingForces>();
            World.RegisterComponent<C_Velocity>();
            World.RegisterComponent<C_Gravity>();
            World.RegisterComponent<C_TerminalVelocity>();
            World.RegisterComponent<C_AngularVelocity>();
            World.RegisterComponent<C_Friction>();
            World.RegisterComponent<C_Mass>();

            World.RegisterComponent<C_Camera>();
            
            World.RegisterComponent<C_PlatformerData>();
            World.RegisterComponent<C_PlatformerInput>();

            World.RegisterComponent<C_Health>();
            World.RegisterComponent<C_Sprite>();
            
            //Register systems

            World.RegisterSystem(S_Physics.ApplyGravityToPendingForces, EcsOnUpdate,
              $"{typeof(C_PendingForces)}, {typeof(C_Gravity)}");

            World.RegisterSystem(S_Physics.ApplyPendingForcesToVelocity, EcsOnUpdate,
              $"{typeof(C_Velocity)}, {typeof(C_PendingForces)}, ?{typeof(C_Friction)}, ?{typeof(C_Mass)}");

            World.RegisterSystem(S_Physics.ClampVelocityXToTerminalVelocity, EcsOnUpdate,
              $"{typeof(C_Velocity)}, {typeof(C_TerminalVelocity)}");

            World.RegisterSystem(S_Physics.ApplyVelocityToPosition, EcsOnUpdate,
              $"{typeof(C_Position)}, {typeof(C_Velocity)}");
            
            World.RegisterSystem(S_Collision.BounceOffScreenEdge, EcsOnUpdate,
              $"{typeof(C_Position)}, {typeof(C_Velocity)}");
              
            World.RegisterSystem(S_Physics.RotateTowardVelocityDirection, EcsOnUpdate,
              $"{typeof(C_Rotation)}, {typeof(C_Velocity)}");

            World.RegisterSystem(S_Camera.UpdateCameraPosition, EcsOnUpdate,
              $"{typeof(C_Camera)}, {typeof(C_Position)}, ?{typeof(C_Rotation)}");
  
            World.RegisterSystem(S_SpriteRendering.PendSpritesForDraw, EcsPostUpdate, 
              $"{typeof(C_Sprite)}, {typeof(C_Position)}, ?{typeof(C_Rotation)}, ?{typeof(C_Scale)}");
            

            Random random = new Random();

            var pochitaPrefab = World.CreatePrefab("PochitaPrefab");
            

            var camera = World.CreateEntity("Camera");
            camera.Set(new C_Position(){Position=Vector2.Zero});
            camera.Set(new C_Rotation());
            camera.Set(new C_Camera(){Zoom=0.5f});

            for(int i = 0; i < 10000; i++) {
                Entity pochita = World.CreateEntity("Pochita " + i);
                pochita.IsA(pochitaPrefab);

                Vector2 position = new Vector2(
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()),
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()));

                random.NextUnitVector(out var velocity);

                pochita.Set(new C_Position() {Position=position});
                pochita.Set(new C_Scale() {Scale=Vector2.One*.03f});
                pochita.Set(new C_Rotation());
                pochita.Set(new C_PendingForces());
                //pochita.Set(new C_Gravity() {Gravity = Vector2.UnitY * 10.0f});
                pochita.Set(new C_Velocity() {Velocity = velocity});
                pochita.Set(new C_Sprite(1, spriteWidth: 190, spriteHeight: 190));
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Tick(gameTime);
            World.Progress(deltaTime);
            InputManager.LateTick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkTurquoise);
            var transformMatrix = Camera.GetViewMatrix();
            
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                transformMatrix: transformMatrix,
                samplerState: SamplerState.PointWrap);
            m_IMGUI.BeforeLayout(gameTime);
            
            S_SpriteRendering.DrawPendingSprites(this);
            DebuggerManager.GUI_Debugger(this);
            
            SpriteBatch.End();
            m_IMGUI.AfterLayout();
        }


    }

}