#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class ChoiceMaker {
        private static readonly Color dimColor = new Color(0f, 0f, 0f) * 0.667f;
        public Boolean Active;
        public List<ChoiceButton> Buttons = new List<ChoiceButton>();
        public Boolean Initialized;

        public void Load(String dataId) {
            Buttons.Clear();
            Initialized = false;
            if (dataId == null)
                return;

            ChoiceData data = Game.Manager.Resources.ChoiceMakerDatas[dataId];
            Buttons.Add(new ChoiceButton(data.ChoiceA, new Point(60, 60), data.ColorA));
            Buttons.Add(new ChoiceButton(data.ChoiceB, new Point(60, 160), data.ColorB));
            if (data.Happens == ChoiceMakerHappens.AtStart)
                Active = true;
            Initialized = true;
        }

        public Boolean Update(Double timeStep) {
            if (!Active || !Initialized)
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
            Game.Manager.CurrentStage.ChoiceMade = true;
            switch (label) {
                case "Double Jump":
                    Game.Manager.PlayerHasDoubleJump = true;
                    Game.Manager.CurrentStage.LoadLevel("Level2a1");
                    break;
                case "Dash (Use J/K)":
                    Game.Manager.PlayerHasDash = true;
                    Game.Manager.CurrentStage.LoadLevel("Level2b1");
                    break;
                case "Bouncy Platforms":
                    Game.Manager.PlayerChoseBouncy = true;
                    if (Game.Manager.PlayerHasDoubleJump)
                        Game.Manager.CurrentStage.LoadLevel("Level3aa1");
                    else if (Game.Manager.PlayerHasDash)
                        Game.Manager.CurrentStage.LoadLevel("Level3ba1");
                    break;
                case "Icy Platforms":
                    Game.Manager.PlayerChoseIcy = true;
                    if (Game.Manager.PlayerHasDoubleJump)
                        Game.Manager.CurrentStage.LoadLevel("Level3ab1");
                    else if (Game.Manager.PlayerHasDash)
                        Game.Manager.CurrentStage.LoadLevel("Level3bb1");
                    break;
                case "Wall Jump":
                    if (Game.Manager.PlayerHasDoubleJump && Game.Manager.PlayerChoseBouncy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4aaa1");
                        Game.Manager.PlayerHasWallJump = true;
                    } else if (Game.Manager.PlayerHasDoubleJump && Game.Manager.PlayerChoseIcy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4aba1");
                        Game.Manager.PlayerHasWallJump = true;
                    } else if (Game.Manager.PlayerHasDash && Game.Manager.PlayerChoseBouncy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4baa1");
                        Game.Manager.PlayerHasWallJump = true;
                    } else if (Game.Manager.PlayerHasDash && Game.Manager.PlayerChoseIcy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4bba1");
                        Game.Manager.PlayerHasWallJump = true;
                    }
                    break;
                case "Float (Hold Jump)":
                    if (Game.Manager.PlayerHasDoubleJump && Game.Manager.PlayerChoseBouncy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4aab1");
                        Game.Manager.PlayerHasFloat = true;
                    } else if (Game.Manager.PlayerHasDoubleJump && Game.Manager.PlayerChoseIcy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4abb1");
                        Game.Manager.PlayerHasFloat = true;
                    } else if (Game.Manager.PlayerHasDash && Game.Manager.PlayerChoseBouncy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4bab1");
                        Game.Manager.PlayerHasFloat = true;
                    } else if (Game.Manager.PlayerHasDash && Game.Manager.PlayerChoseIcy) {
                        Game.Manager.CurrentStage.LoadLevel("Level4bbb1");
                        Game.Manager.PlayerHasFloat = true;
                    }
                    break;

                default:
                    throw new NotImplementedException(
                        String.Format("\"{0}\" was not recognized as a button callback!"));
            }
        }

        public void Draw(Double timeStep) {
            if (!Active || !Initialized)
                return;

            Game.Manager.DrawRectangle(Point.Zero, Options.RESOLUTION_DEFAULT, dimColor); //Shades the stage below.

            foreach (ChoiceButton b in Buttons)
                b.Draw();
        }
    }

    public class ChoiceButton {
        private static Color buttonRectColor = new Color(45, 45, 45);
        private readonly Color _hoverColor;
        private readonly Color _pressColor;
        private Boolean _hovering;
        private Boolean _pressing;

        private Color textColor;

        public ChoiceButton(String label, Point pos, Color hoverColor) {
            Label = label;
            Vector2 v2 = Game.Manager.Resources.SpriteFonts["DefaultFont"].MeasureString(label);
            Position = new Rectangle(pos, new Point((Int32)v2.X, (Int32)v2.Y));
            _hoverColor = hoverColor;
            _pressColor = new Color(hoverColor.R + 25, hoverColor.G + 25, hoverColor.B + 25);
        }

        public Rectangle Position { get; }
        public String Label { get; }

        public Boolean Update() {
            _hovering = false;
            _pressing = false;
            if (Position.Contains(Game.Manager.InputHandler.MousePosition)) {
                _hovering = true;
                foreach (MouseCommand c in Game.Manager.InputHandler.MouseCommands) {
                    if (c.Button != MouseKeys.LeftButton)
                        continue;

                    switch (c.State) {
                        case InputState.Pressed:
                        case InputState.Held:
                            _pressing = true;
                            break;
                        case InputState.Released:
                            return true;
                    }
                }
            }

            return false;
        }

        public void Draw() {
            //Game.Manager.DrawRectangle(Position.Location, Position.Size, buttonRectColor);

            if (_pressing)
                textColor = _pressColor;
            else if (_hovering)
                textColor = _hoverColor;
            else
                textColor = Color.White;

            Game.Manager.MainSpriteBatch.DrawString(Game.Manager.Resources.SpriteFonts["DefaultFont"],
                Label, new Vector2(Position.X, Position.Y), textColor);
        }
    }
}
