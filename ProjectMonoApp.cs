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
using ImGuiNET;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        InputManager m_InputManager;
        World m_World;
        GraphicsDeviceManager m_Graphics;
        SpriteBatch m_SpriteBatch;
        OrthographicCamera m_Camera;

        ImGuiRenderer m_IMGUI;
        

        public static int TOTAL_FRAME_COUNT {get; private set;}

        int m_PochitaID;

        public ProjectMonoApp()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            m_InputManager = new InputManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 320, 240);
            m_Camera = new OrthographicCamera(viewportAdapter);
            m_Camera.Zoom = .5f;
            m_Camera.Move(Vector2.UnitY * 300);
            
            m_IMGUI = new ImGuiRenderer(this);
            m_IMGUI.RebuildFontAtlas();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            m_World = new WorldBuilder()
                .AddSystem(new S_PlayerController(m_InputManager))
                .AddSystem(new S_Platformer())
                .AddSystem(new S_MotionPhysics())
                .AddSystem(new S_CollisionPhysics())
                .AddSystem(new S_SpriteRendering(m_SpriteBatch))
                .Build();

            var pochitaSprite = Content.Load<Texture2D>("graphics/characters/spritesheet_player");
            Entity pochita = m_World.CreateEntity();

            m_PochitaID = pochita.Id;

            pochita.Attach(new C_Transform2(new Vector2(10,100)));
            pochita.Attach(new C_Sprite(pochitaSprite));
            pochita.Attach(new C_Motion(new Vector2(0, 0)));
            pochita.Attach(new C_PlatformerData());
            pochita.Attach(new C_PlatformerInput());
            pochita.Attach(new C_Player());
        }

        protected override void Update(GameTime gameTime)
        {
            TOTAL_FRAME_COUNT++;
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            m_InputManager.Tick(gameTime);
            m_World.Update(gameTime);
            
            if(m_InputManager.GetInputAction("PREV_WEAPON").WasPressedThisFrame) {
                m_Camera.ZoomOut(.1f);
            } else if(m_InputManager.GetInputAction("NEXT_WEAPON").WasPressedThisFrame) {
                m_Camera.ZoomIn(.1f);
            }

            m_InputManager.LateTick();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);
            var transformMatrix = m_Camera.GetViewMatrix();
            
            m_SpriteBatch.Begin(transformMatrix: transformMatrix);
            m_IMGUI.BeforeLayout(gameTime);
            
            m_World.Draw(gameTime);
            base.Draw(gameTime);
            GUI();

            
            m_SpriteBatch.End();
            m_IMGUI.AfterLayout();
        }


        void GUI() {
            
        }
    }

}