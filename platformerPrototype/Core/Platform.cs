#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public enum PlatformType {
        //Background: Black
        //Player: Black 2px outline on white
        None,
        Default, //DarkGray
        Bouncy, //???
        Icy, //LightBlue
        Win //???
    }

    public enum PlatformSide {
        Undefined,
        Top,
        Bottom,
        Right,
        Left
    }

    public enum Direction {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public class Platform : GameObject {
        private static readonly Color DefaultPlatformColor = new Color(50, 50, 50);

        //moving platforms
        public List<Actor> ConnectedObjects = new List<Actor>();
        public Direction Direction;
        public Single MoveRange;
        public Boolean Moving;

        public Stage owner;
        public Direction StartingDir;
        public Point StartingPos;
        public PlatformType Type;

        public Platform() { }

        public Platform(String id, Point position, PlatformType type = PlatformType.Default) : base(id, position) {
            Type = type;
        }

        public Boolean OnCollide(Actor a, PlatformSide side) {
            switch (Type) {
                case PlatformType.Default:
                case PlatformType.Icy:
                    RespondNormal(a, side);
                    break;
                case PlatformType.Bouncy:
                    RespondBouncy(a, side);
                    break;
                case PlatformType.Win:
                    RespondWin(a, side);
                    return true;
            }

            return false;
        }

        /*
        public bool OnCollide(Actor a)
        {
            var ret = false;
            PlatformSide side = PlatformSide.Undefined;

            //If my feet were above the top (y is lesser) of the platform earlier, then now I must be on top of it.
            //Also, if my velocity is down or zero, then that must be it. I cannot land on top of a platform if I am moving up, UNLESS the direction is up.
            
            if (((Direction != Direction.Up) ? a.LastPosition.Bottom <= Position.Top : a.LastPosition.Bottom >= Position.Top)
                && ((Direction != Direction.Up) ? a.ActualVelocity.Y >= 0 : true)) //I land on a platform.
                side = PlatformSide.Top;
            //If my head was below (y is greater) the bottom of the platform earlier, then now I must have hit the bottom.
            //Also, if my velocity is up then that must be it. I cannot hit my head on the bottom of a platform if I am moving down or not moving vertically at all.
            else if (((Direction != Direction.Down) ? a.LastPosition.Top >= Position.Bottom : a.LastPosition.Top <= Position.Bottom)
                && ((Direction != Direction.Down) ? a.ActualVelocity.Y <= 0 : true)) //I hit my head on the underside of a platform.
                side = PlatformSide.Bottom;

            //If my left was to the right (x is greater) of the platform earlier, then now I must have hit its right.
            else if (((Direction != Direction.Right) ? a.LastPosition.Left >= Position.Right : a.LastPosition.Left <= Position.Right)
                && ((Direction != Direction.Right) ? a.ActualVelocity.X <= 0 : true)) //Sideways collision from the right of this platform.
                side = PlatformSide.Right;

            //If my right was to the left (x is lesser) of the platform earlier, then now I must have hit its left.
            //a.LastPosition.Right <= Position.Left
            else if (((Direction != Direction.Left) ? a.LastPosition.Right <= Position.Left : a.LastPosition.Right >= Position.Left)
                && ((Direction != Direction.Left) ? a.ActualVelocity.X >= 0 : true)) //Sideways collision from the left of this platform.
                side = PlatformSide.Left;

            switch (Type)
            {
                case PlatformType.Default:
                case PlatformType.Icy:
                    RespondNormal(a, side);
                    break;
                case PlatformType.Bouncy:
                    RespondBouncy(a, side);
                    break;
                case PlatformType.Win:
                    RespondWin(a, side);
                    ret = true;
                    break;
            }
            return ret;
        }
        */

        private void RespondNormal(Actor a, PlatformSide side) {
            switch (side) {
                case PlatformSide.Top: {
                    if (a.ActualVelocity.Y > 0)
                        a.ActualVelocity.Y = 0;
                    a.ActualCoords.Y = Position.Top - a.Position.Height;

                    a.CurrentPlatformType = Type;
                    a.OnGround = true;

                    ConnectedObjects.Add(a);
                }
                    break;
                case PlatformSide.Bottom: {
                    if (a.ActualVelocity.Y < 0)
                        a.ActualVelocity.Y = 0;
                    a.ActualCoords.Y = Position.Bottom;

                    ConnectedObjects.Add(a);
                }
                    break;
                case PlatformSide.Right: {
                    if (a.ActualVelocity.X < 0)
                        a.ActualVelocity.X = 0;
                    a.ActualCoords.X = Position.Right;

                    if (Game.Manager.PlayerHasWallJump) {
                        (a as Player).OnLeftWall = true;
                        (a as Player).WallCooldown = 100;
                    }

                    ConnectedObjects.Add(a);
                }
                    break;
                case PlatformSide.Left: {
                    if (a.ActualVelocity.X > 0)
                        a.ActualVelocity.X = 0;
                    a.ActualCoords.X = Position.Left - a.Position.Width;
                    if (Game.Manager.PlayerHasWallJump) {
                        (a as Player).OnRightWall = true;
                        (a as Player).WallCooldown = 100;
                    }

                    ConnectedObjects.Add(a);
                }
                    break;
            }
        }

        private void RespondBouncy(Actor a, PlatformSide side) {
            switch (side) {
                case PlatformSide.Top: {
                    if (a.ActualCoords.Y != Position.Top - a.Position.Height) {
                        a.ActualCoords.Y = Position.Top - a.Position.Height;
                        a.ActualVelocity.Y = -a.ActualVelocity.Y * 0.9f < -Player.JUMP_VELO
                            ? -a.ActualVelocity.Y * 0.9f
                            : -Player.JUMP_VELO;
                        a.CurrentPlatformType = Type;
                    }
                }
                    break;
                case PlatformSide.Bottom: {
                    if (a.ActualCoords.Y != Position.Bottom)
                        a.ActualCoords.Y = Position.Bottom;
                    a.ActualVelocity.Y = -a.ActualVelocity.Y;
                }
                    break;
                case PlatformSide.Right:
                    if (a.ActualVelocity.X > 0)
                        a.ActualVelocity.X = a.ActualVelocity.X * 2;
                    else
                        a.ActualVelocity.X = -a.ActualVelocity.X * 2;
                    break;
                case PlatformSide.Left:
                    if (a.ActualVelocity.X > 0)
                        a.ActualVelocity.X = -a.ActualVelocity.X * 2;
                    else
                        a.ActualVelocity.X = a.ActualVelocity.X * 2;
                    break;
            }
        }

        private void RespondWin(Actor a, PlatformSide side) {
            //throw new System.NotImplementedException("Please implement stage progression!");
            if (!(a is Player))
                return;

            if (!a.Stage.ChoiceMade && Game.Manager.ChoiceMaker.Initialized)
                Game.Manager.ChoiceMaker.Active = true;
            else
                a.Stage.LoadNextLevel();
        }

        public override void Update(Double timeStep) {
            base.Update(timeStep);
            if (Moving)
                Move(owner.Player);
        }

        public void Move(Actor player) {
            switch (Direction) {
                case Direction.None:
                    break;
                case Direction.Right:
                    if (Position.X - StartingPos.X < MoveRange) {
                        Position.X++;
                        foreach (Actor i in ConnectedObjects)
                            i.ActualCoords.X++;
                    } else {
                        Direction = Direction.Left;
                    }

                    break;
                case Direction.Left:
                    if (Position.X - StartingPos.X > -MoveRange) {
                        Position.X--;
                        foreach (Actor i in ConnectedObjects)
                            i.ActualCoords.X--;
                    } else {
                        Direction = Direction.Right;
                    }

                    break;
                case Direction.Up:
                    if (Options.RESOLUTION_DEFAULT.Y - Position.Height - Position.Y - StartingPos.Y < MoveRange)
                        Position.Y--;
                    else
                        Direction = Direction.Down;
                    break;
                case Direction.Down:
                    if (Options.RESOLUTION_DEFAULT.Y - Position.Height - Position.Y - StartingPos.Y > -MoveRange) {
                        Position.Y++;
                        foreach (Actor i in ConnectedObjects)
                            i.ActualCoords.Y++;
                    } else {
                        Direction = Direction.Up;
                    }

                    break;
            }
        }

        public override void Draw(Double timeStep) {
            base.Draw(timeStep);
        }

        public static Color GetPlatformTypeColor(PlatformType type) {
            switch (type) {
                case PlatformType.Bouncy:
                    return Color.Orange;
                case PlatformType.Win:
                    return Color.Green;
                case PlatformType.Icy:
                    return Color.LightBlue;
                default:
                    return DefaultPlatformColor;
            }
        }
    }
}
