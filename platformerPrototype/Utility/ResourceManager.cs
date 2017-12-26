#region using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using platformerPrototype.Core;

#endregion

namespace platformerPrototype.Utility {
    public class ResourceManager {
        public SortedList<String, ChoiceData> ChoiceMakerDatas = new SortedList<String, ChoiceData>();
        public SortedList<String, GameObjectData> GameObjectDatas = new SortedList<String, GameObjectData>();
        public SortedList<String, IDescriptor> GraphicDatas = new SortedList<String, IDescriptor>();
        public SortedList<String, LevelData> LevelDatas = new SortedList<String, LevelData>();
        public SortedList<String, Song> Songs = new SortedList<String, Song>();
        public SortedList<String, SoundEffect> SoundEffects = new SortedList<String, SoundEffect>();
        public SortedList<String, SpriteFont> SpriteFonts = new SortedList<String, SpriteFont>();
        public SortedList<String, Sprite> Sprites = new SortedList<String, Sprite>();

        public void Initialize(ContentManager c) {
            //Load resources here.
            InitializeSprites(c);
            InitializeSpriteFonts(c);
            InitializeSongs(c);
            InitializeSoundEffects(c);
            InitializeGraphicDatas();
            InitializeObjectDatas();
            InitializeChoiceMakerDatas();
            InitializeLevelDatas();
        }

        public void Unload() {
            Sprites.Clear();
            SpriteFonts.Clear();
            GraphicDatas.Clear();
            GameObjectDatas.Clear();
            ChoiceMakerDatas.Clear();
            LevelDatas.Clear();
        }

        public RectangleGraphicData RetrieveRectData(String id) {
            IDescriptor d = Game.Manager.Resources.GraphicDatas[id];
            if (!(d is RectangleGraphicData))
                throw new Exception("Tried to retrieve a RectangleGraphicData from a non-rectangle graphic index!");

            return d as RectangleGraphicData;
        }

        private void RectGraphic(String id, Point size, Color color, Single rotation = 0f, Single opacity = 1f,
            Boolean centered = false) {
            GraphicDatas.Add(id, new RectangleGraphicData {
                Id = id,
                DrawSize = size,
                Color = color,
                Rotation = rotation,
                Opacity = opacity,
                DrawCentered = centered
            });
        }

        private void GameObject(String id, String graphicId, Point size = default(Point)) {
            GameObjectDatas.Add(id, new GameObjectData {
                Id = id,
                GraphicId = graphicId,
                Size = size == default(Point) ? Game.Manager.Resources.RetrieveRectData(graphicId).DrawSize : size
            });
        }

        /// <summary>
        ///     Create a platform data.
        /// </summary>
        private void Platform(String id, String graphicId, Point size = default(Point),
            PlatformType type = PlatformType.Default, Boolean moving = false, Single moverange = 0,
            Direction direction = Direction.None) {
            GameObjectDatas.Add(id, new GameObjectData {
                Id = id,
                GraphicId = graphicId,
                Size = size == default(Point) ? Game.Manager.Resources.RetrieveRectData(graphicId).DrawSize : size,
                PlatformType = type,
                Moving = moving,
                MoveRange = moverange,
                Direction = direction
            });
        }

        /// <summary>
        ///     Create a platform data AND its RectangleGraphic data together.
        /// </summary>
        private void Platform(String id, Point size, PlatformType type = PlatformType.Default, Boolean moving = false,
            Single moverange = 0, Direction direction = Direction.None, Color color = default(Color)) {
            GraphicDatas.Add(id + "Graphic", new RectangleGraphicData {
                Id = id + "Graphic",
                DrawSize = size,
                Color = color == default(Color) ? Core.Platform.GetPlatformTypeColor(type) : color,
                Rotation = 0f,
                Opacity = 1f,
                DrawCentered = false
            });
            GameObjectDatas.Add(id, new GameObjectData {
                Id = id,
                GraphicId = id + "Graphic",
                Size = size,
                PlatformType = type,
                Moving = moving,
                MoveRange = moverange,
                Direction = direction
            });
        }

        private void ChoiceMaker(String id, String choiceA, Color colorA, String choiceB, Color colorB,
            ChoiceMakerHappens happens) {
            ChoiceMakerDatas.Add(id, new ChoiceData(id, choiceA, choiceB, colorA, colorB, happens));
        }

        private void Level(String id, String choiceId, Point playerPos, String nextLvl, Single gravity = 1f,
            params InstData[] objects) {
            LevelDatas.Add(id, new LevelData(id, choiceId, playerPos, nextLvl, gravity, objects));
        }

        private void Level(String id, String choiceId, Point playerPos, String nextLvl, InstData[] objects,
            Single gravity = 1f) {
            LevelDatas.Add(id, new LevelData(id, choiceId, playerPos, nextLvl, gravity, objects));
        }

        //Add to this.
        private void InitializeSprites(ContentManager c) {
            Sprites.Add("sun", new Sprite(c.Load<Texture2D>(Content.Assets.sun)));
            Texture2D hills = c.Load<Texture2D>(Content.Assets.hills);
            Sprites.Add("hill0", new Sprite(hills, new Rectangle(2, 1, 7, 7)));
            Sprites.Add("hill1", new Sprite(hills, new Rectangle(13, 2, 12, 6)));
            Sprites.Add("hill2", new Sprite(hills, new Rectangle(0, 11, 13, 7)));
            Sprites.Add("hill3", new Sprite(hills, new Rectangle(16, 9, 10, 9)));
            Texture2D clouds = c.Load<Texture2D>(Content.Assets.clouds);
            Sprites.Add("cloud0", new Sprite(clouds, new Rectangle(3, 2, 6, 3)));
            Sprites.Add("cloud1", new Sprite(clouds, new Rectangle(10, 3, 3, 2)));
            Sprites.Add("cloud2", new Sprite(clouds, new Rectangle(15, 3, 5, 2)));
            Sprites.Add("cloud3", new Sprite(clouds, new Rectangle(22, 1, 9, 6)));
            Sprites.Add("cloud4", new Sprite(clouds, new Rectangle(8, 10, 6, 3)));
            Sprites.Add("cloud5", new Sprite(clouds, new Rectangle(17, 10, 7, 3)));
        }

        //Add to this.
        private void InitializeSpriteFonts(ContentManager c) {
            SpriteFonts.Add("DefaultFont", c.Load<SpriteFont>(Content.Assets.text));
        }

        //Add to this.
        private void InitializeSongs(ContentManager c) {
            Songs.Add("Balcony", c.Load<Song>(Content.Assets.music));
        }

        //Add to this.
        private void InitializeSoundEffects(ContentManager c) {
            SoundEffects.Add("Jump", c.Load<SoundEffect>(Content.Assets.jump));
        }

        // Add to this.
        private void InitializeGraphicDatas() {
            RectGraphic("PlayerGraphic", new Point(20, 20), Color.Black);
            RectGraphic("PlayerGraphicF", new Point(16, 16), Color.White);
        }

