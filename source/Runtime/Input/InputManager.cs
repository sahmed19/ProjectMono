using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectMono.Input {

    public class InputManager
    {
        public readonly bool KEYBOARD_INPUT_ENABLED = true;
        public readonly bool GAMEPAD_INPUT_ENABLED = true;

        Dictionary<int, InputAction> m_InputActionDictionary;
        Dictionary<int, InputAxis> m_InputAxisDictionary;
        
        void AddInputAction(InputAction action) => m_InputActionDictionary.Add(action.ID, action);
        void AddInputAxis(InputAxis axis) => m_InputAxisDictionary.Add(axis.ID, axis);

        public InputAction GetInputAction(string name) => GetInputAction(string.GetHashCode(name));
        public InputAction GetInputAction(int ID) => m_InputActionDictionary[ID];
        public InputAxis GetInputAxis(string name) => GetInputAxis(string.GetHashCode(name));
        public InputAxis GetInputAxis(int ID) => m_InputAxisDictionary[ID];
        
        Game m_Game;

        public InputManager(Game game) {
            m_Game = game;
            m_InputActionDictionary = new Dictionary<int, InputAction>();
            m_InputAxisDictionary = new Dictionary<int, InputAxis>();

            AddInputAction(new InputAction(this, "MOVE_LEFT", "Move left.", Keys.Left, Keys.None, Buttons.LeftThumbstickLeft, Buttons.DPadLeft));
            AddInputAction(new InputAction(this, "MOVE_RIGHT", "Move right.", Keys.Right, Keys.None, Buttons.LeftThumbstickRight, Buttons.DPadRight));
            AddInputAction(new InputAction(this, "MOVE_DOWN", "Move down.", Keys.Down, Keys.None, Buttons.LeftThumbstickDown, Buttons.DPadDown));
            AddInputAction(new InputAction(this, "MOVE_UP", "Move up.", Keys.Up, Keys.None, Buttons.LeftThumbstickUp, Buttons.DPadUp));
            AddInputAction(new InputAction(this, "JUMP", "Jump into the air.", Keys.Z, Buttons.A));
            AddInputAction(new InputAction(this, "SHOOT", "Fire your currently equipped weapon.", Keys.X, Buttons.B));
            AddInputAction(new InputAction(this, "PREV_WEAPON", "Equip your previous weapon.", Keys.A, Buttons.LeftShoulder));
            AddInputAction(new InputAction(this, "NEXT_WEAPON", "Equip your next weapon.", Keys.S, Buttons.RightShoulder));

            AddInputAxis(new InputAxis("MOVE_HORIZONTAL", "Move horizontally.", 
                GetInputAction("MOVE_RIGHT"), GetInputAction("MOVE_LEFT")));
            AddInputAxis(new InputAxis("MOVE_VERTICAL", "Move vertically.", 
                GetInputAction("MOVE_UP"), GetInputAction("MOVE_DOWN")));
        }

        public void Tick(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            foreach(var InputAction in m_InputActionDictionary.Values)
                InputAction.Tick(gameTime, keyboardState, gamepadState);   
        }

        public void LateTick() {
            foreach(var InputAction in m_InputActionDictionary.Values)
                InputAction.ResetLastFramePressState();  
        }

    }

    public class InputAxis {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ID { get; private set; }
        InputAction m_PositiveAction;
        InputAction m_NegativeAction;
        
        public InputAxis(string name, string description, InputAction positiveAction, InputAction negativeAction)
        {
            Name = name;
            Description = description;
            ID = string.GetHashCode(name);
            m_PositiveAction = positiveAction;
            m_NegativeAction = negativeAction;
        }

        public float Value() {
            float value = 0.0f;
            if(m_PositiveAction.IsBeingPressed)
                value += 1.0f;
            if(m_NegativeAction.IsBeingPressed)
                value -= 1.0f;

            return value;
        }

        public static implicit operator float(InputAxis axis) {
            return axis.Value();
        }

    }

}