using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectMono.Input;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        InputManager m_InputManager;
        GraphicsDeviceManager m_Graphics;
        SpriteBatch m_SpriteBatch;
        Texture2D m_PochitaSprite;
        Vector2 m_PochitaPosition = Vector2.One;

        readonly int HORIZONTAL_HASH = string.GetHashCode("MOVE_HORIZONTAL");
        readonly int VERTICAL_HASH = string.GetHashCode("MOVE_VERTICAL");

        bool facingLeft;

        public ProjectMonoApp()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            m_InputManager = new InputManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            DebuggerManager.Print("Project Initialized!");
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            DebuggerManager.Print("Content Loaded!");

            m_PochitaSprite = Content.Load<Texture2D>("Sprites/CharacterProfiles/pochita_def");
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            m_InputManager.Tick(gameTime);

            float speed = 250f;

            Vector2 movementInput = new Vector2(m_InputManager.GetInputAxis(HORIZONTAL_HASH), m_InputManager.GetInputAxis(VERTICAL_HASH));

            m_PochitaPosition.X += speed * deltaTime * movementInput.X;
            m_PochitaPosition.Y -= speed * deltaTime * movementInput.Y;

            if(Math.Abs(movementInput.X) > 0)
                facingLeft = movementInput.X < 0;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(
                m_PochitaSprite,
                m_PochitaPosition,
                null,
                Color.White,
                0.0f,
                new Vector2(m_PochitaSprite.Width/2, m_PochitaSprite.Height/2),
                Vector2.One,
                facingLeft? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f);
            m_SpriteBatch.End();


            base.Draw(gameTime);
        }
    }

}