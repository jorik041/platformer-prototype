#region using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace platformerPrototype.Core {
    public class Actor : GameObject {
        private Boolean _init;
        public Vector2 ActualCoords;
        public Vector2 ActualVelocity = new Vector2(0, 0);

        public PlatformType CurrentPlatformType = PlatformType.None;
        public Rectangle LastPosition;
        public Boolean OnGround;
        public Stage Stage;

        public Actor() { }

        public Actor(String id, Point position) : base(id, position) {
            ActualCoords.X = position.X;
            ActualCoords.Y = position.Y;
            LastPosition = Position;
        }

        public virtual void Initialize(Stage stage) {
            Stage = stage;
            _init = true;
        }

        public override void Update(Double timeStep) {
            if (!_init)
                throw new InvalidOperationException("Actor called Update() before initialization!");

            if (Stage.ChangingWorld)
                return;

            OnGround = false;
            foreach (Platform i in Stage.Platforms) {
                if (CollisionCheck(i))
                    return;
            }

            Move(timeStep);
        }

        public virtual void Move(Double timeStep) {
            if (!OnGround) {
                ActualVelocity.Y += Stage.GRAVITY_BASE * Stage.GravityMod;
                CurrentPlatformType = PlatformType.None;
            }

            LastPosition = Position;

            ActualCoords.X += ActualVelocity.X;
            ActualCoords.Y += ActualVelocity.Y;

            Position.X = (Int32)ActualCoords.X;
            Position.Y = (Int32)ActualCoords.Y;
        }

        public override void Draw(Double timeStep) {
            base.Draw(timeStep);
        }

        public virtual Boolean CollisionCheck(Platform collider) {
            Boolean collidesWithTop;
            Boolean collidesWithBottom;
            Boolean collidesWithRight;
            Boolean collidesWithLeft;
            Rectangle cRect = new Rectangle(collider.Position.X - 1, collider.Position.Y - 1,
                collider.Position.Width + 2, collider.Position.Height + 2);
            if (!Position.Intersects(cRect) && !cRect.Contains(Position))
                return false;
            //if (!cRect.Contains(Position)) return false;

            // Top/Bottom Corner Check
            collidesWithTop = Position.Intersects(new Rectangle(collider.Position.X - 1, collider.Position.Y - 1,
                collider.Position.Width + 2, collider.Position.Height / 2 + 2));
            collidesWithBottom = Position.Intersects(new Rectangle(collider.Position.X - 1,
                collider.Position.Center.Y - 1, collider.Position.Width + 2, collider.Position.Height / 2 + 2));
            // Right/Left Corner Check
            collidesWithRight = Position.Intersects(new Rectangle(collider.Position.Center.X - 1,
                collider.Position.Y - 1, collider.Position.Width / 2 + 2, collider.Position.Height + 2));
            collidesWithLeft = Position.Intersects(new Rectangle(collider.Position.X - 1, collider.Position.Y - 1,
                collider.Position.Width / 2 + 2, collider.Position.Height + 2));

            //if (collidesWithTop && collidesWithBottom)
            //{
            //    if (collidesWithRight)
            //        return collider.OnCollide(this, PlatformSide.Right);
            //    else if (collidesWithLeft)
            //        return collider.OnCollide(this, PlatformSide.Left);
            //    else 
            //        return collider.OnCollide(this, PlatformSide.Top);

            //}

            //if (collidesWithRight && collidesWithLeft)
            //{
            //    if (collidesWithTop)
            //        return collider.OnCollide(this, PlatformSide.Top);
            //    if (collidesWithBottom)
            //        return collider.OnCollide(this, PlatformSide.Bottom);
            //}

            if (collidesWithTop)
                (this as Player).topconnecting = true;
            if (collidesWithBottom && Position.Top >= collider.Position.Bottom && collider.Direction == Direction.Down)
                (this as Player).bottomconnecting = true;
            if (collidesWithTop) {
                if (collidesWithRight) //TR
                    if (Position.Right <= collider.Position.Right)
                        return collider.OnCollide(this, PlatformSide.Top);
                    else if (Position.Bottom >= collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Right);
                    else if (Position.Bottom - collider.Position.Top <= collider.Position.Right - Position.Left)
                        return collider.OnCollide(this, PlatformSide.Top); //TOP COLLISION
                    else
                        return collider.OnCollide(this, PlatformSide.Right); //RIGHT COLLISION

                if (collidesWithLeft) //TL
                    if (Position.Left >= collider.Position.Left)
                        return collider.OnCollide(this, PlatformSide.Top);
                    else if (Position.Bottom >= collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Left);
                    else if (Position.Bottom - collider.Position.Top <= Position.Right - collider.Position.Left)
                        return collider.OnCollide(this, PlatformSide.Top); //TOP COLLISION
                    else
                        return collider.OnCollide(this, PlatformSide.Left); //LEFT COLLISION
            } else {
                if (collidesWithRight) //BR
                    if (Position.Bottom <= collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Right);
                    else if (Position.Right <= collider.Position.Right)
                        return collider.OnCollide(this, PlatformSide.Bottom);
                    else if (Position.Left - collider.Position.Right <= Position.Top - collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Right); //RIGHT COLLISION
                    else
                        return collider.OnCollide(this, PlatformSide.Bottom); //BOTTOM COLLISION

                if (collidesWithLeft) //BL
                    if (Position.Bottom <= collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Left);
                    else if (Position.Left >= collider.Position.Left)
                        return collider.OnCollide(this, PlatformSide.Bottom);
                    else if (Position.Right - collider.Position.Left <= Position.Top - collider.Position.Bottom)
                        return collider.OnCollide(this, PlatformSide.Left); //LEFT COLLISION
                    else
                        return collider.OnCollide(this, PlatformSide.Bottom); //BOTTOM COLLISION
            }

            return false;
        }
    }
}
