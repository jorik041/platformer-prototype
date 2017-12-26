#region using

using System;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public enum ChoiceMakerHappens {
        AtStart,
        AtWin
    }

    public class ChoiceData : IDescriptor {
        public String ChoiceA;
        public String ChoiceB;
        public Color ColorA;
        public Color ColorB;
        public ChoiceMakerHappens Happens;
        public String Id;

        public ChoiceData(String id, String a, String b, Color cA, Color cB,
            ChoiceMakerHappens happens = ChoiceMakerHappens.AtStart) {
            Id = id;
            ChoiceA = a;
            ColorA = cA;
            ChoiceB = b;
            ColorB = cB;
            Happens = happens;
        }

        public String GetIdentifier() {
            return Id;
        }
    }
}
