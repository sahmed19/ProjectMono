﻿using System;
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
using ImGuiNET.XNA;

namespace ProjectMono.Core {

    public class ProjectMonoApp : Game
    {
        SpriteBatch m_SpriteBatch;
        ImGuiRenderer m_IMGUI;
        public OrthographicCamera Camera { get; private set; }
        public InputManager InputManager {get; private set; }
        public World World { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager {get; private set; }
        

        public static int TOTAL_FRAME_COUNT {get; private set;}

        //int m_PlayerID;

        Effect effect;

        public ProjectMonoApp()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            InputManager = new InputManager(this);
            DebuggerManager.Initialize();

            Settings.Load(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 320, 180);
            Camera = new OrthographicCamera(viewportAdapter);
            Camera.Move(new Vector2(-200.0f, 200.0f));
            
            m_IMGUI = new ImGuiRenderer(this);
            m_IMGUI.RebuildFontAtlas();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            World = new World(new string[]{""});

            //Register components
            World.RegisterComponent<C_Camera>();
            World.RegisterComponent<C_Motion>();
            World.RegisterComponent<C_PlatformerData>();
            World.RegisterComponent<C_PlatformerInput>();
            World.RegisterComponent<C_Player>();
            World.RegisterComponent<C_Sprite>();
            World.RegisterComponent<C_Transform2>();

            var playerSprite = Content.Load<Texture2D>("graphics/characters/spritesheet_player");
            var pochitaSprite = Content.Load<Texture2D>("graphics/characters/pochita_icon");
            effect = Content.Load<Effect>("graphics/shaders/character");

            //var player = World.CreateEntity("Player");
            //var camera = World.CreateEntity("Camera");

            Random random = new Random();

            for(int i = 0; i < 100; i++) {
                Entity pochita = World.CreateEntity("Pochita");
                //pochita.Attach(new C_Name("Pochita " + i));

                Vector2 position = new Vector2(
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()),
                    MathHelper.Lerp(-10.0f, 10.0f, (float) random.NextDouble()));

                random.NextUnitVector(out var velocity);
                /*
                pochita.Attach(new C_Transform2(position, 0.0f, Vector2.One * .1f));
                pochita.Attach(new C_Motion(velocity));
                pochita.Attach(new C_Sprite(pochitaSprite, spriteWidth: 190, spriteHeight: 190));*/
            }
            /*
            player.Attach(new C_Name("Player"));
            player.Attach(new C_Transform2(new Vector2(1,10)));
            player.Attach(new C_Sprite(playerSprite, spriteWidth: 16, spriteHeight: 16, orderInLayer: 50));
            player.Attach(new C_Motion(new Vector2(0, 0), mass: 50));
            player.Attach(new C_PlatformerData());
            player.Attach(new C_PlatformerInput());
            player.Attach(new C_Player());

            camera.Attach(new C_Name("Camera"));
            camera.Attach(new C_Transform2());
            camera.Attach(new C_Camera());*/
        }

        protected override void Update(GameTime gameTime)
        {
            TOTAL_FRAME_COUNT++;
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Tick(gameTime);
            //World.Update(gameTime);
            InputManager.LateTick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);
            var transformMatrix = Camera.GetViewMatrix();
            
            m_SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                transformMatrix: transformMatrix,
                samplerState: SamplerState.PointWrap);
            m_IMGUI.BeforeLayout(gameTime);
            
            //World.Draw(gameTime);
            base.Draw(gameTime);
            
            DebuggerManager.GUI_Debugger(this);
            
            m_SpriteBatch.End();
            m_IMGUI.AfterLayout();
        }
    }

}