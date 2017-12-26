#region using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace platformerPrototype.Utility {
    public abstract class InputCommand {
        public InputState State;
    }

    public enum MouseKeys {
        LeftButton,
        RightButton,
        MiddleButton
    }

    public enum InputState {
        Pressed,
        Held,
        Released
    }

    public class MouseCommand : InputCommand {
        public MouseKeys Button;
        public Point Position;

        public MouseCommand(MouseKeys btn, InputState state, Point pos) {
            Button = btn;
            State = state;
            Position = pos;
        }
    }

    public class KeyboardCommand : InputCommand {
        public Keys Button;

        public KeyboardCommand(Keys btn, InputState state) {
            Button = btn;
            State = state;
        }
    }
}
