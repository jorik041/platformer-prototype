#region using

using System;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class LevelData : IDescriptor {
        public String ChoiceId;
        public Single GravityModifier = 1f;
        public String LevelId;
        public String NextLevelId;
        public InstData[] Objects;
        public Point PlayerPos;

        public LevelData(String id, String choiceId, Point playerPos, String nextLevelId, Single gravity = 1f,
            params InstData[] objects) {
            LevelId = id;
            ChoiceId = choiceId;
            PlayerPos = playerPos;
            NextLevelId = nextLevelId;
            GravityModifier = gravity;
            Objects = objects;
        }

        public LevelData(String id, String choiceId, Point playerPos, String nextLevelId, InstData[] objects,
            Single gravity = 1f) {
            LevelId = id;
            ChoiceId = choiceId;
            PlayerPos = playerPos;
            NextLevelId = nextLevelId;
            GravityModifier = gravity;
            Objects = objects;
        }

        public String GetIdentifier() {
            return LevelId;
        }
    }

    public class InstData {
        public String Id;
        public Point Pos;

        public InstData(String id, Point pos) {
            Id = id;
            Pos = pos;
        }
    }
}
