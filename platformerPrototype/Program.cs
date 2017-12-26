#region using

using System;

#endregion

namespace platformerPrototype {
    public static class Game {
        public static GameManager Manager;

        private static void Main() {
            Start();
        }

        private static void Start() {
            try {
                Manager = new GameManager();
                Manager.Run();
            }
            finally {
                Manager.Dispose();
            }
        }
    }
}
