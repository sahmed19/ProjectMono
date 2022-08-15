using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using ProjectMono.Gameplay;
using ProjectMono.Input;

namespace ProjectMono.Physics {

    public class S_PlayerController : EntityProcessingSystem
    {
        InputManager m_InputManager;
        ComponentMapper<C_PlatformerInput> m_PlatformerInputMapper;

        public S_PlayerController(InputManager inputManager) : base(Aspect.All(
            typeof(C_PlatformerInput),
            typeof(C_Player)))
        {
            m_InputManager = inputManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            m_PlatformerInputMapper = mapperService.GetMapper<C_PlatformerInput>();
        }

        public override void Process(GameTime gameTime, int entityID)
        {
            float deltaTime = gameTime.DeltaTime();
            
            var platformerInput = m_PlatformerInputMapper.Get(entityID);

            var jumpInput = m_InputManager.GetInputAction("JUMP");
            float horizontalInput = m_InputManager.GetInputAxis("MOVE_HORIZONTAL");

            
            platformerInput.JumpBuffer = jumpInput.WasPressedThisFrame;
            platformerInput.HoldingJump = jumpInput.IsBeingPressed;
            platformerInput.HorizontalInput = horizontalInput;
        }
    }

}