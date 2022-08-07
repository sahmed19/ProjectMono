using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace ProjectMono.Input {

    public class InputAction
    {
//public
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ID { get; private set; }
        public bool WasPressedThisFrame => !m_wasBeingPressedLastFrame && m_isBeingPressed;
        public bool WasReleasedThisFrame => m_wasBeingPressedLastFrame && !m_isBeingPressed;
        public bool IsBeingPressed => m_isBeingPressed;
        public float PressStateElapsedTime => m_pressStateElapsedTime;
        public event Action E_OnPressed = null;
        public event Action E_OnReleased = null;
//private
        InputManager m_ParentInputManager;
        Keys m_PrimaryBinding, m_SecondaryBinding;
        Buttons m_PrimaryGamepadBinding, m_SecondaryGamepadBinding;
        bool m_isBeingPressed, m_wasBeingPressedLastFrame;
        float m_pressStateElapsedTime=0, m_timeOfLastPress=0.0f, m_timeOfLastRelease=0.0f;

//constructors
        public InputAction(InputManager inputManager, string name, string description, Keys primaryBinding) : 
            this(inputManager, name, description, primaryBinding, Keys.None, Buttons.None, Buttons.None) {}
        public InputAction(InputManager inputManager, string name, string description, Keys primaryBinding, Buttons primaryGamepadBinding) : 
            this(inputManager, name, description, primaryBinding, Keys.None, primaryGamepadBinding, Buttons.None) {}
        public InputAction(InputManager inputManager, string name, string description,  Keys primaryBinding, Keys secondaryBinding, Buttons primaryGamepadBinding, Buttons secondaryGamepadBinding)
        {
            m_ParentInputManager = inputManager;
            Name = name;
            Description = description;
            ID = string.GetHashCode(name);
            m_PrimaryBinding = primaryBinding;
            m_SecondaryBinding = secondaryBinding;
            m_PrimaryGamepadBinding = primaryGamepadBinding;
            m_SecondaryGamepadBinding = secondaryGamepadBinding;
        }

//methods
        public void ClearCallbacks()
        {
            E_OnPressed=null; E_OnReleased=null;
        }
        public void Tick(GameTime gameTime, KeyboardState state, GamePadState gpState)
        {
            float deltaTime = gameTime.DeltaTime();

            m_isBeingPressed = IsBindingDown(state, gpState);

            if(WasPressedThisFrame)
            {
                OnPressed();
                m_timeOfLastPress=gameTime.CurrentTime();
                DebuggerManager.Print(Name + " was pressed at " + (float) m_timeOfLastPress, MessageType.INPUT_DEBUG);
            }
            else if(WasReleasedThisFrame)
            {
                OnReleased();
                m_timeOfLastRelease=gameTime.CurrentTime();
                DebuggerManager.Print(Name + " was released at " + (float) m_timeOfLastRelease, MessageType.INPUT_DEBUG);
            } 
            else 
                m_pressStateElapsedTime += deltaTime;
            
            m_wasBeingPressedLastFrame = m_isBeingPressed;
        }

        void OnPressed() {
            E_OnPressed?.Invoke();
            m_pressStateElapsedTime = 0.0f;
        }

        void OnReleased() {
            E_OnReleased?.Invoke();
            m_pressStateElapsedTime = 0.0f;
        }

        bool IsBindingDown_Keyboard(KeyboardState state) => 
            state.IsKeyDown(m_PrimaryBinding) || state.IsKeyDown(m_SecondaryBinding);
        bool IsBindingDown_Gamepad(GamePadState gpState) => 
            gpState.IsButtonDown(m_PrimaryGamepadBinding) || gpState.IsButtonDown(m_SecondaryGamepadBinding);

        bool IsBindingDown(KeyboardState state, GamePadState gpState) =>
            (m_ParentInputManager.KEYBOARD_INPUT_ENABLED && IsBindingDown_Keyboard(state)) ||
            (m_ParentInputManager.GAMEPAD_INPUT_ENABLED && IsBindingDown_Gamepad(gpState));

    }
}