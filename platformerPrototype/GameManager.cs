#region using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using platformerPrototype.Core;
using platformerPrototype.Utility;

#endregion

namespace platformerPrototype {
    public class GameManager : Microsoft.Xna.Framework.Game {
        private readonly Color backgroundColor = new Color(0x09, 0x04, 0x11);
        private Boolean _isLargeReso;
        private Texture2D _whiteRect;
        public ChoiceMaker ChoiceMaker;

        public Stage CurrentStage;

        private Double drawTimeStep;

        //Choices
        public String InitialLevel = "Level1";

        public Boolean PlayerChoseBouncy = false;
        public Boolean PlayerChoseIcy = false;
        public Boolean PlayerHasDash = false;
        public Boolean PlayerHasDoubleJump = false;
        public Boolean PlayerHasFloat = false;

        public Boolean PlayerHasWallJump = false;

        public RestraintMessageScreen SorryScreen;

        private Double updateTimeStep;

        public GameManager() {
            GDManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Options = new Options();

            // Apply a standard pre-update resolution.
            GDManager.PreferredBackBufferWidth = Options.RESOLUTION_DEFAULT.X;
            GDManager.PreferredBackBufferHeight = Options.RESOLUTION_DEFAULT.Y;
            GDManager.IsFullScreen = false;
            _isLargeReso = false;
            GDManager.GraphicsProfile = GraphicsProfile.Reach;
            IsMouseVisible = true;
            Window.Title = "Choices";

#if !DEBUG
            InitialLevel = "Level1";
            PlayerHasDoubleJump = false;
            PlayerHasDash = false;
            PlayerHasWallJump = false;
            PlayerHasFloat = false;
            #endif
        }

        public GraphicsDeviceManager GDManager { get; internal set; }
        public SpriteBatch MainSpriteBatch { get; internal set; }

        public InputHandler InputHandler { get; internal set; }
        public ResourceManager Resources { get; internal set; }
        public Options Options { get; set; }
        public AudioManager Audio { get; internal set; }

        protected override void Initialize() {
            InputHandler = new InputHandler(3);
            InputHandler.RegisterKeyboardListener(Keys.W);
            InputHandler.RegisterKeyboardListener(Keys.A);
            InputHandler.RegisterKeyboardListener(Keys.D);
            InputHandler.RegisterKeyboardListener(Keys.Left);
            InputHandler.RegisterKeyboardListener(Keys.Right);
            InputHandler.RegisterKeyboardListener(Keys.Up);
            InputHandler.RegisterKeyboardListener(Keys.J);
            InputHandler.RegisterKeyboardListener(Keys.K);
            InputHandler.RegisterMouseListener(MouseKeys.LeftButton);
            InputHandler.RegisterKeyboardListener(Keys.Escape);
            InputHandler.RegisterKeyboardListener(Keys.F4);
            // Input handler will NOT generate commands without a listener.

            Audio = new AudioManager();

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;

            base.Initialize();
        }

        protected override void LoadContent() {
            MainSpriteBatch = new SpriteBatch(GraphicsDevice);
            Resources = new ResourceManager();
            Resources.Initialize(Content);
            // Go to Utility/ResourceManager.cs to load content.

            _whiteRect = new Texture2D(GraphicsDevice, 1, 1);
            _whiteRect.SetData(new[] {Color.White});

            InitializeGameState();
        }

        /// <summary>
        ///     Try to declare anything you want to declare game-wise here. You can't statically declare up
        ///     there because it needs the Manager to finish what it's doing first.
        /// </summary>
        private void InitializeGameState() {
            SorryScreen = new RestraintMessageScreen();
            ChoiceMaker = new ChoiceMaker();
            CurrentStage = new Stage();
            CurrentStage.LoadLevel(InitialLevel);
            ChoiceMaker.Load(CurrentStage.Data.ChoiceId);
            Audio.PlayMusic("Balcony");
        }

        public new void Dispose() {
            InputHandler.Dispose();
            base.Dispose();
        }

