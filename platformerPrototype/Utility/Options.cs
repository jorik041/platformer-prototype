#region using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace platformerPrototype.Utility {
    public class Options {
        //16:9 widescreen
        public static Point RESOLUTION_DEFAULT = new Point(960, 540); // 1:1 
        public static Point RESOLUTION_LARGE = new Point(1920, 1080); // 2:1
        public Single BgmVolume = 0.65f;
        public Boolean CanHandleLargeReso = true;

        public Boolean LargeResolution = false;
        public Boolean PlayBgm = true;
        public Boolean PlaySfx = true;
        public Single SfxVolume = 0.4f;

        public Int32 GetResolutionScaleFactor() {
            if (LargeResolution)
                return 2;

            return 1;
        }

        public Int32 GetBackBufferWidth() {
            if (LargeResolution)
                return RESOLUTION_LARGE.X;

            return RESOLUTION_DEFAULT.X;
        }

        public Int32 GetBackBufferHeight() {
            if (LargeResolution)
                return RESOLUTION_LARGE.Y;

            return RESOLUTION_DEFAULT.Y;
        }
    }
}