        // Add to this.
        private void InitializeObjectDatas() {
            GameObject("Player", "PlayerGraphic");

            //This specific configuration allows you to skip graphicData creation entirely.
            Platform("Floor", new Point(2500, 25));

            //Win
            Platform("Win", new Point(25, 25), PlatformType.Win);
            Platform("MovingWin", new Point(25, 25), PlatformType.Win, true, 50, Direction.Left);

            //Normal Presets
            Platform("NormalPlatformSmall", new Point(50, 25));
            Platform("NormalPlatformMedium", new Point(100, 25));
            Platform("NormalPlatformLarge", new Point(150, 25));
            Platform("NormalPlatformLarge2", new Point(250, 25));
            Platform("NormalPlatformHuge", new Point(300, 25));
            Platform("NormalPlatformHuge2", new Point(400, 25));

            Platform("NormalBlockSmall", new Point(50, 50));
            Platform("NormalBlockMedium", new Point(100, 100));
            Platform("NormalBlockLarge", new Point(150, 150));

            Platform("MovingBlockSmall", new Point(50, 50), PlatformType.Default, true, 50, Direction.Left);
            Platform("MovingBlockSmall2", new Point(50, 50), PlatformType.Default, true, 50, Direction.Right);

            Platform("MovingBlockSmallBouncy", new Point(50, 50), PlatformType.Bouncy, true, 50, Direction.Left);
            Platform("MovingBlockSmallBouncy2", new Point(50, 50), PlatformType.Bouncy, true, 50, Direction.Right);
            Platform("MovingBlockSmallBouncy3", new Point(50, 50), PlatformType.Bouncy, true, 5000, Direction.Left);

            Platform("MovingBlockMedium", new Point(100, 100), PlatformType.Default, true, 50, Direction.Left);
            Platform("MovingBlockLarge", new Point(150, 150), PlatformType.Default, true, 50, Direction.Left);

            Platform("NormalBlockMediumStacked", new Point(100, 200));
            Platform("NormalBlockLargeStacked", new Point(200, 400));
            Platform("NormalBlockLargeStacked2", new Point(200, 600));

            Platform("ThinPillarSmall", new Point(25, 50));
            Platform("ThinPillarMedium", new Point(25, 100));
            Platform("ThinPillarLarge", new Point(25, 150));

            Platform("ThickPillarSmall", new Point(50, 100));
            Platform("ThickPillarMedium", new Point(50, 200));
            Platform("ThickPillarLarge", new Point(50, 300));
            Platform("ThickPillarLarge2", new Point(50, 400));
            Platform("ThickPillarHuge", new Point(50, 600));
            Platform("ThickPillarHuge2", new Point(50, 1200));

            //Moving Presets
            Platform("MovingPlatformMedium", new Point(100, 25), PlatformType.Default, true, 50, Direction.Right);
            Platform("MovingPlatformMedium2", new Point(100, 25), PlatformType.Default, true, 50, Direction.Left);
            Platform("MovingPlatformMedium3", new Point(100, 25), PlatformType.Default, true, 200, Direction.Up);
            Platform("MovingPlatformMedium4", new Point(100, 25), PlatformType.Default, true, 200, Direction.Down);
            Platform("MovingPlatformMedium5", new Point(100, 25), PlatformType.Default, true, 50, Direction.Down);
            Platform("MovingPlatformMedium6", new Point(100, 25), PlatformType.Default, true, 50, Direction.Up);

            Platform("MovingPlatformLarge", new Point(200, 25), PlatformType.Default, true, 500, Direction.Left);
            Platform("MovingPlatformLarge2", new Point(200, 25), PlatformType.Default, true, 200, Direction.Left);

            Platform("MovingPlatformSmall1", new Point(50, 25), PlatformType.Default, true, 200, Direction.Up);
            Platform("MovingPlatformSmall2", new Point(50, 25), PlatformType.Default, true, 200, Direction.Down);
            Platform("MovingPlatformSmall3", new Point(50, 25), PlatformType.Default, true, 200, Direction.Left);
            Platform("MovingPlatformSmall4", new Point(50, 25), PlatformType.Default, true, 200, Direction.Right);

            Platform("MovingThickPillar", new Point(50, 500), PlatformType.Default, true, 50, Direction.Up);
            Platform("MovingThickPillar2", new Point(50, 500), PlatformType.Default, true, 50, Direction.Down);
            Platform("MovingPillarMedium", new Point(50, 200), PlatformType.Default, true, 100, Direction.Left);
            Platform("MovingPillarMedium2", new Point(50, 200), PlatformType.Default, true, 100, Direction.Right);

            Platform("MovingLargeThickPillar", new Point(100, 600), PlatformType.Default, true, 60, Direction.Up);
            Platform("MovingLargeThickPillar2", new Point(100, 600), PlatformType.Default, true, 60, Direction.Down);

            Platform("MovingMegaBlock", new Point(1000, 800), PlatformType.Default, true, 120, Direction.Up);
            Platform("MovingMegaBlock2", new Point(1000, 800), PlatformType.Default, true, 120, Direction.Down);

            //Bouncy Presets
            Platform("BouncySmall", new Point(50, 25), PlatformType.Bouncy);
            Platform("BouncyMedium", new Point(75, 25), PlatformType.Bouncy);
            Platform("BouncyLarge", new Point(100, 25), PlatformType.Bouncy);
            Platform("BouncyHuge", new Point(200, 25), PlatformType.Bouncy);

            Platform("ThickBouncySmall", new Point(50, 50), PlatformType.Bouncy);
            Platform("ThickBouncyMedium", new Point(75, 50), PlatformType.Bouncy);
            Platform("ThickBouncyLarge", new Point(100, 50), PlatformType.Bouncy);
            Platform("ThickBouncyHuge", new Point(200, 50), PlatformType.Bouncy);
            Platform("ThickBouncyHuge2", new Point(300, 50), PlatformType.Bouncy);

            Platform("BouncyPillar", new Point(50, 300), PlatformType.Bouncy);
            Platform("BouncyPillar2", new Point(100, 300), PlatformType.Bouncy);

            Platform("BouncyHugeMoving", new Point(200, 25), PlatformType.Bouncy, true, 100, Direction.Up);
            Platform("BouncyHugeMoving2", new Point(200, 25), PlatformType.Bouncy, true, 50, Direction.Left);
            Platform("BouncyHugeMoving3", new Point(200, 25), PlatformType.Bouncy, true, 50, Direction.Right);
            Platform("BouncyHugeMoving4", new Point(200, 25), PlatformType.Bouncy, true, 150, Direction.Up);
            Platform("BouncyHugeMoving5", new Point(200, 25), PlatformType.Bouncy, true, 6000, Direction.Right);

            //Icy Presets
            Platform("IcyFloor", new Point(4500, 25), PlatformType.Icy);
            Platform("IcyFloor2", new Point(2500, 25), PlatformType.Icy);

            Platform("IcyCube", new Point(25, 25), PlatformType.Icy);
            Platform("IcySmall", new Point(50, 25), PlatformType.Icy);
            Platform("IcyMedium", new Point(100, 25), PlatformType.Icy);
            Platform("IcyLarge", new Point(150, 25), PlatformType.Icy);
            Platform("IcyHuge", new Point(200, 25), PlatformType.Icy);
            Platform("IcyHuge2", new Point(400, 25), PlatformType.Icy);

            Platform("IcyLargeMoving", new Point(150, 25), PlatformType.Icy, true, 100, Direction.Left);
            Platform("IcyLargeMoving2", new Point(150, 25), PlatformType.Icy, true, 100, Direction.Right);
            Platform("IcyLargeMoving3", new Point(150, 25), PlatformType.Icy, true, 100, Direction.Up);
            Platform("IcyLargeMoving4", new Point(150, 25), PlatformType.Icy, true, 100, Direction.Down);

            Platform("IcySmallMoving", new Point(50, 25), PlatformType.Icy, true, 100, Direction.Left);
            Platform("IcySmallMoving2", new Point(50, 25), PlatformType.Icy, true, 100, Direction.Right);
            Platform("IcySmallMoving3", new Point(50, 25), PlatformType.Icy, true, 100, Direction.Up);
            Platform("IcySmallMoving4", new Point(50, 25), PlatformType.Icy, true, 100, Direction.Down);

            Platform("IcyThinPillarSmall", new Point(25, 50), PlatformType.Icy);
            Platform("IcyThinPillarMedium", new Point(25, 100), PlatformType.Icy);
            Platform("IcyThinPillarLarge", new Point(25, 150), PlatformType.Icy);
            Platform("IcyThinPillarHuge", new Point(25, 300), PlatformType.Icy);

            Platform("IcyThinPillarMoving", new Point(25, 300), PlatformType.Icy, true, 100, Direction.Left);
            Platform("IcyThinPillarMoving2", new Point(25, 300), PlatformType.Icy, true, 100, Direction.Right);

            Platform("IcyThickPillarSmall", new Point(50, 100), PlatformType.Icy);
            Platform("IcyThickPillarMedium", new Point(50, 200), PlatformType.Icy);
            Platform("IcyThickPillarLarge", new Point(50, 300), PlatformType.Icy);
            Platform("IcyThickPillarHuge", new Point(50, 600), PlatformType.Icy);
            Platform("IcyThickPillarHuge2", new Point(50, 1200), PlatformType.Icy);

            Platform("IcyThickPillarMoving", new Point(50, 900), PlatformType.Icy, true, 50, Direction.Down);
            Platform("IcyThickPillarMoving2", new Point(50, 900), PlatformType.Icy, true, 50, Direction.Up);
            Platform("IcyThickPillarMoving3", new Point(50, 900), PlatformType.Icy, true, 100, Direction.Down);
            Platform("IcyThickPillarMoving4", new Point(50, 900), PlatformType.Icy, true, 100, Direction.Up);

            Platform("IcyBlockLarge", new Point(200, 200), PlatformType.Icy);
            Platform("IcyBlockLargeStacked", new Point(200, 400), PlatformType.Icy);
            Platform("IcyBlockLargeStacked2", new Point(200, 600), PlatformType.Icy);
            Platform("IcyBlockLargeStacked3", new Point(200, 800), PlatformType.Icy);
            Platform("IcyBlockLargeStacked4", new Point(200, 1000), PlatformType.Icy);
            Platform("IcyBlockLargeStacked5", new Point(200, 1200), PlatformType.Icy);
            Platform("IcyBlockLargeStacked6", new Point(200, 1400), PlatformType.Icy);
            Platform("IcyBlockLargeStacked7", new Point(200, 1600), PlatformType.Icy);
            Platform("IcyBlockLargeStacked8", new Point(200, 1800), PlatformType.Icy);

            Platform("MegaBlock", new Point(1000, 800));

            //Testing
            Platform("PillarA", new Point(40, 90), PlatformType.Bouncy);
            Platform("PlatformA", new Point(90, 40), PlatformType.Bouncy);
            Platform("Floor2", new Point(Options.RESOLUTION_DEFAULT.X, 25), PlatformType.Icy);

            //Specifics
            Platform("abab-Sec2DownPillar", new Point(50, 600));
            Platform("abab-Sec2Plat", new Point(100, 50));
            Platform("abab-Sec2UpPillar", new Point(50, 300));
            Platform("abab-Sec2Ceiling", new Point(1000, 200));

            Platform("abab-Sec3Floor", new Point(1510, 50));
            Platform("abab-Sec3Block", new Point(175, 200));
            Platform("abab-Sec3MBlock", new Point(200, 200), PlatformType.Bouncy, true, 70, Direction.Up);
            Platform("abab-Sec3Roadbump", new Point(10, 10), PlatformType.Bouncy);
            Platform("abab-Sec3DownPillar", new Point(50, 400));
            Platform("abab-Sec3Floor2", new Point(350, 50));
            Platform("abab-Sec3Ceiling2", new Point(350, 200));
            Platform("abab-Sec3MegaCeiling", new Point(1750, 500));
            Platform("abab-Sec4BounceUp", new Point(50, 50), PlatformType.Bouncy, true, 80, Direction.Up);
            Platform("abab-Sec4BounceDown", new Point(50, 50), PlatformType.Bouncy, true, 80, Direction.Down);
            Platform("abab-Sec4BounceLeft", new Point(50, 50), PlatformType.Bouncy, true, 80, Direction.Left);
            Platform("abab-Sec4BounceRight", new Point(50, 50), PlatformType.Bouncy, true, 80, Direction.Right);
        }

        //Add to this.
        private void InitializeChoiceMakerDatas() {
            ChoiceMaker("First", "Double Jump", Color.Orange, "Dash (Use J/K)", Color.Blue, ChoiceMakerHappens.AtWin);
            ChoiceMaker("Second", "Bouncy Platforms", Color.Orange, "Icy Platforms", Color.Blue,
                ChoiceMakerHappens.AtWin);
            ChoiceMaker("Third", "Wall Jump", Color.Orange, "Float (Hold Jump)", Color.Blue, ChoiceMakerHappens.AtWin);
        }

