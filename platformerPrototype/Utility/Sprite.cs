#region using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace platformerPrototype.Utility {
    /// <summary>
    ///     Handling class for Texture2D.
    /// </summary>
    public class Sprite : IDescriptor {
        private Rectangle _drawRect;
        private Rectangle _srcRect;

        public Sprite(Texture2D texture) {
            Texture = texture;
            _srcRect = texture.Bounds;
            _drawRect = _srcRect;
        }

        public Sprite(Texture2D texture, Rectangle srcRect) {
            Texture = texture;
            _srcRect = srcRect;
            _drawRect = srcRect;
        }

        public String Id { get; internal set; }
        public Texture2D Texture { get; }

        public String GetIdentifier() {
            return Id;
        }

        public void Draw(Int32 x = 0, Int32 y = 0, Int32 xScale = 1, Int32 yScale = 1, Single rotation = 0f,
            Single opacity = 1.0f, Boolean centered = false) {
            _drawRect.X = x * Game.Manager.Options.GetResolutionScaleFactor();
            _drawRect.Y = y * Game.Manager.Options.GetResolutionScaleFactor();
            _drawRect.Width = _srcRect.Width * xScale * Game.Manager.Options.GetResolutionScaleFactor();
            _drawRect.Height = _srcRect.Height * yScale * Game.Manager.Options.GetResolutionScaleFactor();
            Game.Manager.MainSpriteBatch.Draw(Texture, destinationRectangle: _drawRect, sourceRectangle: _srcRect,
                rotation: rotation, color: Color.White * opacity,
                origin: centered ? new Vector2(_srcRect.Width / 2f, _srcRect.Height / 2f) : Vector2.Zero);
        }
    }
}
