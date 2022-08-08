using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Gameplay;
using ProjectMono.Input;

namespace ProjectMono.Physics {

    public class S_PlayerInput : EntityProcessingSystem
    {
        InputManager m_InputManager;
        ComponentMapper<C_Player> m_PlayerMapper;
        ComponentMapper<C_Motion> m_MotionMapper;
        ComponentMapper<C_Transform2> m_TransformMapper;

        public S_PlayerInput(InputManager inputManager) : base(Aspect.All(
            typeof(C_Player),
            typeof(C_Motion),
            typeof(C_Transform2)))
        {
            m_InputManager = inputManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_PlayerMapper = mapperService.GetMapper<C_Player>();
            m_MotionMapper = mapperService.GetMapper<C_Motion>();
            m_TransformMapper = mapperService.GetMapper<C_Transform2>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            C_Player player = m_PlayerMapper.Get(entityID);
            C_Motion motion = m_MotionMapper.Get(entityID);
            C_Transform2 transform = m_TransformMapper.Get(entityID);

            float horizontalInput = m_InputManager.GetInputAxis("MOVE_HORIZONTAL");

            motion.TerminalVelocity = C_Player.TOP_SPEED;
            motion.PendingForces.X += horizontalInput * C_Player.ACCELERATION * deltaTime;

            if(transform.Position.Y <= 150.0f && m_InputManager.GetInputAction("JUMP").WasPressedThisFrame) {
                motion.PendingForces.Y += C_Player.JUMP_FORCE;
                DebuggerManager.Print("Got here");
            }

        }
    }

}