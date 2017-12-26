#region using

using System;

#endregion

namespace platformerPrototype.Utility {
    public interface IGraphic {
        void Draw(Int32 x, Int32 y, Double timeStep);
    }
}
