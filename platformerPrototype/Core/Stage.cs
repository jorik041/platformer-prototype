#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype.Core {
    public class Stage {
        public const Single GRAVITY_BASE = 0.5f; //Acceleration.
        public List<Actor> Actors = new List<Actor>();

        public Single backgroundtransformation;

        public List<ScrollingBgObject> BgObjects = new List<ScrollingBgObject>();

        public Camera2D Camera;
        public Boolean ChangingWorld;

        public Boolean ChoiceMade;
        public LevelData Data;
        public Single GravityMod = 1f;

        public String LevelId;

        private Double lifeTime;
        public String NextLevelId;
        public List<Platform> Platforms = new List<Platform>();

        public Player Player;
        public Point Spawn;

        public void LoadLevel(String id) {
            Player = null;
            GravityMod = 1f;
            Platforms.Clear();
            Actors.Clear();
            BgObjects.Clear();

            if (Camera == null)
                Camera = new Camera2D(Game.Manager.GraphicsDevice.Viewport);

            LevelId = id;
            try {
                Data = Game.Manager.Resources.LevelDatas[id];
            }
            catch (KeyNotFoundException e) {
                Data = Game.Manager.Resources.LevelDatas["Level1"];
                Game.Manager.PlayerChoseBouncy = false;
                Game.Manager.PlayerChoseIcy = false;
                Game.Manager.PlayerHasDash = false;
                Game.Manager.PlayerHasDoubleJump = false;
                Game.Manager.PlayerHasFloat = false;
                Game.Manager.PlayerHasWallJump = false;
                ChoiceMade = false;
                Game.Manager.SorryScreen.Activate();
            }
            Spawn = new Point(Data.PlayerPos.X, Options.RESOLUTION_DEFAULT.Y - Data.PlayerPos.Y);
            Player = new Player("Player", Spawn);
            Player.Initialize(this);
            GravityMod = Data.GravityModifier;
            if (Data.ChoiceId != null)
                ChoiceMade = false;
            foreach (InstData obj in Data.Objects) {
                GameObjectData data = Game.Manager.Resources.GameObjectDatas[obj.Id];
                if (data.Type == GameObjectType.Platform) {
                    Platform plat = new Platform {
                        ObjectId = data.Id,
                        Graphic = GraphicData.GenerateGraphic(data.GraphicId),
                        Position = new Rectangle(
                            new Point(obj.Pos.X, Options.RESOLUTION_DEFAULT.Y - data.Size.Y - obj.Pos.Y), data.Size),
                        Type = data.PlatformType,
                        StartingPos = obj.Pos,
                        StartingDir = data.Direction,
                        Moving = data.Moving,
                        MoveRange = data.MoveRange,
                        Direction = data.Direction
                    };
                    plat.owner = this;
                    Platforms.Add(plat);
                }
                if (data.Type == GameObjectType.Actor) {
                    Actor a = new Actor {
                        ObjectId = data.Id,
                        Graphic = GraphicData.GenerateGraphic(data.GraphicId),
                        Position = new Rectangle(obj.Pos, data.Size)
                    };
                    a.ActualCoords.X = a.Position.X;
                    a.ActualCoords.Y = a.Position.Y - Options.RESOLUTION_DEFAULT.Y;
                    a.LastPosition = a.Position;
                    Actors.Add(a);
                }
            }

            Random rand = new Random();
            Int32 hills = rand.Next(6, 8);
            for (Int32 a = 0; a < hills * 3; a++) {
                Int32 size = rand.Next(3, 15);
                BgObjects.Add(new ScrollingBgObject(
                    Game.Manager.Resources.Sprites["cloud" + rand.Next(0, 5)],
                    new Vector2(rand.Next(0, Options.RESOLUTION_DEFAULT.X),
                        rand.Next(-10, Options.RESOLUTION_DEFAULT.Y - 100)),
                    (Single)(rand.NextDouble() * 0.4) + 0.1f,
                    new Point(size, size),
                    3f + (Single)(rand.NextDouble() * 15f)
                ));
            }
            for (Int32 b = 0; b < hills; b++) {
                Int32 size = rand.Next(20, 30);
                BgObjects.Add(new ScrollingBgObject(
                    Game.Manager.Resources.Sprites["hill" + rand.Next(0, 3)],
                    new Vector2(rand.Next(20, Options.RESOLUTION_DEFAULT.X + 20), Options.RESOLUTION_DEFAULT.Y),
                    0.85f + (Single)(rand.NextDouble() * 0.15),
                    new Point(size, size),
                    0.2f + (Single)(rand.NextDouble() * 0.8f)
                ));
            }

            NextLevelId = Data.NextLevelId;
            Game.Manager.ChoiceMaker.Load(Data.ChoiceId);
        }

        public void Update(Double timeStep) {
            ChangingWorld = false;
            foreach (Platform i in Platforms)
                i.Update(timeStep);

            Player.Update(timeStep);
            //(Player.Graphic as RectangleGraphic).Rotation += (float)(Math.PI / 60f);
            foreach (Actor a in Actors)
                a.Update(timeStep);

            Camera.Update(Player);
        }

        public void DrawBackground(Double timeStep) {
            lifeTime += timeStep;
            Game.Manager.Resources.Sprites["sun"]
                .Draw(0,
                    Options.RESOLUTION_DEFAULT.Y, 22, 22, (Single)(lifeTime * Math.PI / 50), 0.25f, true);
            foreach (ScrollingBgObject o in BgObjects)
                o.Draw(timeStep);
        }

        public void DrawForeground(Double timeStep) {
            foreach (Platform p in Platforms)
                p.Draw(timeStep);
            foreach (Actor a in Actors)
                a.Draw(timeStep);

            Player.Draw(timeStep);
        }

        public void LoadNextLevel() {
            String id = NextLevelId;
            ChangingWorld = true;
            LoadLevel(id);
            try {
                Game.Manager.ChoiceMaker.Load(Game.Manager.Resources.LevelDatas[id].ChoiceId);
            }
            catch { }
        }
    }

    public class ScrollingBgObject {
        public Single Opacity;
        public Vector2 Position;
        public Point Scale;
        public Single ScrollSpeed;
        public Sprite Sprite;

        public ScrollingBgObject(Sprite sprite, Vector2 pos, Single opacity, Point scale, Single scrollSpeed) {
            Sprite = sprite;
            Position = pos;
            Opacity = opacity;
            Scale = scale;
            ScrollSpeed = scrollSpeed;
        }

        public void Draw(Double timeStep) {
            Position.X -= (Single)(ScrollSpeed * timeStep);
            if (Position.X < -(Sprite.Texture.Width * Scale.X))
                Position.X = Options.RESOLUTION_DEFAULT.X + Sprite.Texture.Width * Scale.X;
            Sprite.Draw((Int32)Position.X,
                (Int32)Position.Y - (Int32)(Game.Manager.CurrentStage.Camera.Location.Y / 10), Scale.X, Scale.Y,
                opacity: Opacity, centered: true);
        }
    }
}
