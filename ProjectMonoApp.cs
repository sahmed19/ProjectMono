using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;

using ProjectMono.Input;
using ProjectMono.Physics;
using ProjectMono.Graphics;
using ProjectMono.Gameplay;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        InputManager m_InputManager;
        World m_World;
        GraphicsDeviceManager m_Graphics;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_World = new WorldBuilder()
                .AddSystem(new S_PlayerInput(m_InputManager))
                .AddSystem(new S_MotionPhysics())
                .AddSystem(new S_SpriteRendering(GraphicsDevice))
                .Build();

            var pochitaSprite = Content.Load<Texture2D>("Sprites/CharacterProfiles/pochita_def");
            Entity pochita = m_World.CreateEntity();

            m_PochitaID = pochita.Id;

            pochita.Attach(new C_Transform2(new Vector2(200,100)));
            pochita.Attach(new C_Sprite(pochitaSprite));
            pochita.Attach(new C_Motion(new Vector2(0, 0), 0).WithFriction(C_Player.FRICTION));
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
            m_InputManager.LateTick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);
            m_World.Draw(gameTime);
            base.Draw(gameTime);
        }
    }

}