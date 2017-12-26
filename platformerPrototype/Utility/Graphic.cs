#region using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace platformerPrototype.Utility {
    /// <summary>
    ///     Graphics are containers of sprites. They may carry effect data, animation data, and other state data necessary to
    ///     function alongside a timeStep.
    ///     They also are useful for animation.
    /// </summary>
    public class BasicGraphic : IGraphic {
        private readonly Sprite _sprite;
        public Point DrawScale;
        public String GraphicId;
        public Single Opacity;
        public Single Rotation;

        public BasicGraphic(Sprite sprite, Point scale = default(Point), Single rot = 0f, Single opacity = 1.0f) {
            GraphicId = "CUSTOM";
            _sprite = sprite;
            if (scale == default(Point))
                DrawScale = new Point(1, 1);
            else
                DrawScale = scale;
            Rotation = rot;
            Opacity = opacity;
        }

        public BasicGraphic(String id) {
            BasicGraphicData data = Game.Manager.Resources.GraphicDatas[id] as BasicGraphicData;
            GraphicId = id;
            _sprite = Game.Manager.Resources.Sprites[data.SpriteId];
            DrawScale = data.DrawScale;
            Rotation = data.Rotation;
            Opacity = data.Opacity;
        }

        public void Draw(Int32 x, Int32 y, Double timeStep) {
            _sprite.Draw(x, y, DrawScale.X, DrawScale.Y, Rotation, Opacity);
        }
    }

    /// <summary>
    ///     Cycles through animations, GBA-style zero tweening. Default draws per frame is 3 to emulate old animation.
    /// </summary>
    public class SimpleAnimGraphic : IGraphic {
        private readonly Int32 _drawsPerFrame;
        private readonly Sprite[] _sprites;

        //state data
        private readonly Int64 drawCount = 0;
        //zero -> betweens -> end -> zero
        public Point DrawScale;
        public String GraphicId;
        public Single Opacity;
        public Single Rotation;

        public SimpleAnimGraphic(Sprite[] sprites, Point scale = default(Point), Single rot = 0f, Single opacity = 1.0f,
            Int32 drawsPerFrame = 3) {
            GraphicId = "CUSTOM";
            _sprites = sprites;
            if (scale == default(Point))
                DrawScale = new Point(1, 1);
            else
                DrawScale = scale;
            Rotation = rot;
            Opacity = opacity;
            _drawsPerFrame = drawsPerFrame;
        }

        public SimpleAnimGraphic(String id) {
            SimpleAnimGraphicData data = Game.Manager.Resources.GraphicDatas[id] as SimpleAnimGraphicData;
            GraphicId = id;
            _sprites = new Sprite[data.SpriteIds.Length];
            for (Int16 a = 0; a < _sprites.Length; a++)
                _sprites[a] = Game.Manager.Resources.Sprites[data.SpriteIds[a]];

            DrawScale = data.DrawScale;
            Rotation = data.Rotation;
            Opacity = data.Opacity;
            _drawsPerFrame = data.DrawsPerFrame;
        }

        public void Draw(Int32 x, Int32 y, Double timeStep) {
            //explanation
            //_sprites.Length * _drawsPerFrame is the total amount of draws for the entire animation. (as totDraws)
            //drawCount % totDraws disregards previous total draws and returns only remainder
            //remainder / drawsPerFrame gets frame progress
            //flooring frame progress returns current frame
            Int32 frame = (Int32)Math.Floor((Single)(drawCount % (_sprites.Length * _drawsPerFrame)) / _drawsPerFrame);

            _sprites[frame].Draw(x, y, DrawScale.X, DrawScale.Y, Rotation, Opacity);
        }
    }

    public class RectangleGraphic : IGraphic {
        public Color Color;
        public Boolean DrawCentered;
        public Point DrawSize;
        public String GraphicId;
        public Single Opacity;
        public Single Rotation;

        public RectangleGraphic(Point size, Color color, Single rot = 0f, Single opacity = 1.0f,
            Boolean drawCentered = false) {
            GraphicId = "CUSTOM";
            DrawSize = size;
            Color = color;
            Rotation = rot;
            Opacity = opacity;
            DrawCentered = drawCentered;
        }

        public RectangleGraphic(String id) {
            RectangleGraphicData data = Game.Manager.Resources.GraphicDatas[id] as RectangleGraphicData;
            GraphicId = id;
            DrawSize = data.DrawSize;
            Color = data.Color;
            Rotation = data.Rotation;
            Opacity = data.Opacity;
            DrawCentered = data.DrawCentered;
        }

        public void Draw(Int32 x, Int32 y, Double timeStep) {
            Game.Manager.DrawRectangle(new Point(x, y), DrawSize, Color, Rotation, Opacity,
                DrawCentered ? new Vector2(DrawSize.X / 2, DrawSize.Y / 2) : Vector2.Zero);
        }
    }
}