        // Add to this.
        private void InitializeLevelDatas() {
            Level("TestZone", null, new Point(20, 200), "", 1,
                new InstData("Floor", new Point(0, 0)),
                new InstData("PillarA", new Point(90, 165)),
                new InstData("PillarA", new Point(50, 210)),
                new InstData("PlatformA", new Point(185, 105)),
                new InstData("PlatformA", new Point(245, 155)),
                new InstData("Floor2", new Point(400, 25)),
                new InstData("Win", new Point(250, 50)),
                new InstData("MovingPlatformMedium", new Point(600, 150)),
                new InstData("MovingPlatformMedium", new Point(600, 50)),
                new InstData("MovingPlatformMedium", new Point(820, 100))
            );

            //LevelSet 1 (No Choice) (COMPLETE)
            //1 Level
            Level("Level1", "First", new Point(20, 100), "", 1,
                new InstData("Floor", new Point(0, 0)),
                new InstData("NormalBlockSmall", new Point(250, 0)),
                new InstData("NormalBlockSmall", new Point(300, 25)),
                new InstData("NormalBlockSmall", new Point(400, 25)),
                new InstData("NormalBlockSmall", new Point(450, 0)),
                new InstData("NormalBlockMedium", new Point(750, 0)),
                new InstData("NormalBlockLarge", new Point(950, 0)),
                new InstData("NormalBlockMedium", new Point(1200, 0)),
                new InstData("ThinPillarSmall", new Point(1800, 25)),
                new InstData("ThinPillarMedium", new Point(1900, 10)),
                new InstData("ThinPillarLarge", new Point(2000, 0)),
                new InstData("ThinPillarMedium", new Point(2100, 10)),
                new InstData("ThinPillarSmall", new Point(2200, 25)),
                new InstData("NormalPlatformMedium", new Point(2600, 60)),
                new InstData("NormalPlatformMedium", new Point(2800, 100)),
                new InstData("NormalPlatformMedium", new Point(3000, 125)),
                new InstData("NormalPlatformLarge", new Point(3225, 125)),
                new InstData("Win", new Point(3300, 150))
            );

            //LevelSet 2a (Double Jump) (COMPLETE)
            //3 Levels
            Level("Level2a1", null, new Point(20, 100), "Level2a2", 1,
                new InstData("NormalPlatformLarge", new Point(0, 0)),
                new InstData("NormalPlatformMedium", new Point(225, 150)),
                new InstData("NormalPlatformMedium", new Point(500, 200)),
                new InstData("NormalPlatformSmall", new Point(800, 200)),
                new InstData("NormalPlatformSmall", new Point(1050, 200)),
                new InstData("NormalPlatformSmall", new Point(1350, 200)),
                new InstData("NormalPlatformSmall", new Point(1350, 375)),
                new InstData("NormalPlatformSmall", new Point(1350, 550)),
                new InstData("NormalPlatformSmall", new Point(1350, 725)),
                new InstData("NormalPlatformMedium", new Point(1650, 650)),
                new InstData("NormalPlatformMedium", new Point(2050, 350)),
                new InstData("NormalPlatformMedium", new Point(2450, 50)),
                new InstData("NormalPlatformLarge", new Point(2800, 50)),
                new InstData("MovingPlatformMedium", new Point(3200, 50)),
                new InstData("NormalPlatformLarge", new Point(3600, 50)),
                new InstData("Win", new Point(3675, 75))
            );
            Level("Level2a2", null, new Point(125, 300), "Level2a3", 1,
                new InstData("NormalPlatformLarge", new Point(100, 200)),
                new InstData("ThickPillarLarge", new Point(275, 275)),
                new InstData("NormalPlatformHuge", new Point(350, 200)),
                new InstData("MovingThickPillar", new Point(700, 225)),
                new InstData("NormalPlatformHuge", new Point(800, 200)),
                new InstData("MovingPlatformMedium", new Point(1300, 200)),
                new InstData("MovingThickPillar", new Point(1475, 225)),
                new InstData("MovingPlatformMedium2", new Point(1600, 200)),
                new InstData("MovingPlatformMedium3", new Point(2000, 300)),
                new InstData("MovingPlatformMedium4", new Point(2300, 300)),
                new InstData("MovingPlatformMedium3", new Point(2650, 300)),
                new InstData("MovingPlatformMedium4", new Point(3050, 300)),
                new InstData("MovingPlatformMedium6", new Point(3300, 350)),
                new InstData("MovingPlatformMedium5", new Point(3550, 300)),
                new InstData("MovingPlatformMedium6", new Point(3800, 250)),
                new InstData("MovingPlatformMedium5", new Point(4050, 200)),
                new InstData("MovingPlatformMedium2", new Point(4400, 150)),
                new InstData("MovingWin", new Point(4450, 176))
            );
            Level("Level2a3", "Second", new Point(565, 300), "Level2a3", 1,
                new InstData("NormalPlatformLarge", new Point(500, 200)),
                new InstData("MovingPlatformMedium4", new Point(525, 500)),
                new InstData("MovingPlatformMedium3", new Point(700, 900)),
                new InstData("MovingPlatformMedium4", new Point(350, 1200)),
                new InstData("MovingPlatformMedium3", new Point(525, 1700)),
                new InstData("MovingPlatformSmall2", new Point(375, 2200)),
                new InstData("MovingPlatformSmall2", new Point(725, 2200)),
                new InstData("MovingPlatformSmall1", new Point(550, 2700)),
                new InstData("NormalPlatformSmall", new Point(550, 3050)),
                new InstData("NormalPlatformSmall", new Point(550, 3200)),
                new InstData("NormalPlatformSmall", new Point(550, 3350)),
                new InstData("NormalPlatformSmall", new Point(550, 3500)),
                new InstData("NormalPlatformSmall", new Point(640, 3650)),
                new InstData("NormalPlatformSmall", new Point(460, 3750)),
                new InstData("NormalPlatformSmall", new Point(660, 3850)),
                new InstData("NormalPlatformSmall", new Point(440, 3950)),
                new InstData("NormalPlatformSmall", new Point(550, 4050)),
                new InstData("MovingPlatformSmall3", new Point(550, 4150)),
                new InstData("MovingPlatformSmall4", new Point(550, 4250)),
                new InstData("MovingPlatformSmall3", new Point(550, 4350)),
                new InstData("MovingPlatformSmall4", new Point(550, 4450)),
                new InstData("MovingPlatformSmall3", new Point(550, 4550)),
                new InstData("MovingPlatformSmall4", new Point(550, 4650)),
                new InstData("MovingPlatformSmall1", new Point(550, 4900)),
                new InstData("Win", new Point(562, 5200))
            );

            //LevelSet 2b (Dash) (COMPLETE)
            //3 Levels

            Level("Level2b1", null, new Point(20, 100), "Level2b2", 1,
                new InstData("NormalPlatformLarge", new Point(0, 0)),
                new InstData("NormalPlatformLarge", new Point(350, 0)),
                new InstData("NormalPlatformLarge", new Point(700, 0)),
                new InstData("ThickPillarLarge", new Point(950, -200)),
                new InstData("ThickPillarLarge", new Point(950, 250)),
                new InstData("ThickPillarLarge", new Point(1200, -200)),
                new InstData("ThickPillarLarge", new Point(1200, 250)),
                new InstData("ThickPillarLarge", new Point(1450, -200)),
                new InstData("ThickPillarLarge", new Point(1450, 250)),
                new InstData("ThickPillarLarge", new Point(1700, -200)),
                new InstData("ThickPillarLarge", new Point(1700, 250)),
                new InstData("MovingThickPillar", new Point(1850, 250)),
                new InstData("MovingThickPillar2", new Point(1850, -325)),
                new InstData("MovingThickPillar", new Point(2000, -325)),
                new InstData("MovingThickPillar2", new Point(2000, 250)),
                new InstData("MovingThickPillar", new Point(2150, 250)),
                new InstData("MovingThickPillar2", new Point(2150, -325)),
                new InstData("MovingThickPillar", new Point(2300, -325)),
                new InstData("MovingThickPillar2", new Point(2300, 250)),
                new InstData("MovingThickPillar2", new Point(2450, -325)),
                new InstData("MovingPlatformLarge2", new Point(2850, 25)),
                new InstData("Win", new Point(3200, 100))
            );
            Level("Level2b2", null, new Point(150, 300), "Level2b3", 1,
                new InstData("NormalPlatformHuge", new Point(100, 200)),
                new InstData("MovingThickPillar", new Point(500, 275)),
                new InstData("MovingThickPillar2", new Point(500, -325)),
                new InstData("MovingThickPillar", new Point(550, 265)),
                new InstData("MovingThickPillar2", new Point(550, -335)),
                new InstData("MovingThickPillar", new Point(600, 255)),
                new InstData("MovingThickPillar2", new Point(600, -345)),
                new InstData("MovingThickPillar", new Point(650, 245)),
                new InstData("MovingThickPillar2", new Point(650, -355)),
                new InstData("MovingThickPillar", new Point(700, 235)),
                new InstData("MovingThickPillar2", new Point(700, -365)),
                new InstData("MovingThickPillar", new Point(750, 225)),
                new InstData("MovingThickPillar2", new Point(750, -375)),
                new InstData("MovingThickPillar", new Point(950, -375)),
                new InstData("MovingThickPillar2", new Point(950, 225)),
                new InstData("MovingThickPillar", new Point(1150, 225)),
                new InstData("MovingThickPillar2", new Point(1150, -375)),
                new InstData("MovingThickPillar", new Point(1200, 235)),
                new InstData("MovingThickPillar2", new Point(1200, -365)),
                new InstData("MovingThickPillar", new Point(1250, 245)),
                new InstData("MovingThickPillar2", new Point(1250, -355)),
                new InstData("MovingThickPillar", new Point(1300, 255)),
                new InstData("MovingThickPillar2", new Point(1300, -345)),
                new InstData("MovingThickPillar", new Point(1500, -375)),
                new InstData("MovingMegaBlock", new Point(1700, 300)),
                new InstData("MovingMegaBlock2", new Point(1700, -620)),
                new InstData("MovingMegaBlock", new Point(2900, -620)),
                new InstData("MovingMegaBlock2", new Point(2900, 300)),
                new InstData("NormalPlatformHuge", new Point(4100, 100)),
                new InstData("MovingPlatformMedium3", new Point(4450, 275)),
                new InstData("Win", new Point(4500, 575))
            );
            Level("Level2b3", "Second", new Point(580, 300), "Level2b3", 0.33f,
                new InstData("NormalPlatformHuge", new Point(500, 200)),
                new InstData("NormalPlatformHuge", new Point(1200, 200)),
                new InstData("NormalPlatformHuge", new Point(1900, 200)),
                new InstData("NormalPlatformHuge", new Point(2600, 400)),
                new InstData("NormalPlatformHuge", new Point(3300, 600)),
                new InstData("NormalPlatformHuge", new Point(4000, 800)),
                new InstData("NormalPlatformHuge", new Point(4700, 1000)),
                new InstData("NormalPlatformHuge", new Point(5500, 1200)),
                new InstData("NormalPlatformHuge", new Point(6300, 1400)),
                new InstData("NormalPlatformHuge", new Point(7100, 1600)),
                new InstData("NormalPlatformHuge", new Point(7900, 1800)),
                new InstData("NormalPlatformHuge", new Point(8700, 2000)),
                new InstData("MegaBlock", new Point(10000, -650)),
                new InstData("Win", new Point(10500, 175))
            );

            //LevelSet 3aa (Double Jump, Bounce) (COMPLETE)
            Level("Level3aa1", null, new Point(20, 100), "Level3aa2", 1,
                new InstData("Floor", new Point(0, 0)),
                new InstData("NormalBlockMedium", new Point(350, 100)),
                new InstData("NormalBlockMediumStacked", new Point(450, 100)),
                new InstData("BouncyHuge", new Point(600, 25)),
                new InstData("ThickPillarLarge", new Point(850, 0)),
                new InstData("NormalBlockMediumStacked", new Point(900, 0)),
                new InstData("NormalBlockMedium", new Point(1000, 0)),
                new InstData("BouncyLarge", new Point(1100, 350)),
                new InstData("BouncyLarge", new Point(1300, 350)),
                new InstData("BouncyLarge", new Point(1500, 350)),
                new InstData("BouncyLarge", new Point(1700, 350)),
                new InstData("BouncyLarge", new Point(1900, 350)),
                new InstData("ThickPillarLarge", new Point(2100, 0)),
                new InstData("BouncyLarge", new Point(2700, 0)),
                new InstData("BouncyLarge", new Point(2900, 0)),
                new InstData("BouncyLarge", new Point(3050, 0)),
                new InstData("BouncyLarge", new Point(3200, 0)),
                new InstData("BouncyLarge", new Point(3350, 0)),
                new InstData("BouncyLarge", new Point(3500, 0)),
                new InstData("BouncyMedium", new Point(2988, 125)),
                new InstData("BouncyMedium", new Point(3138, 125)),
                new InstData("BouncyMedium", new Point(3288, 125)),
                new InstData("BouncyMedium", new Point(3438, 125)),
                new InstData("NormalPlatformLarge", new Point(3700, 0)),
                new InstData("Win", new Point(3775, 25))
            );

            Level("Level3aa2", null, new Point(20, 100), "Level3aa3", 1,
                new InstData("NormalPlatformHuge", new Point(0, 0)),
                new InstData("NormalBlockLarge", new Point(150, 25)),
                new InstData("BouncyHuge", new Point(400, 25)),
                new InstData("NormalBlockLargeStacked", new Point(700, -150)),
                new InstData("BouncyHuge", new Point(1000, 25)),
                new InstData("NormalBlockLargeStacked", new Point(1300, -50)),
                new InstData("BouncyHuge", new Point(1600, 25)),
                new InstData("NormalBlockLargeStacked", new Point(1900, 0)),
                new InstData("BouncyHuge", new Point(2200, 25)),
                new InstData("NormalBlockLargeStacked2", new Point(2500, -150)),
                new InstData("BouncyHugeMoving", new Point(2800, 150)),
                new InstData("NormalBlockLargeStacked2", new Point(3100, -150)),
                new InstData("BouncyHugeMoving2", new Point(3375, 100)),
                new InstData("BouncyHugeMoving3", new Point(3650, 100)),
                new InstData("NormalBlockLargeStacked2", new Point(3950, -150)),
                new InstData("BouncyMedium", new Point(4425, 100)),
                new InstData("BouncyMedium", new Point(4700, 100)),
                new InstData("BouncyMedium", new Point(4925, 100)),
                new InstData("BouncyMedium", new Point(5125, 100)),
                new InstData("BouncyMedium", new Point(5300, 100)),
                new InstData("BouncyHugeMoving4", new Point(5475, 300)),
                new InstData("BouncyHuge", new Point(5775, 100)),
                new InstData("Win", new Point(5862, 400))
            );

            Level("Level3aa3", "Third", new Point(300, 300), "Level3aa3", 1,
                new InstData("BouncyHugeMoving5", new Point(200, 200)),
                new InstData("NormalBlockSmall", new Point(500, 225)),
                new InstData("NormalBlockSmall", new Point(800, 225)),
                new InstData("NormalBlockSmall", new Point(800, 350)),
                new InstData("MovingBlockSmallBouncy", new Point(1100, 225)),
                new InstData("MovingBlockSmallBouncy2", new Point(1300, 225)),
                new InstData("BouncyPillar", new Point(1600, 275)),
                new InstData("BouncyPillar", new Point(1800, 275)),
                new InstData("BouncyPillar", new Point(1950, 275)),
                new InstData("BouncyPillar", new Point(2150, 276)),
                new InstData("MovingBlockSmallBouncy3", new Point(4500, 225)),
                new InstData("MovingBlockSmallBouncy3", new Point(4500, 325)),
                new InstData("MovingBlockSmallBouncy3", new Point(4500, 425)),
                new InstData("MovingBlockSmallBouncy3", new Point(4700, 350)),
                new InstData("MovingBlockSmallBouncy3", new Point(4700, 450)),
                new InstData("MovingBlockSmallBouncy3", new Point(4700, 250)),
                new InstData("MovingBlockSmallBouncy3", new Point(4900, 375)),
                new InstData("MovingBlockSmallBouncy3", new Point(4900, 475)),
                new InstData("MovingBlockSmallBouncy3", new Point(4900, 275)),
                new InstData("MovingBlockSmallBouncy3", new Point(5100, 350)),
                new InstData("MovingBlockSmallBouncy3", new Point(5100, 450)),
                new InstData("MovingBlockSmallBouncy3", new Point(5100, 250)),
                new InstData("MovingBlockSmallBouncy3", new Point(5300, 225)),
                new InstData("MovingBlockSmallBouncy3", new Point(5300, 325)),
                new InstData("MovingBlockSmallBouncy3", new Point(5300, 425)),
                new InstData("MovingBlockSmallBouncy3", new Point(5500, 350)),
                new InstData("MovingBlockSmallBouncy3", new Point(5500, 450)),
                new InstData("MovingBlockSmallBouncy3", new Point(5500, 250)),
                new InstData("MovingThickPillar", new Point(3350, -175)),
                new InstData("MovingThickPillar2", new Point(3800, -175)),
                new InstData("MovingThickPillar", new Point(4300, -175)),
                new InstData("NormalBlockMedium", new Point(4650, 225)),
                new InstData("NormalBlockMediumStacked", new Point(5100, 225)),
                new InstData("NormalBlockMediumStacked", new Point(5650, 225)),
                new InstData("Win", new Point(6150, 500))
            );

            //LevelSet 3ab (Double Jump, Ice) (COMPLETE)
            //3 Levels
            Level("Level3ab1", null, new Point(20, 100), "Level3ab2", 1,
                new InstData("Floor", new Point(0, 0)),
                new InstData("IcyLarge", new Point(200, 25)),
                new InstData("IcyLarge", new Point(550, 175)),
                new InstData("IcyLarge", new Point(900, 325)),
                new InstData("IcySmall", new Point(1200, 325)),
                new InstData("IcySmall", new Point(1400, 325)),
                new InstData("IcySmall", new Point(1750, 325)),
                new InstData("ThickPillarLarge", new Point(1950, 0)),
                new InstData("IcyLarge", new Point(2700, 0)),
                new InstData("IcyLarge", new Point(3100, 0)),
                new InstData("IcyLarge", new Point(3500, 0)),
                new InstData("IcyThickPillarSmall", new Point(3800, 0)),
                new InstData("IcyThickPillarMedium", new Point(4100, 0)),
                new InstData("IcyThickPillarLarge", new Point(4400, 0)),
                new InstData("IcyBlockLarge", new Point(4600, 0)),
                new InstData("IcyBlockLargeStacked", new Point(4800, 0)),
                new InstData("IcyBlockLarge", new Point(5000, 0)),
                new InstData("Win", new Point(4888, 400))
            );
            Level("Level3ab2", null, new Point(20, 100), "Level3ab3", 1,
                new InstData("IcyLarge", new Point(0, 0)),
                new InstData("IcyLarge", new Point(400, 0)),
                new InstData("IcyLarge", new Point(800, 0)),
                new InstData("IcyLarge", new Point(1200, 0)),
                new InstData("IcyLarge", new Point(1600, 0)),
                new InstData("IcyLarge", new Point(2000, 0)),
                new InstData("IcyLarge", new Point(2400, 0)),
                new InstData("IcyBlockLarge", new Point(2700, 0)),
                new InstData("IcyBlockLargeStacked", new Point(2900, -25)),
                new InstData("IcyBlockLargeStacked2", new Point(3100, -50)),
                new InstData("IcyBlockLargeStacked3", new Point(3300, -75)),
                new InstData("IcyBlockLargeStacked3", new Point(3500, -75)),
                new InstData("IcyBlockLargeStacked2", new Point(3700, -50)),
                new InstData("IcyBlockLargeStacked2", new Point(3900, -50)),
                new InstData("IcyBlockLargeStacked", new Point(4100, -25)),
                new InstData("IcyThinPillarLarge", new Point(4500, 0)),
                new InstData("IcyThinPillarMedium", new Point(4700, 0)),
                new InstData("IcyThinPillarLarge", new Point(4900, 0)),
                new InstData("IcyThinPillarSmall", new Point(5100, 0)),
                new InstData("IcyThinPillarSmall", new Point(5300, 0)),
                new InstData("IcyThinPillarLarge", new Point(5500, 0)),
                new InstData("IcyThinPillarLarge", new Point(5800, 0)),
                new InstData("IcyLargeMoving", new Point(6100, 25)),
                new InstData("IcyLargeMoving2", new Point(6600, 25)),
                new InstData("IcyLargeMoving", new Point(7100, 25)),
                new InstData("IcyLargeMoving2", new Point(7600, 25)),
                new InstData("IcyLargeMoving", new Point(8100, 25)),
                new InstData("IcyLargeMoving2", new Point(8600, 25)),
                new InstData("Win", new Point(8750, 50))
            );
            Level("Level3ab3", "Third", new Point(20, 100), "Level3ab3", 1,
                new InstData("IcyFloor", new Point(0, 0)),
                new InstData("IcySmall", new Point(800, 25)),
                new InstData("IcySmall", new Point(1000, 25)),
                new InstData("IcySmall", new Point(1200, 25)),
                new InstData("IcySmall", new Point(1400, 25)),
                new InstData("IcySmall", new Point(1600, 25)),
                new InstData("IcyLarge", new Point(2100, 150)),
                new InstData("IcyLarge", new Point(2350, 300)),
                new InstData("IcyLarge", new Point(2600, 450)),
                new InstData("IcyLarge", new Point(2850, 600)),
                new InstData("IcyLarge", new Point(3100, 750)),
                new InstData("IcyLarge", new Point(3350, 900)),
                new InstData("IcyLarge", new Point(3600, 1050)),
                new InstData("IcyLarge", new Point(3850, 900)),
                new InstData("IcyLarge", new Point(4100, 750)),
                new InstData("IcyLarge", new Point(4350, 600)),
                new InstData("IcyLarge", new Point(4600, 450)),
                new InstData("IcyLarge", new Point(4850, 300)),
                new InstData("IcyLarge", new Point(5100, 150)),
                new InstData("IcyBlockLargeStacked", new Point(2975, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(3175, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(3375, 0)),
                new InstData("IcyBlockLargeStacked3", new Point(3575, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(3775, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(3975, 0)),
                new InstData("IcyBlockLargeStacked", new Point(4125, 0)),
                new InstData("IcySmallMoving", new Point(5450, 150)),
                new InstData("IcySmallMoving3", new Point(5750, 250)),
                new InstData("IcySmallMoving", new Point(6000, 350)),
                new InstData("IcySmallMoving3", new Point(6250, 450)),
                new InstData("IcySmallMoving4", new Point(6250, 750)),
                new InstData("IcySmallMoving3", new Point(6250, 1050)),
                new InstData("IcySmallMoving4", new Point(6250, 1350)),
                new InstData("IcySmallMoving2", new Point(6450, 1600)),
                new InstData("IcySmallMoving", new Point(6050, 1600)),
                new InstData("IcySmallMoving", new Point(6450, 1750)),
                new InstData("IcySmallMoving2", new Point(6050, 1750)),
                new InstData("IcySmallMoving4", new Point(6250, 2000)),
                new InstData("IcySmallMoving2", new Point(6450, 2250)),
                new InstData("IcySmallMoving", new Point(6050, 2250)),
                new InstData("IcySmallMoving", new Point(6450, 2400)),
                new InstData("IcySmallMoving2", new Point(6050, 2400)),
                new InstData("IcySmallMoving2", new Point(6450, 2550)),
                new InstData("IcySmallMoving", new Point(6050, 2550)),
                new InstData("IcySmallMoving", new Point(6450, 2700)),
                new InstData("IcySmallMoving2", new Point(6050, 2700)),
                new InstData("IcySmallMoving4", new Point(6250, 2950)),
                new InstData("Win", new Point(6262, 3250))
            );

            //LevelSet 3ba (Dash, Bounce) COMPLETE
            //3 Levels
            Level("Level3ba1", null, new Point(20, 100), "Level3ba2", 1f,
                new InstData("NormalPlatformHuge2", new Point(0, 0)),
                new InstData("BouncySmall", new Point(150, 25)),
                new InstData("ThickBouncySmall", new Point(275, 25)),
                new InstData("ThickPillarSmall", new Point(400, 0)),
                new InstData("ThickPillarMedium", new Point(600, -50)),
                new InstData("ThickPillarMedium", new Point(800, 0)),
                new InstData("ThickPillarLarge", new Point(1000, -50)),
                new InstData("ThickPillarLarge", new Point(1200, 0)),
                new InstData("MovingPlatformMedium", new Point(1350, 325)),
                new InstData("MovingPlatformMedium2", new Point(1600, 375)),
                new InstData("ThickPillarHuge", new Point(1800, -225)),
                new InstData("NormalBlockSmall", new Point(2075, 550)),
                new InstData("NormalBlockSmall", new Point(2100, 500)),
                new InstData("NormalBlockSmall", new Point(2125, 450)),
                new InstData("ThickPillarHuge", new Point(2150, 450)),
                new InstData("NormalBlockSmall", new Point(2150, 400)),
                new InstData("NormalBlockSmall", new Point(2175, 450)),
                new InstData("NormalBlockSmall", new Point(2200, 500)),
                new InstData("NormalBlockSmall", new Point(2225, 550)),
                new InstData("ThickBouncyHuge2", new Point(1850, 0)),
                new InstData("NormalPlatformLarge", new Point(2150, 0)),
                new InstData("NormalPlatformHuge2", new Point(2300, 0)),
                new InstData("BouncySmall", new Point(50 + 2300, 25)),
                new InstData("BouncySmall", new Point(150 + 2300, 100)),
                new InstData("BouncySmall", new Point(250 + 2300, 175)),
                new InstData("BouncySmall", new Point(350 + 2300, 250)),
                new InstData("BouncySmall", new Point(450 + 2300, 325)),
                new InstData("ThickPillarLarge", new Point(450 + 2300, 25)),
                new InstData("BouncySmall", new Point(650 + 2300, 320)),
                new InstData("ThickPillarLarge", new Point(650 + 2300, 20)),
                new InstData("BouncySmall", new Point(850 + 2300, 315)),
                new InstData("ThickPillarLarge", new Point(850 + 2300, 15)),
                new InstData("BouncySmall", new Point(1050 + 2300, 310)),
                new InstData("ThickPillarLarge", new Point(1050 + 2300, 10)),
                new InstData("BouncySmall", new Point(1250 + 2300, 305)),
                new InstData("ThickPillarLarge", new Point(1250 + 2300, 5)),
                new InstData("BouncySmall", new Point(1450 + 2300, 300)),
                new InstData("ThickPillarLarge", new Point(1450 + 2300, 0)),
                new InstData("ThickBouncyHuge", new Point(1600 + 2300, 0)),
                new InstData("NormalPlatformHuge", new Point(1900 + 2300, 0)),
                new InstData("Win", new Point(2050 + 2300, 25))
            );
            Level("Level3ba2", null, new Point(20, 800), "Level3ba3", 1f,
                new InstData("NormalPlatformHuge", new Point(0, 700)),
                new InstData("ThickPillarLarge", new Point(500, 200)),
                new InstData("ThickBouncySmall", new Point(500, 500)),
                new InstData("ThickPillarLarge", new Point(750, 300)),
                new InstData("ThickBouncySmall", new Point(750, 600)),
                new InstData("ThickPillarLarge", new Point(1000, 250)),
                new InstData("ThickBouncySmall", new Point(1000, 550)),
                new InstData("ThickPillarLarge", new Point(1250, 200)),
                new InstData("ThickBouncySmall", new Point(1250, 500)),
                new InstData("ThickPillarLarge", new Point(1500, 150)),
                new InstData("ThickBouncySmall", new Point(1500, 450)),
                new InstData("MovingThickPillar", new Point(1600, 600)),
                new InstData("MovingThickPillar2", new Point(1600, 0)),
                new InstData("ThickPillarLarge", new Point(1700, 150)),
                new InstData("ThickBouncySmall", new Point(1700, 450)),
                new InstData("MovingThickPillar", new Point(1800, 600)),
                new InstData("MovingThickPillar2", new Point(1800, 0)),
                new InstData("ThickPillarLarge", new Point(1900, 150)),
                new InstData("ThickBouncySmall", new Point(1900, 450)),
                new InstData("BouncyHugeMoving3", new Point(2150, 450)),
                new InstData("BouncyHugeMoving2", new Point(2650, 250)),
                new InstData("NormalPlatformHuge", new Point(3100, 0)),
                new InstData("Win", new Point(3300, 25))
            );
            Level("Level3ba3", "Third", new Point(40, 5100), "Level3ba3", 1f,
                new InstData("NormalPlatformHuge", new Point(0, 5000)),
                new InstData("ThickPillarHuge2", new Point(550, 3600)),
                new InstData("ThickBouncySmall", new Point(550, 4800)),
                new InstData("ThickPillarHuge2", new Point(850, 3400)),
                new InstData("ThickBouncySmall", new Point(850, 4600)),
                new InstData("ThickPillarHuge2", new Point(1200, 3200)),
                new InstData("ThickBouncySmall", new Point(1200, 4400)),
                new InstData("ThickPillarHuge2", new Point(1575, 3000)),
                new InstData("ThickBouncySmall", new Point(1575, 4200)),
                new InstData("ThickPillarHuge2", new Point(2025, 2200)),
                new InstData("ThickBouncySmall", new Point(2025, 3400)),
                new InstData("ThickPillarHuge2", new Point(2350, 2200)),
                new InstData("ThickBouncySmall", new Point(2350, 3400)),
                new InstData("ThickPillarHuge2", new Point(2650, 2200)),
                new InstData("ThickBouncySmall", new Point(2650, 3400)),
                new InstData("ThickPillarHuge2", new Point(2950, 2200)),
                new InstData("ThickBouncySmall", new Point(2950, 3400)),
                new InstData("ThickPillarHuge2", new Point(3150, 2200)),
                new InstData("ThickBouncySmall", new Point(3150, 3400)),
                new InstData("ThickPillarHuge2", new Point(3300, 2200)),
                new InstData("ThickBouncySmall", new Point(3300, 3400)),
                new InstData("ThickPillarHuge2", new Point(3500, 2000)),
                new InstData("ThickBouncySmall", new Point(3500, 3200)),
                new InstData("ThickPillarHuge2", new Point(3700, 2200)),
                new InstData("ThickBouncySmall", new Point(3700, 3400)),
                new InstData("ThickPillarHuge2", new Point(3900, 2000)),
                new InstData("ThickBouncySmall", new Point(3900, 3200)),
                new InstData("ThickPillarHuge2", new Point(4150, 1800)),
                new InstData("ThickBouncySmall", new Point(4150, 3000)),
                new InstData("ThickPillarHuge2", new Point(4450, 1400)),
                new InstData("ThickBouncySmall", new Point(4450, 2600)),
                new InstData("ThickPillarHuge2", new Point(4750, 1000)),
                new InstData("ThickBouncySmall", new Point(4750, 2200)),
                new InstData("ThickPillarHuge2", new Point(5050, 600)),
                new InstData("ThickBouncySmall", new Point(5050, 1800)),
                new InstData("ThickPillarHuge2", new Point(5400, 0)),
                new InstData("ThickBouncySmall", new Point(5400, 1200)),
                new InstData("ThickPillarHuge2", new Point(5725, 0)),
                new InstData("ThickBouncySmall", new Point(5725, 1200)),
                new InstData("ThickPillarHuge2", new Point(6150, -600)),
                new InstData("ThickBouncySmall", new Point(6150, 600)),
                new InstData("NormalPlatformHuge", new Point(6500, 0)),
                new InstData("Win", new Point(6700, 25))
            );

            //LevelSet 3d (Dash, Ice) COMPLETE
            //3 Levels
            Level("Level3bb1", null, new Point(20, 100), "Level3bb2", 1,
                new InstData("IcyFloor", new Point(0, 0)),
                new InstData("IcyThickPillarMoving", new Point(500, 50)),
                new InstData("IcyThickPillarMoving", new Point(550, 50)),
                new InstData("IcyThickPillarMoving", new Point(600, 50)),
                new InstData("IcyThickPillarMoving", new Point(650, 50)),
                new InstData("IcyThickPillarMoving", new Point(700, 50)),
                new InstData("IcyThickPillarMoving", new Point(750, 50)),
                new InstData("IcyThickPillarMoving", new Point(800, 50)),
                new InstData("IcyThickPillarMoving", new Point(850, 50)),
                new InstData("IcyThickPillarMoving", new Point(900, 50)),
                new InstData("IcyThickPillarMoving", new Point(950, 50)),
                new InstData("IcyThickPillarMoving", new Point(1000, 50)),
                new InstData("IcyThickPillarMoving", new Point(1050, 50)),
                new InstData("IcyThickPillarMoving", new Point(1100, 50)),
                new InstData("IcyThickPillarMoving", new Point(1150, 50)),
                new InstData("IcyThickPillarMoving", new Point(1200, 50)),
                new InstData("IcyThickPillarMoving", new Point(1250, 50)),
                new InstData("IcyThickPillarMoving", new Point(1600, 50)),
                new InstData("IcyThickPillarMoving", new Point(1700, 50)),
                new InstData("IcyThickPillarMoving", new Point(1800, 50)),
                new InstData("IcyThickPillarMoving", new Point(1900, 50)),
                new InstData("IcyThickPillarMoving", new Point(2000, 50)),
                new InstData("IcyThickPillarMoving", new Point(2100, 50)),
                new InstData("IcyThickPillarMoving", new Point(2200, 50)),
                new InstData("IcyThickPillarMoving", new Point(2300, 50)),
                new InstData("IcyThickPillarMoving", new Point(2400, 50)),
                new InstData("IcyThickPillarMoving", new Point(2500, 50)),
                new InstData("IcyThickPillarMoving", new Point(2600, 50)),
                new InstData("IcyThickPillarMoving", new Point(2700, 50)),
                new InstData("IcyThickPillarMoving", new Point(2800, 50)),
                new InstData("IcyThickPillarMoving", new Point(2900, 50)),
                new InstData("IcyThickPillarMoving", new Point(3000, 50)),
                new InstData("IcyThickPillarMoving", new Point(3100, 50)),
                new InstData("IcyThickPillarMoving2", new Point(3700, 50)),
                new InstData("IcyThickPillarMoving", new Point(3850, 50)),
                new InstData("IcyThickPillarMoving2", new Point(4000, 50)),
                new InstData("IcyThickPillarMoving", new Point(4150, 50)),
                new InstData("IcyThickPillarMoving2", new Point(4300, 50)),
                new InstData("IcyHuge2", new Point(4700, 0)),
                new InstData("IcyHuge2", new Point(5300, 0)),
                new InstData("IcyHuge2", new Point(5900, 0)),
                new InstData("IcyHuge2", new Point(6500, 0)),
                new InstData("IcyHuge2", new Point(7100, 0)),
                new InstData("IcyHuge2", new Point(7650, 100)),
                new InstData("IcyHuge2", new Point(7100, 200)),
                new InstData("IcyHuge2", new Point(7650, 300)),
                new InstData("IcyHuge2", new Point(7100, 400)),
                new InstData("IcyHuge2", new Point(7650, 500)),
                new InstData("IcyHuge2", new Point(7100, 600)),
                new InstData("IcyHuge2", new Point(7650, 700)),
                new InstData("Win", new Point(8000, 725))
            );
            Level("Level3bb2", null, new Point(20, 100), "Level3bb3", 1,
                new InstData("IcyHuge", new Point(0, 0)),
                new InstData("IcyCube", new Point(225, 0)),
                new InstData("IcyCube", new Point(275, 0)),
                new InstData("IcyCube", new Point(325, 0)),
                new InstData("IcyCube", new Point(375, 0)),
                new InstData("IcyCube", new Point(425, 0)),
                new InstData("IcyCube", new Point(475, 0)),
                new InstData("IcyCube", new Point(525, 0)),
                new InstData("IcyCube", new Point(575, 0)),
                new InstData("IcyCube", new Point(625, 0)),
                new InstData("IcyCube", new Point(675, 0)),
                new InstData("IcyCube", new Point(925, 0)),
                new InstData("IcyCube", new Point(975, 0)),
                new InstData("IcyCube", new Point(1025, 0)),
                new InstData("IcyCube", new Point(1075, 0)),
                new InstData("IcyCube", new Point(1125, 0)),
                new InstData("IcyCube", new Point(1175, 0)),
                new InstData("IcyCube", new Point(1225, 0)),
                new InstData("IcyCube", new Point(1275, 0)),
                new InstData("IcyCube", new Point(1325, 0)),
                new InstData("IcyCube", new Point(1375, 0)),
                new InstData("IcyThinPillarSmall", new Point(1625, 0)),
                new InstData("IcyThinPillarSmall", new Point(1675, 0)),
                new InstData("IcyThinPillarSmall", new Point(1725, 0)),
                new InstData("IcyThinPillarSmall", new Point(1775, 0)),
                new InstData("IcyThinPillarSmall", new Point(1825, 0)),
                new InstData("IcyThinPillarSmall", new Point(1875, 0)),
                new InstData("IcyThinPillarSmall", new Point(1925, 0)),
                new InstData("IcyThinPillarSmall", new Point(1975, 0)),
                new InstData("IcyThinPillarSmall", new Point(2025, 0)),
                new InstData("IcyThinPillarSmall", new Point(2075, 0)),
                new InstData("IcyThinPillarMedium", new Point(2325, 0)),
                new InstData("IcyThinPillarMedium", new Point(2375, 0)),
                new InstData("IcyThinPillarMedium", new Point(2425, 0)),
                new InstData("IcyThinPillarMedium", new Point(2475, 0)),
                new InstData("IcyThinPillarMedium", new Point(2525, 0)),
                new InstData("IcyThinPillarMedium", new Point(2575, 0)),
                new InstData("IcyThinPillarMedium", new Point(2625, 0)),
                new InstData("IcyThinPillarMedium", new Point(2675, 0)),
                new InstData("IcyThinPillarMedium", new Point(2725, 0)),
                new InstData("IcyThinPillarMedium", new Point(2775, 0)),
                new InstData("IcyThinPillarLarge", new Point(3025, 0)),
                new InstData("IcyThinPillarLarge", new Point(3075, 0)),
                new InstData("IcyThinPillarLarge", new Point(3125, 0)),
                new InstData("IcyThinPillarLarge", new Point(3175, 0)),
                new InstData("IcyThinPillarLarge", new Point(3225, 0)),
                new InstData("IcyThinPillarLarge", new Point(3275, 0)),
                new InstData("IcyThinPillarLarge", new Point(3325, 0)),
                new InstData("IcyThinPillarLarge", new Point(3375, 0)),
                new InstData("IcyThinPillarLarge", new Point(3425, 0)),
                new InstData("IcyThinPillarLarge", new Point(3475, 0)),
                new InstData("IcyHuge2", new Point(3700, 100)),
                new InstData("IcyHuge2", new Point(4225, 100)),
                new InstData("IcyThinPillarLarge", new Point(4200, 100)),
                new InstData("IcyThinPillarLarge", new Point(4100, -25)),
                new InstData("IcyHuge2", new Point(4125, 0)),
                new InstData("IcyHuge2", new Point(4625, 100)),
                new InstData("IcyHuge2", new Point(5025, 100)),
                new InstData("IcyHuge2", new Point(5425, 100)),
                new InstData("IcyHuge2", new Point(5825, 100)),
                new InstData("IcyHuge2", new Point(4675, 0)),
                new InstData("IcyHuge2", new Point(5225, 0)),
                new InstData("IcyCube", new Point(5775, 0)),
                new InstData("IcyHuge2", new Point(5950, 0)),
                new InstData("IcyThinPillarHuge", new Point(6350, 0)),
                new InstData("IcyCube", new Point(6000, 150)),
                new InstData("IcyThinPillarMedium", new Point(5800, 150)),
                new InstData("IcyThinPillarMedium", new Point(5600, 150)),
                new InstData("IcyThinPillarMedium", new Point(5400, 150)),
                new InstData("IcyThinPillarMedium", new Point(5200, 150)),
                new InstData("IcyThinPillarMedium", new Point(5000, 150)),
                new InstData("IcyThinPillarLarge", new Point(4800, 150)),
                new InstData("IcyThinPillarLarge", new Point(4600, 150)),
                new InstData("IcyThinPillarLarge", new Point(4400, 100)),
                new InstData("Win", new Point(4300, 125))
            );
            Level("Level3bb3", "Third", new Point(20, 100), "Level3bb3", 1,
                new InstData("IcyHuge", new Point(0, 0)),
                new InstData("IcyLargeMoving", new Point(525, 0)),
                new InstData("IcyLargeMoving2", new Point(1075, 0)),
                new InstData("IcyLargeMoving4", new Point(1500, 150)),
                new InstData("IcyLargeMoving3", new Point(1900, 250)),
                new InstData("IcyLargeMoving4", new Point(2300, 350)),
                new InstData("IcyLargeMoving3", new Point(2700, 450)),
                new InstData("IcyThickPillarHuge", new Point(3000, -50)),
                new InstData("IcyThickPillarHuge", new Point(3250, -50)),
                new InstData("IcyThickPillarHuge", new Point(3500, -50)),
                new InstData("IcyThickPillarHuge", new Point(3750, -50)),
                new InstData("IcyThickPillarHuge", new Point(4000, -50)),
                new InstData("IcyThickPillarHuge", new Point(4250, -50)),
                new InstData("IcyThickPillarMoving3", new Point(3000, 650)),
                new InstData("IcyThickPillarMoving4", new Point(3250, 650)),
                new InstData("IcyThickPillarMoving3", new Point(3500, 650)),
                new InstData("IcyThickPillarMoving4", new Point(3750, 650)),
                new InstData("IcyThickPillarMoving3", new Point(4000, 650)),
                new InstData("IcyThickPillarMoving4", new Point(4250, 650)),
                new InstData("IcyLargeMoving", new Point(4500, 500)),
                new InstData("IcyThinPillarHuge", new Point(5000, 0)),
                new InstData("IcyThinPillarMoving", new Point(5200, 0)),
                new InstData("IcyThinPillarHuge", new Point(5400, 0)),
                new InstData("IcyHuge", new Point(5600, 300)),
                new InstData("Win", new Point(5750, 325))
            );
            //LevelSet 4aaa (Double Jump, Bounce, Wall Jump) (COMPLETE)

            Level("Level4aaa1", null, new Point(20, 100), "", 1,
                new InstData("Floor", new Point(0, 0)),
                new InstData("ThickPillarHuge", new Point(450, 75)),
                new InstData("BouncyHuge", new Point(600, 25)),
                new InstData("ThickPillarHuge", new Point(900, 400)),
                new InstData("ThickPillarMedium", new Point(1300, 725)),
                new InstData("ThickPillarHuge", new Point(1700, 400)),
                new InstData("Floor", new Point(1700, 400)),
                new InstData("BouncyHuge", new Point(1900, 425)),
                new InstData("MovingThickPillar", new Point(2250, 725)),
                new InstData("MovingThickPillar2", new Point(2250, 1475)),
                new InstData("ThickPillarHuge", new Point(2600, 1800)),
                new InstData("ThickPillarHuge", new Point(2975, 1800)),
                new InstData("MovingPlatformLarge", new Point(3125, 1600)),
                new InstData("ThickPillarHuge", new Point(3425, 1800)),
                new InstData("ThickPillarHuge", new Point(3825, 1800)),
                new InstData("NormalPlatformHuge", new Point(3875, 1800)),
                new InstData("MovingPillarMedium", new Point(4500, 1800)),
                new InstData("MovingPillarMedium2", new Point(5000, 1800)),
                new InstData("NormalPlatformHuge", new Point(5375, 1800)),
                new InstData("NormalPlatformHuge", new Point(5925, 1800)),
                new InstData("NormalPlatformHuge", new Point(5950, 2575)),
                new InstData("NormalPlatformHuge", new Point(6150, 2575)),
                new InstData("NormalPlatformHuge", new Point(6350, 2575)),
                new InstData("ThickPillarHuge", new Point(5950, 2000)),
                new InstData("ThickPillarLarge2", new Point(6225, 2175)),
                new InstData("ThickPillarLarge2", new Point(6375, 2175)),
                new InstData("ThickPillarHuge", new Point(6650, 2000)),
                new InstData("BouncyHuge", new Point(6225, 1800)),
                new InstData("NormalPlatformHuge", new Point(6425, 1800)),
                new InstData("Win", new Point(6312, 2500))
            );
            //LevelSet 4aab (Double Jump, Bounce, Float) (COMPLETE)

            Level("Level4aab1", null, new Point(20, 600), "", 1,
                new InstData("NormalPlatformHuge", new Point(0, 500)),
                new InstData("BouncyHuge", new Point(500, 200)),
                new InstData("NormalPlatformHuge", new Point(850, 500)),
                new InstData("NormalPlatformHuge", new Point(1550, 500)),
                new InstData("NormalPlatformHuge", new Point(1750, 500)),
                new InstData("ThickPillarHuge", new Point(2125, 550)),
                new InstData("ThickPillarHuge", new Point(2225, 550)),
                new InstData("NormalPlatformHuge", new Point(2350, 500)),
                new InstData("NormalPlatformHuge", new Point(2550, 500)),
                new InstData("NormalPlatformHuge", new Point(2750, 700)),
                new InstData("NormalPlatformHuge", new Point(2950, 900)),
                new InstData("NormalPlatformHuge", new Point(3150, 1100)),
                new InstData("NormalPlatformHuge", new Point(3350, 1300)),
                new InstData("NormalPlatformHuge", new Point(3550, 1500)),
                new InstData("BouncyHuge", new Point(4000, 1200)),
                new InstData("BouncyHuge", new Point(4500, 1000)),
                new InstData("BouncyHuge", new Point(5000, 800)),
                new InstData("BouncyHuge", new Point(5500, 600)),
                new InstData("BouncyHuge", new Point(6000, 400)),
                new InstData("BouncyHuge", new Point(6500, 200)),
                new InstData("ThickPillarHuge", new Point(6800, -200)),
                new InstData("Win", new Point(7300, 300))
            );
            //LevelSet 4aba (Double Jump, Ice, Wall Jump) (COMPLETE)
            Level("Level4aba1", null, new Point(20, 100), "", 1,
                new InstData("IcyFloor", new Point(0, 0)),
                new InstData("IcyThickPillarHuge", new Point(450, 0)),
                new InstData("IcyThickPillarHuge2", new Point(850, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(2800, 0)),
                new InstData("IcyBlockLargeStacked3", new Point(3000, 0)),
                new InstData("IcyBlockLargeStacked5", new Point(3200, 0)),
                new InstData("IcyBlockLargeStacked7", new Point(3400, 0)),
                new InstData("IcyBlockLargeStacked6", new Point(3600, 0)),
                new InstData("IcyBlockLargeStacked4", new Point(3800, 0)),
                new InstData("IcyBlockLargeStacked2", new Point(4000, 0)),
                new InstData("IcyThickPillarHuge2", new Point(3400, 1750)),
                new InstData("IcyThickPillarHuge2", new Point(3550, 1750)),
                new InstData("IcyThickPillarHuge2", new Point(3475, 3000)),
                new InstData("IcyThickPillarLarge", new Point(3750, 4200)),
                new InstData("IcyThickPillarLarge", new Point(4000, 4200)),
                new InstData("IcyThickPillarLarge", new Point(4300, 4200)),
                new InstData("IcyThickPillarLarge", new Point(4650, 4200)),
                new InstData("IcyThickPillarLarge", new Point(5050, 4200)),
                new InstData("IcyHuge", new Point(5250, 4300)),
                new InstData("IcyHuge", new Point(5750, 4300)),
                new InstData("IcyThickPillarHuge2", new Point(5700, 4500)),
                new InstData("IcyThickPillarHuge2", new Point(5950, 4500)),
                new InstData("IcyThickPillarLarge", new Point(5700, 5875)),
                new InstData("IcyThickPillarLarge", new Point(5950, 6275)),
                new InstData("IcyThickPillarLarge", new Point(5700, 6675)),
                new InstData("IcyThickPillarLarge", new Point(5950, 7075)),
                new InstData("IcyThickPillarLarge", new Point(5700, 7475)),
                new InstData("IcyThickPillarLarge", new Point(5950, 7875)),
                new InstData("IcyThickPillarLarge", new Point(5700, 8275)),
                new InstData("IcyThickPillarLarge", new Point(5950, 8675)),
                new InstData("IcyThickPillarLarge", new Point(5700, 9075)),
                new InstData("IcyThickPillarLarge", new Point(5950, 9475)),
                new InstData("IcySmall", new Point(6288, 9775)),
                new InstData("Win", new Point(6300, 10000))
            );
            //LevelSet 4abb (Double Jump, Ice, Float) (COMPLETE)
            Level("Level4abb1", null, new Point(20, 100), "", 1,
                new InstData("IcyMedium", new Point(0, 0)),
                new InstData("IcyMedium", new Point(500, 50)),
                new InstData("IcyMedium", new Point(1000, 100)),
                new InstData("IcyMedium", new Point(1400, 200)),
                new InstData("IcyMedium", new Point(1600, 350)),
                new InstData("IcyMedium", new Point(1800, 525)),
                new InstData("IcyThickPillarHuge", new Point(2100, 100)),
                new InstData("IcySmallMoving", new Point(2400, 100)),
                new InstData("IcySmallMoving", new Point(2800, 100)),
                new InstData("IcySmallMoving", new Point(3200, 100)),
                new InstData("IcySmallMoving", new Point(3400, 100)),
                new InstData("IcySmallMoving", new Point(3600, 100)),
                new InstData("IcySmallMoving", new Point(3800, 100)),
                new InstData("IcyThickPillarLarge", new Point(3250, 150)),
                new InstData("IcyThickPillarLarge", new Point(3350, 150)),
                new InstData("IcyThickPillarLarge", new Point(3400, 150)),
                new InstData("IcyThickPillarLarge", new Point(3450, 150)),
                new InstData("IcyThickPillarLarge", new Point(3550, 150)),
                new InstData("IcyThickPillarLarge", new Point(3600, 150)),
                new InstData("IcyThickPillarLarge", new Point(3650, 150)),
                new InstData("IcyThickPillarLarge", new Point(3750, 150)),
                new InstData("IcyMedium", new Point(4300, 100)),
                new InstData("IcyLarge", new Point(4900, 100)),
                new InstData("IcyThickPillarHuge2", new Point(5300, 100)),
                new InstData("IcyHuge", new Point(5350, 100)),
                new InstData("IcyLargeMoving", new Point(4900, 250)),
                new InstData("IcyLargeMoving2", new Point(4900, 425)),
                new InstData("IcyLargeMoving", new Point(4900, 600)),
                new InstData("IcyLargeMoving2", new Point(4900, 775)),
                new InstData("IcyLargeMoving", new Point(4900, 950)),
                new InstData("IcyLargeMoving2", new Point(4900, 1125)),
                new InstData("IcyFloor2", new Point(5300, 1300)),
                new InstData("Win", new Point(5450, 125))
            );

            //LevelSet 4abb (Double Jump, Ice, Float)

            //LevelSet 4baa (Dash, Bounce, Wall Jump)

            //LevelSet 4bab (Dash, Bounce, Float)
            Level("Level4bab", null, new Point(450, 400), "", 1,
                new InstData("NormalPlatformHuge", new Point(300, 300)),
                //Section1
                new InstData("ThinPillarLarge", new Point(350, 150)),
                new InstData("ThinPillarLarge", new Point(350, 0)),
                new InstData("ThinPillarLarge", new Point(525, 150)),
                new InstData("ThinPillarLarge", new Point(525, 0)),
                new InstData("ThickPillarLarge", new Point(850, 0)),
                new InstData("ThickBouncySmall", new Point(850, 300)),
                new InstData("ThickPillarLarge", new Point(1050, 75)),
                new InstData("ThickBouncySmall", new Point(1050, 375)),
                new InstData("ThickPillarLarge", new Point(1250, 150)),
                new InstData("ThickBouncySmall", new Point(1250, 450)),
                new InstData("ThickPillarLarge", new Point(1450, 225)),
                new InstData("ThickBouncySmall", new Point(1450, 525)),
                new InstData("ThickPillarLarge", new Point(1650, 300)),
                new InstData("ThickBouncySmall", new Point(1650, 600)),
                //Section2
                new InstData("abab-Sec2DownPillar", new Point(1900, 0)),
                new InstData("abab-Sec2Plat", new Point(1875, 600)),
                new InstData("abab-Sec2DownPillar", new Point(2100, 0)),
                new InstData("abab-Sec2Plat", new Point(2075, 600)),
                new InstData("abab-Sec2DownPillar", new Point(2300, 0)),
                new InstData("abab-Sec2Plat", new Point(2275, 600)),
                new InstData("abab-Sec2DownPillar", new Point(2500, 0)),
                new InstData("abab-Sec2Plat", new Point(2475, 600)),
                new InstData("abab-Sec2UpPillar", new Point(1950, 900)),
                new InstData("abab-Sec2Plat", new Point(1925, 850)),
                new InstData("abab-Sec2UpPillar", new Point(2150, 900)),
                new InstData("abab-Sec2Plat", new Point(2125, 850)),
                new InstData("abab-Sec2UpPillar", new Point(2350, 900)),
                new InstData("abab-Sec2Plat", new Point(2325, 850)),
                new InstData("abab-Sec2UpPillar", new Point(2550, 900)),
                new InstData("abab-Sec2Plat", new Point(2525, 850)),
                //Section3
                new InstData("abab-Sec3Floor", new Point(2850, 600)),
                new InstData("abab-Sec2DownPillar", new Point(3000, 0)),
                new InstData("abab-Sec2DownPillar", new Point(3200, 0)),
                new InstData("abab-Sec2DownPillar", new Point(3400, 0)),
                new InstData("abab-Sec2DownPillar", new Point(3600, 0)),
                new InstData("abab-Sec2DownPillar", new Point(3800, 0)),
                new InstData("abab-Sec2DownPillar", new Point(4000, 0)),
                new InstData("abab-Sec2DownPillar", new Point(4200, 0)),
                new InstData("abab-Sec3Block", new Point(2850, 750)),
                new InstData("abab-Sec3MBlock", new Point(3025, 650)),
                new InstData("abab-Sec3Roadbump", new Point(3225, 650)),
                new InstData("abab-Sec3Block", new Point(3225, 750)),
                new InstData("abab-Sec3MBlock", new Point(3400, 650)),
                new InstData("abab-Sec3Roadbump", new Point(3600, 650)),
                new InstData("abab-Sec3Block", new Point(3600, 750)),
                new InstData("abab-Sec3MBlock", new Point(3775, 650)),
                new InstData("abab-Sec3Roadbump", new Point(3975, 650)),
                new InstData("abab-Sec2DownPillar", new Point(4310, 0)),
                new InstData("abab-Sec3Block", new Point(3975, 750)),
                new InstData("abab-Sec3MBlock", new Point(4150, 650)),
                new InstData("abab-Sec3Roadbump", new Point(4350, 650)),
                new InstData("abab-Sec3Floor2", new Point(4350, 400)),
                new InstData("abab-Sec3Ceiling2", new Point(4350, 700)),
                //Section 4 (HARD!)
                new InstData("abab-Sec4BounceUp", new Point(4950, 400)),
                new InstData("abab-Sec4BounceDown", new Point(5075, 600)),
                new InstData("abab-Sec4BounceRight", new Point(5200, 350)),
                new InstData("abab-Sec4BounceLeft", new Point(4950, 550)),
                new InstData("abab-Sec4BounceUp", new Point(5225, 600)),
                new InstData("abab-Sec4BounceDown", new Point(5200, 700)),
                new InstData("abab-Sec4BounceUp", new Point(5400, 700)),
                new InstData("abab-Sec4BounceUp", new Point(5550, 800)),
                new InstData("abab-Sec4BounceDown", new Point(5550, 800)),
                new InstData("abab-Sec4BounceRight", new Point(5550, 800)),
                new InstData("abab-Sec4BounceLeft", new Point(5550, 800)),
                new InstData("abab-Sec4BounceUp", new Point(5700, 900)),
                new InstData("abab-Sec4BounceDown", new Point(5700, 900)),
                new InstData("abab-Sec4BounceRight", new Point(5700, 900)),
                new InstData("abab-Sec4BounceLeft", new Point(5700, 900)),
                new InstData("abab-Sec4BounceUp", new Point(5850, 1000)),
                new InstData("abab-Sec4BounceDown", new Point(5850, 1000)),
                new InstData("abab-Sec4BounceRight", new Point(5850, 1000)),
                new InstData("abab-Sec4BounceLeft", new Point(5850, 1000)),
                new InstData("abab-Sec4BounceUp", new Point(6200, 1000)),
                new InstData("abab-Sec4BounceDown", new Point(6200, 1000)),
                new InstData("abab-Sec4BounceRight", new Point(6200, 1000)),
                new InstData("abab-Sec4BounceLeft", new Point(6200, 1000)),
                new InstData("abab-Sec4BounceUp", new Point(6450, 1100)),
                new InstData("abab-Sec4BounceDown", new Point(6450, 1100)),
                new InstData("abab-Sec4BounceRight", new Point(6450, 1100)),
                new InstData("abab-Sec4BounceLeft", new Point(6450, 1100)),
                new InstData("abab-Sec4BounceUp", new Point(6700, 1250)),
                new InstData("abab-Sec4BounceDown", new Point(6700, 1250)),
                new InstData("abab-Sec4BounceRight", new Point(6700, 1250)),
                new InstData("abab-Sec4BounceLeft", new Point(6700, 1250)),
                new InstData("Win", new Point(6712, 1262))
            );
            //LevelSet 4bba (Dash, Ice, Wall Jump)

            //LevelSet 4bbb (Dash, Ice, Float)
        }
    }
}
