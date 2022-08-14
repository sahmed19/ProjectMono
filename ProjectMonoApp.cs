using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;
using MonoGame.Extended;

using ProjectMono.Input;
using ProjectMono.Physics;
using ProjectMono.Graphics;
using ProjectMono.Gameplay;
using MonoGame.Extended.ViewportAdapters;

using ImGuiNET.XNA;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        
        SpriteBatch m_SpriteBatch;
        ImGuiRenderer m_IMGUI;
        public OrthographicCamera Camera { get; private set; }
        public InputManager InputManager {get; private set; }
        public World World { get; private set; }
        public GraphicsDeviceManager Graphics {get; private set; }
        

        public static int TOTAL_FRAME_COUNT {get; private set;}

        int m_PochitaID;

        public ProjectMonoApp()
        {
            Graphics = new GraphicsDeviceManager(this);
            InputManager = new InputManager(this);

            

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 320, 240);
            Camera = new OrthographicCamera(viewportAdapter);
            Camera.Move(new Vector2(-200.0f, 200.0f));
            
            m_IMGUI = new ImGuiRenderer(this);
            m_IMGUI.RebuildFontAtlas();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            World = new WorldBuilder()
                .AddSystem(new S_PlayerController(InputManager))
                .AddSystem(new S_Platformer())
                .AddSystem(new S_MotionPhysics())
                .AddSystem(new S_CollisionPhysics())
                .AddSystem(new S_CameraTargeting())
                .AddSystem(new S_CameraSystem(Camera))
                .AddSystem(new S_SpriteRendering(m_SpriteBatch))
                .Build();

            var pochitaSprite = Content.Load<Texture2D>("graphics/characters/spritesheet_player");
            Entity pochita = World.CreateEntity();
            Entity camera = World.CreateEntity();

            m_PochitaID = pochita.Id;

            pochita.Attach(new C_Transform2(new Vector2(10,100)));
            pochita.Attach(new C_Sprite(pochitaSprite, 16, 16));
            pochita.Attach(new C_Motion(new Vector2(0, 0)));
            pochita.Attach(new C_PlatformerData());
            pochita.Attach(new C_PlatformerInput());
            pochita.Attach(new C_Player());

            camera.Attach(new C_Transform2());
            camera.Attach(new C_Camera());
        }

        protected override void Update(GameTime gameTime)
        {
            TOTAL_FRAME_COUNT++;
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Tick(gameTime);
            World.Update(gameTime);
            InputManager.LateTick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);
            var transformMatrix = Camera.GetViewMatrix();
            
            m_SpriteBatch.Begin(
                transformMatrix: transformMatrix,
                samplerState: SamplerState.PointClamp);
            m_IMGUI.BeforeLayout(gameTime);
            
            World.Draw(gameTime);
            base.Draw(gameTime);
            
            DebuggerManager.GUI_Debugger(this);
            
            m_SpriteBatch.End();
            m_IMGUI.AfterLayout();
        }
    }

}