        protected override void UnloadContent() {
            Resources.Unload();
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            updateTimeStep = gameTime.ElapsedGameTime.TotalMilliseconds / 1000d;
            InputHandler.Update(Mouse.GetState(), Keyboard.GetState());
            InputHandler.GenerateCommands();
            // Call game logic on this.Commands to get input data.

            foreach (KeyboardCommand c in InputHandler.KeyCommands) {
                switch (c.Button) {
                    case Keys.Escape:
                        Exit();
                        return;
                    case Keys.F4:
                        if (c.State == InputState.Pressed)
                            Options.LargeResolution = !Options.LargeResolution;
                        break;
                }
            }

            if (_isLargeReso != Options.LargeResolution) {
                //Determines the maximum usable resolution judged by the GraphicsDevice bounds.
                if (GDManager.GraphicsDevice.DisplayMode.Width < Options.RESOLUTION_LARGE.X
                    || GDManager.GraphicsDevice.DisplayMode.Height < Options.RESOLUTION_LARGE.Y)
                    Options.CanHandleLargeReso = false;

                //If the resolution setting is higher than the graphics device can handle, change the option to the usable maxReso.
                if (Options.LargeResolution && !Options.CanHandleLargeReso)
                    Options.LargeResolution = false;

                GDManager.PreferredBackBufferWidth = Options.GetBackBufferWidth();
                GDManager.PreferredBackBufferHeight = Options.GetBackBufferHeight();
                GDManager.IsFullScreen = Options.LargeResolution;
                GDManager.ApplyChanges();
                _isLargeReso = Options.LargeResolution;
            }

            // If the choicemaker is not active, update the stage. Otherwise don't do anything.
            if (!SorryScreen.Update(updateTimeStep))
                if (!ChoiceMaker.Update(updateTimeStep)) {
                    CurrentStage.Update(updateTimeStep);
#if DEBUG
                    // Preprocessor directive removes this function in Release/AnyCPU Configuration.
                    foreach (MouseCommand c in InputHandler.MouseCommands) {
                        //CurrentStage.Player.ActualCoords.X = c.Position.X + CurrentStage.Camera.Location.X;
                        //CurrentStage.Player.ActualCoords.Y = c.Position.Y + CurrentStage.Camera.Location.Y;
                        //CurrentStage.Player.Position = new Rectangle(c.Position, CurrentStage.Player.Position.Size);
                        //CurrentStage.Player.LastPosition = CurrentStage.Player.Position;
                        //CurrentStage.Player.ActualVelocity.X = 0;
                        //CurrentStage.Player.ActualVelocity.Y = 0;
                        if (!CurrentStage.ChoiceMade && Game.Manager.ChoiceMaker.Initialized)
                            Game.Manager.ChoiceMaker.Active = true;
                        else
                            CurrentStage.LoadNextLevel();
                    }
#endif
                }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            drawTimeStep = gameTime.ElapsedGameTime.TotalMilliseconds / 1000d;
            GraphicsDevice.Clear(backgroundColor);

            //Draw Background objects.
            MainSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            CurrentStage.DrawBackground(ChoiceMaker.Active ? drawTimeStep / 4d : drawTimeStep);
            MainSpriteBatch.End();

            //Draw Stage objects.
            MainSpriteBatch.Begin(transformMatrix: CurrentStage.Camera.GetViewMatrix());
            CurrentStage.DrawForeground(drawTimeStep);
            MainSpriteBatch.End();

            //Draw Choice Maker.
            MainSpriteBatch.Begin(samplerState: SamplerState.AnisotropicWrap, blendState: BlendState.AlphaBlend);
            if (SorryScreen.Active)
                SorryScreen.Draw(drawTimeStep);
            else
                ChoiceMaker.Draw(drawTimeStep);
            MainSpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        ///     Floating point positioning.
        /// </summary>
        public void DrawRectangle(Vector2 pos, Vector2 size, Color color, Single rotation = 0.0f, Single opacity = 1.0f,
            Vector2 origin = default(Vector2)) {
            MainSpriteBatch.Draw(_whiteRect, pos * Options.GetResolutionScaleFactor(), null, color * opacity, rotation,
                origin, size * Options.GetResolutionScaleFactor(), SpriteEffects.None, 0f);
        }

        /// <summary>
        ///     Integral positioning.
        /// </summary>
        public void DrawRectangle(Point pos, Point size, Color color, Single rotation = 0.0f, Single opacity = 1.0f,
            Vector2 origin = default(Vector2)) {
            MainSpriteBatch.Draw(_whiteRect,
                new Rectangle(pos.X * Options.GetResolutionScaleFactor(), pos.Y * Options.GetResolutionScaleFactor(),
                    size.X * Options.GetResolutionScaleFactor(), size.Y * Options.GetResolutionScaleFactor()), null,
                color * opacity, rotation, origin, SpriteEffects.None, 0f);
        }
    }
}
