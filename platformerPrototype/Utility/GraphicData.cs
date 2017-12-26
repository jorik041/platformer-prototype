#region using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace platformerPrototype.Utility {
    public static class GraphicData {
        public static IGraphic GenerateGraphic(String graphicId) {
            IDescriptor data = Game.Manager.Resources.GraphicDatas[graphicId];
            if (data is BasicGraphicData)
                return new BasicGraphic(graphicId);
            if (data is SimpleAnimGraphic)
                return new SimpleAnimGraphic(graphicId);
            if (data is RectangleGraphicData)
                return new RectangleGraphic(graphicId);

            throw new NotImplementedException(
                String.Format("Graphic ID \"{0}\" was not supported by GenerateGraphic(string)!", graphicId));
        }
    }

    public class BasicGraphicData : IDescriptor {
        public String Id { get; internal set; }
        public String SpriteId { get; internal set; }
        public Point DrawScale { get; internal set; }
        public Single Rotation { get; internal set; }
        public Single Opacity { get; internal set; }

        public String GetIdentifier() {
            return Id;
        }
    }

    public class SimpleAnimGraphicData : IDescriptor {
        public String Id { get; internal set; }
        public String[] SpriteIds { get; internal set; }
        public Point DrawScale { get; internal set; }
        public Single Rotation { get; internal set; }
        public Single Opacity { get; internal set; }
        public Int32 DrawsPerFrame { get; internal set; }

        public String GetIdentifier() {
            return Id;
        }
    }

    public class RectangleGraphicData : IDescriptor {
        public String Id { get; internal set; }
        public Point DrawSize { get; internal set; }
        public Color Color { get; internal set; }
        public Single Rotation { get; internal set; }
        public Single Opacity { get; internal set; }
        public Boolean DrawCentered { get; internal set; }

        public String GetIdentifier() {
            return Id;
        }
    }
}
