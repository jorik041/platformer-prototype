#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace platformerPrototype.Utility {
    public class InputHandler : IDisposable {
        public StackArray<KeyboardState> KeyboardHistory;
        public List<Keys> KeyboardListeners;
        public StackArray<MouseState> MouseHistory;
        public List<MouseKeys> MouseListeners;

        public InputHandler(Int32 framesToKeep = 3) {
            MouseHistory =
                new StackArray<MouseState>(framesToKeep,
                    (Int32)(framesToKeep * 0.05)); //Larger collections demand more padding.
            KeyboardHistory = new StackArray<KeyboardState>(framesToKeep, (Int32)(framesToKeep * 0.05));
            MouseListeners = new List<MouseKeys>(Enum.GetValues(typeof(MouseKeys)).Length);
            KeyboardListeners = new List<Keys>(Enum.GetValues(typeof(Keys)).Length);
        }

        public Boolean Disposed { get; internal set; }

        public Point MousePosition { get; internal set; }
        public List<MouseCommand> MouseCommands { get; internal set; } = new List<MouseCommand>();
        public List<KeyboardCommand> KeyCommands { get; internal set; } = new List<KeyboardCommand>();

        public void Dispose() {
            if (Disposed)
                return;

            MouseHistory.Dispose();
            KeyboardHistory.Dispose();
            MouseListeners.Clear();
            KeyboardListeners.Clear();
            MouseCommands.Clear();
            KeyCommands.Clear();
            Disposed = true;
        }

        public void RegisterMouseListener(MouseKeys key) {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            if (MouseListeners.Contains(key))
                return;

            MouseListeners.Add(key);
        }

        public void UnregisterMouseListener(MouseKeys key) {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            MouseListeners.Remove(key);
        }

        public void RegisterKeyboardListener(Keys key) {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            if (KeyboardListeners.Contains(key))
                return;

            KeyboardListeners.Add(key);
        }

        public void UnregisterKeyboardListener(Keys key) {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            KeyboardListeners.Remove(key);
        }

        public void Update(MouseState mState, KeyboardState kState) {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            MouseHistory.Insert(mState);
            KeyboardHistory.Insert(kState);
        }

        public void GenerateCommands() {
            if (Disposed)
                throw new ObjectDisposedException("Attempted to access InputHandler after object is disposed!");

            MouseCommands.Clear();
            KeyCommands.Clear();

            MousePosition = MouseHistory[0].Position;
            for (Byte a = 0; a < MouseListeners.Count; a++) {
                switch (MouseListeners[a]) {
                    case MouseKeys.LeftButton:
                        HandleLeftMouseButton();
                        break;
                    case MouseKeys.MiddleButton:
                        HandleMiddleMouseButton();
                        break;
                    case MouseKeys.RightButton:
                        HandleRightMouseButton();
                        break;
                }
            }
            for (Int16 b = 0; b < KeyboardListeners.Count; b++)
                HandleKeyboardButton(KeyboardListeners[b]);
        }

        private void HandleLeftMouseButton() {
            if (MouseHistory[0].LeftButton == ButtonState.Pressed)
                if (MouseHistory[1].LeftButton == ButtonState.Pressed) //Adds a one frame latency.
                    MouseCommands.Add(new MouseCommand(MouseKeys.LeftButton, InputState.Held,
                        MouseHistory[0].Position));
                else
                    MouseCommands.Add(new MouseCommand(MouseKeys.LeftButton, InputState.Pressed,
                        MouseHistory[0].Position));
            else if (MouseHistory[1].LeftButton == ButtonState.Pressed)
                MouseCommands.Add(new MouseCommand(MouseKeys.LeftButton, InputState.Released,
                    MouseHistory[0].Position));
        }

        private void HandleRightMouseButton() {
            if (MouseHistory[0].RightButton == ButtonState.Pressed)
                if (MouseHistory[1].RightButton == ButtonState.Pressed)
                    MouseCommands.Add(
                        new MouseCommand(MouseKeys.RightButton, InputState.Held, MouseHistory[0].Position));
                else
                    MouseCommands.Add(new MouseCommand(MouseKeys.RightButton, InputState.Pressed,
                        MouseHistory[0].Position));
            else if (MouseHistory[1].RightButton == ButtonState.Pressed)
                MouseCommands.Add(
                    new MouseCommand(MouseKeys.RightButton, InputState.Released, MouseHistory[0].Position));
        }

        private void HandleMiddleMouseButton() {
            if (MouseHistory[0].MiddleButton == ButtonState.Pressed)
                if (MouseHistory[1].MiddleButton == ButtonState.Pressed)
                    MouseCommands.Add(new MouseCommand(MouseKeys.MiddleButton, InputState.Held,
                        MouseHistory[0].Position));
                else
                    MouseCommands.Add(new MouseCommand(MouseKeys.MiddleButton, InputState.Pressed,
                        MouseHistory[0].Position));
            else if (MouseHistory[1].MiddleButton == ButtonState.Pressed)
                MouseCommands.Add(new MouseCommand(MouseKeys.MiddleButton, InputState.Released,
                    MouseHistory[0].Position));
        }

        private void HandleKeyboardButton(Keys key) {
            if (KeyboardHistory[0].IsKeyDown(key))
                if (KeyboardHistory[1].IsKeyDown(key))
                    KeyCommands.Add(new KeyboardCommand(key, InputState.Held));
                else
                    KeyCommands.Add(new KeyboardCommand(key, InputState.Pressed));
            else if (KeyboardHistory[1].IsKeyDown(key))
                KeyCommands.Add(new KeyboardCommand(key, InputState.Released));
        }
    }
}
