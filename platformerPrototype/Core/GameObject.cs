#region using

using System;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class GameObject {
        public IGraphic Graphic;
        public Rectangle Position;

        public GameObject() { }

        public GameObject(IGraphic graphic, Rectangle position) {
            ObjectId = "CUSTOM";
            Graphic = graphic;
            Position = position;
        }

        public GameObject(String id, Point position) {
            GameObjectData data = Game.Manager.Resources.GameObjectDatas[id];
            Graphic = GraphicData.GenerateGraphic(data.GraphicId);
            Position = new Rectangle(position, data.Size);
        }

        public String ObjectId { get; internal set; }

        public Point Center {
            get => Position.Center;
            set {
                Position.X += value.X - Position.Center.X;
                Position.Y += value.Y - Position.Center.Y;
            }
        }

        public static GameObject GenerateObject(String id, Point position) {
            GameObjectData data = Game.Manager.Resources.GameObjectDatas[id];
            GameObject ret;
            if (data.Type == GameObjectType.Platform) {
                ret = new Platform {
                    ObjectId = data.Id,
                    Graphic = GraphicData.GenerateGraphic(data.GraphicId),
                    Position = new Rectangle(position, data.Size)
                };
                return ret;
            }

            if (data.Type == GameObjectType.Actor) {
                ret = new Actor {
                    ObjectId = data.Id,
                    Graphic = GraphicData.GenerateGraphic(data.GraphicId),
                    Position = new Rectangle(position, data.Size)
                };
                (ret as Actor).ActualCoords.X = position.X;
                (ret as Actor).ActualCoords.Y = position.Y;
                (ret as Actor).LastPosition = ret.Position;
                return ret;
            }

            throw new Exception("Could not parse the object!");
        }

        public virtual void Update(Double timeStep) { }

        public virtual void Draw(Double timeStep) {
            Graphic.Draw(Position.X, Position.Y, timeStep);
        }
    }
}
