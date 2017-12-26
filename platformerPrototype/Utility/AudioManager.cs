#region using

using System;
using Microsoft.Xna.Framework.Media;

#endregion

namespace platformerPrototype.Utility {
    public class AudioManager {
        private String lastMusicTrack;

        public MediaState BGMState {
            get => MediaPlayer.State;
        }

        /// <summary>
        ///     Handles both new tracks and resuming paused tracks.
        /// </summary>
        public void PlayMusic(String track = null) {
            if (track != null && lastMusicTrack != track) {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = Game.Manager.Options.BgmVolume;
                MediaPlayer.Play(Game.Manager.Resources.Songs[track]);
                lastMusicTrack = track;
            }

            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        public void PauseMusic() {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        public void PlaySFX(String track, Single volumeMod = 1.0f, Single pitch = 1.0f) {
            Game.Manager.Resources.SoundEffects[track].Play(Game.Manager.Options.SfxVolume * volumeMod, pitch, 1.0f);
        }
    }
}
