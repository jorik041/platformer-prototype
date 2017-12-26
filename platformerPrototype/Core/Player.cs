#region using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class Player : Actor {
        public const Single JUMP_VELO = 10f;
        private const Single HORIZONTAL_SPEED = 0.45f;
        private const Single MAX_SPEED = 3.25f;
        public Boolean bottomconnecting;
        private Double dashcooldown;
        private Int32 dashes;

        private Int32 deathcount;
        public RectangleGraphic Fill;

        // choice metadata
        public Int32 jumpsleft;
        private Boolean leftButtonHeld;
        public Boolean OnLeftWall;
        public Boolean OnRightWall;
        private Boolean rightButtonHeld;

        public Boolean topconnecting;
        public Double WallCooldown;

        public Player(String id, Point position) : base(id, position) { }

        public override void Initialize(Stage stage) {
            base.Initialize(stage);
            Fill = GraphicData.GenerateGraphic("PlayerGraphicF") as RectangleGraphic;
        }

        public override void Update(Double timeStep) {
            foreach (KeyboardCommand kc in Game.Manager.InputHandler.KeyCommands) {
                #region Horizontal Input

                if (kc.Button == Keys.Right || kc.Button == Keys.D) {
                    if (kc.State == InputState.Pressed || kc.State == InputState.Held) {
                        rightButtonHeld = true;
                        if (CurrentPlatformType != PlatformType.Icy && ActualVelocity.X < MAX_SPEED)
                            ActualVelocity.X += HORIZONTAL_SPEED;
                        else if (CurrentPlatformType == PlatformType.Icy && ActualVelocity.X < MAX_SPEED * 2)
                            ActualVelocity.X += HORIZONTAL_SPEED;
                    }
                    if (kc.State == InputState.Released)
                        rightButtonHeld = false;
                }
                if (kc.Button == Keys.Left || kc.Button == Keys.A) {
                    if (kc.State == InputState.Pressed || kc.State == InputState.Held) {
                        leftButtonHeld = true;
                        if (CurrentPlatformType != PlatformType.Icy && ActualVelocity.X > -MAX_SPEED)
                            ActualVelocity.X -= HORIZONTAL_SPEED;
                        else if (CurrentPlatformType == PlatformType.Icy && ActualVelocity.X > -MAX_SPEED * 2)
                            ActualVelocity.X -= HORIZONTAL_SPEED;
                    }
                    if (kc.State == InputState.Released)
                        leftButtonHeld = false;
                }
                if (kc.Button == Keys.J)
                    if (kc.State == InputState.Pressed && kc.State != InputState.Held && dashes > 0) {
                        dashes = dashes - 1;
                        ActualVelocity.X = -JUMP_VELO;
                    }
                if (kc.Button == Keys.K)
                    if (kc.State == InputState.Pressed && dashes > 0) {
                        dashes = dashes - 1;
                        ActualVelocity.X = JUMP_VELO;
                    }

                #endregion

                #region Vertical Input

                if (kc.Button == Keys.Up || kc.Button == Keys.W) {
                    if (kc.State == InputState.Pressed || kc.State == InputState.Held)
                        if (OnGround) {
                            ActualVelocity.Y = -JUMP_VELO;
                            if (kc.State == InputState.Pressed)
                                Game.Manager.Audio.PlaySFX("Jump", pitch: 0.8f);
                        } else if (OnRightWall && kc.State == InputState.Pressed) {
                            ActualVelocity.Y = -JUMP_VELO;
                            ActualVelocity.X = -JUMP_VELO;
                            if (Game.Manager.PlayerHasDoubleJump)
                                jumpsleft = 1;
                            Game.Manager.Audio.PlaySFX("Jump", pitch: 0.8f);
                        } else if (OnLeftWall && kc.State == InputState.Pressed) {
                            ActualVelocity.Y = -JUMP_VELO;
                            ActualVelocity.X = JUMP_VELO;
                            if (Game.Manager.PlayerHasDoubleJump)
                                jumpsleft = 1;
                            Game.Manager.Audio.PlaySFX("Jump", pitch: 0.8f);
                        } else if (jumpsleft > 0 && kc.State == InputState.Pressed) {
                            ActualVelocity.Y = -JUMP_VELO;
                            jumpsleft = jumpsleft - 1;
                            Game.Manager.Audio.PlaySFX("Jump", pitch: 0.8f);
                        }
                    if (!OnGround && kc.State == InputState.Held && Game.Manager.PlayerHasFloat)
                        if (ActualVelocity.Y > JUMP_VELO / 7.5f)
                            ActualVelocity.Y = JUMP_VELO / 7.5f;
                }

                #endregion
            }

            #region Speed Handling

            if (CurrentPlatformType != PlatformType.Icy) {
                if (ActualVelocity.X > MAX_SPEED)
                    ActualVelocity.X -= ActualVelocity.X / 30;

                if (ActualVelocity.X < -MAX_SPEED)
                    ActualVelocity.X -= ActualVelocity.X / 30;

                if (!leftButtonHeld && !rightButtonHeld || leftButtonHeld && rightButtonHeld)
                    ActualVelocity.X -= ActualVelocity.X / 15;
            } else {
                if (ActualVelocity.X > MAX_SPEED * 2)
                    ActualVelocity.X -= ActualVelocity.X / 150;

                if (ActualVelocity.X < -MAX_SPEED * 2)
                    ActualVelocity.X -= ActualVelocity.X / 150;

                if (!leftButtonHeld && !rightButtonHeld || leftButtonHeld && rightButtonHeld)
                    ActualVelocity.X -= ActualVelocity.X / 75;
            }

            #endregion

            if (dashcooldown > 0) {
                dashcooldown -= timeStep * 1000;
            } else if (Game.Manager.PlayerHasDash) {
                dashes = 1;
                dashcooldown = 500;
            }

            if (Stage.ChangingWorld)
                return;

            if (OnGround)
                if (Game.Manager.PlayerHasDoubleJump)
                    jumpsleft = 1;

            OnGround = false;

            if (WallCooldown > 0) {
                WallCooldown = WallCooldown - timeStep * 1000;
            } else {
                OnRightWall = false;
                OnLeftWall = false;
            }
            foreach (Platform i in Stage.Platforms) {
                i.ConnectedObjects.Clear();
                if (CollisionCheck(i))
                    return;
            }

            if (topconnecting && bottomconnecting)
                Die();
            if (ActualCoords.Y > Options.RESOLUTION_DEFAULT.Y)
                Die();

            topconnecting = false;
            bottomconnecting = false;
            Move(timeStep);
        }

        public override void Move(Double timeStep) {
            if (!OnGround) {
                if ((OnRightWall || OnLeftWall) && ActualVelocity.Y > 0)
                    ActualVelocity.Y += Stage.GRAVITY_BASE * Stage.GravityMod / 4;
                else
                    ActualVelocity.Y += Stage.GRAVITY_BASE * Stage.GravityMod;

                CurrentPlatformType = PlatformType.None;
            }

            LastPosition = Position;

            if (ActualVelocity.X > JUMP_VELO * 2)
                ActualVelocity.X = JUMP_VELO * 2;
            else if (ActualVelocity.X < -(JUMP_VELO * 2))
                ActualVelocity.X = -(JUMP_VELO * 2);

            if (ActualVelocity.Y > JUMP_VELO * 2)
                ActualVelocity.Y = JUMP_VELO * 2;
            else if (ActualVelocity.Y < -(JUMP_VELO * 2))
                ActualVelocity.Y = -(JUMP_VELO * 2);

            ActualCoords.X += ActualVelocity.X;
            ActualCoords.Y += ActualVelocity.Y;

            Position.X = (Int32)ActualCoords.X;
            Position.Y = (Int32)ActualCoords.Y;
        }

        public override void Draw(Double timeStep) {
            base.Draw(timeStep);
            Fill.Draw(Position.X + 2, Position.Y + 2, timeStep);
        }

        public void Die() {
            foreach (Platform i in Stage.Platforms) {
                i.Position.X = i.StartingPos.X;
                i.Direction = i.StartingDir;
                i.Position.Y = Options.RESOLUTION_DEFAULT.Y - i.Position.Height - i.StartingPos.Y;
            }

            deathcount++;
            ActualVelocity.X = 0;
            ActualVelocity.Y = 0;
            ActualCoords.X = Stage.Spawn.X;
            ActualCoords.Y = Stage.Spawn.Y;
        }
    }
}
