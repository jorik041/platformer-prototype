#region using

using System;
using System.Diagnostics;

#endregion

namespace platformerPrototype.Utility {
    public class FrameTimer {
        private readonly Stopwatch _stopwatch;
        private readonly Int32 _updateMs;
        private Int64 _accumulator;
        private Int64 _totalElapsedMs;
        private Int32 _updateCount;
        public Int64 LastMsDelta = -1;
        public Double UpdatesPerSecond = -1;

        public FrameTimer(Int32 updateMs = 2000) {
            _stopwatch = new Stopwatch();
            _updateMs = updateMs;
        }

        public void Start() {
            _stopwatch.Start();
            _totalElapsedMs = _stopwatch.ElapsedMilliseconds;
        }

        public void Stop() {
            _stopwatch.Stop();
            _totalElapsedMs = _stopwatch.ElapsedMilliseconds;
        }

        public void Update() {
            _updateCount++;
            LastMsDelta = _stopwatch.ElapsedMilliseconds - _totalElapsedMs;
            _totalElapsedMs += LastMsDelta;
            _accumulator += LastMsDelta;
            if (_accumulator > _updateMs) {
                UpdatesPerSecond = (Double)_updateCount / _accumulator;
                _accumulator = 0;
                _updateCount = 0;
            }
        }
    }
}
