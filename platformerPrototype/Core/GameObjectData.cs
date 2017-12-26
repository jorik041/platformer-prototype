#region using

using System;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class GameObjectData : IDescriptor {
        public GameObjectType Type { get; internal set; }
        public PlatformType PlatformType { get; internal set; } = PlatformType.Default;
        public String Id { get; internal set; }
        public String GraphicId { get; internal set; }
        public Point Size { get; internal set; }

        public Boolean Moving { get; internal set; } = false;
        public Single MoveRange { get; internal set; } = 0;
        public Direction Direction { get; internal set; } = Direction.None;

        public String GetIdentifier() {
            return Id;
        }
    }

    public enum GameObjectType {
        Platform,
        Actor
    }
}
