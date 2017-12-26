#region using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace platformerPrototype.Core {
    public class Camera2D {
        public readonly Viewport Viewport;

        public Vector2 Location;

        public Camera2D(Viewport viewport) {
            Viewport = viewport;

            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Location = Vector2.Zero;
        }

        public Single Rotation { get; set; }
        public Single Zoom { get; set; }
        public Vector2 Origin { get; set; }

        public Matrix GetViewMatrix() {
            return
                Matrix.CreateTranslation(new Vector3(-Location, 0.0f))
                * Matrix.CreateTranslation(new Vector3(-Origin, 0.0f))
                * Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(Zoom, Zoom, 1)
                * Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void Update(Player player) {
            if (player.Position.X - Viewport.Width / 2 > 0)
                Location.X = (player.Position.X - Viewport.Width / 2) * Game.Manager.Options.GetResolutionScaleFactor();
            else
                Location.X = 0;

            if (player.Position.Y < Viewport.Height / 2)
                Location.Y = player.Position.Y - Viewport.Height / 2 * Game.Manager.Options.GetResolutionScaleFactor();
            else
                Location.Y = 0;
        }
    }
}
