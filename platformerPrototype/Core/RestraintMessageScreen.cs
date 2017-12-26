#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class RestraintMessageScreen {
        private static readonly Color dimColor = new Color(0f, 0f, 0f) * 0.667f;
        public Boolean Active;
        public List<ChoiceButton> Buttons = new List<ChoiceButton>();

        public RestraintMessageScreen() {
            Buttons.Add(new ChoiceButton("You have completed this path.", new Point(60, 60), Color.Green));
            Buttons.Add(new ChoiceButton("You have been returned to Level 1.", new Point(60, 160), Color.Green));
            Buttons.Add(new ChoiceButton("Thank you for playing Choices.", new Point(60, 260), Color.Green));
        }

        public void Activate() {
            Active = true;
        }

        public Boolean Update(Double timeStep) {
            if (!Active)
                return false;

            foreach (ChoiceButton b in Buttons) {
                if (b.Update()) {
                    PerformButtonCallback(b.Label);
                    break;
                }
            }

            return true;
        }

        private void PerformButtonCallback(String label) {
            Active = false;
        }

        public void Draw(Double timeStep) {
            if (!Active)
                return;

            Game.Manager.DrawRectangle(Point.Zero, Options.RESOLUTION_DEFAULT, dimColor); //Shades the stage below.

            foreach (ChoiceButton b in Buttons)
                b.Draw();
        }
    }
}
