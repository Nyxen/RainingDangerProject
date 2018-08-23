using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainingDanger.Mechanics;
using RainingDanger.Screens;
using System;
using ZeroLibrary.Xna.Graphic.Sprites;
using ZeroLibrary.Xna.Input;
using ZeroLibrary;
 
using ZeroLibrary.Xna.Graphic;


#if !WINDOWS
using Microsoft.Xna.Framework.Input.Touch;

#if ANDROID
using Android.App;
using Android.Content;
using Android.Preferences;

#endif

#endif

namespace RainingDanger
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        TimeSpan elapsedAdRefreshTime = TimeSpan.FromSeconds(26);
        TimeSpan adRefreshTime = TimeSpan.FromSeconds(30);

        public MainGame()
        {

            graphics = new GraphicsDeviceManager(this);
            //maybe we need to buffer for 1080 x 1920
#if WINDOWS

            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
#else
#if iOS
            Content.RootDirectory = "Assets//Content";
#else
            Content.RootDirectory = "Content";
#endif
            graphics.ToggleFullScreen();
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
#endif
        }

        protected override void Initialize()
        {
            Setting setting = Setting.Create();

            Global.MusicEnabled = setting.Get<bool>("music");
            Global.SFXEnabled = setting.Get<bool>("sfx");
            Global.Control = setting.Get<int>("control").Cast<ControlTypes>();


            //need to check if android can actually store longs
            for (int i = 0; i < Global.HighScores.Length; i++)
            {
                Global.HighScores[i] = TimeSpan.FromTicks(setting.Get<long>(String.Format("HighScore{0}", i)));
                Global.HighScoresType[i] = setting.Get<int>(String.Format("HighScoreControl{0}", i)).Cast<ControlTypes>();
                Global.HighScoreDates[i] = new DateTime(setting.Get<long>(String.Format("HighScoresDate{0}", i)));
            }

           
            Global.PlayTutorial = true;
            
            setting.Set("music", Global.MusicEnabled, typeof(bool));
            setting.Set("sfx", Global.SFXEnabled, typeof(bool));
            setting.Set("control", Global.Control.ToInt(), typeof(int));

            for (int i = 0; i < Global.HighScores.Length; i++)
            {
                setting.Set(String.Format("HighScore{0}", i), Global.HighScores[i].Ticks, typeof(long));
                setting.Set(String.Format("HighScoreControl{0}", i), Global.HighScoresType[i].ToInt(), typeof(int));
                setting.Set(String.Format("HighScoresDate{0}", i), Global.HighScoreDates[i].Ticks, typeof(long));
            }
#if !WINDOWS

            TouchManager touchManager = TouchManager.Create(this);
            touchManager.GestureTypes = GestureType.DragComplete | GestureType.Flick | GestureType.HorizontalDrag | GestureType.FreeDrag | GestureType.Tap;

            Components.Add(touchManager);
            //TouchPanel.EnabledGestures = GestureType.DragComplete | GestureType.VerticalDrag | GestureType.HorizontalDrag | GestureType.Tap;
#else
            Components.Add(KeyBoardManager.Create(this));
            Components.Add(MouseManager.Create(this));
            Global.Control = ControlTypes.Tap;

#endif



            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / 30f);
            //IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Global.Pixel.SetData<Color>(new Color[] { Color.White });

            HighScoreControl.Images = new System.Collections.Generic.Dictionary<ControlTypes, Texture2D>();
            HighScoreControl.Images.Add(ControlTypes.Tap, Content.Load<Texture2D>("Screen Images/Tap Asset"));
            HighScoreControl.Images.Add(ControlTypes.Swipe, Content.Load<Texture2D>("Screen Images/Swipe Asset"));

            TitleScreen titleScreen = new TitleScreen(GraphicsDevice, 1080, 1920);
            GameScreen gameScreen = new GameScreen(GraphicsDevice, 1080, 1920);
            GameOverScreen gameOverScreen = new GameOverScreen(GraphicsDevice, 1080, 1920);
            OptionScreen optionScreen = new OptionScreen(GraphicsDevice, 1080, 1920);
            ControlSelectScreen controlScreen = new ControlSelectScreen(GraphicsDevice, 1080, 1920);
            StatScreen highScoreScreen = new StatScreen(GraphicsDevice, 1080, 1920);

            ScreenManager.Add(gameScreen);
            ScreenManager.Add(titleScreen);
            ScreenManager.Add(optionScreen);
            ScreenManager.Add(controlScreen);
            ScreenManager.Add(highScoreScreen);
            ScreenManager.Add(gameOverScreen);

            ScreenManager.Load(Content);


            Global.ElapsedGameTime = TimeSpan.Zero;

            Vector2 currentScale = new Vector2(GraphicsDevice.Viewport.Width / 1080f, GraphicsDevice.Viewport.Height / 1920f);

#if WINDOWS
            MouseManager.Instance.Scale = currentScale;
#else
            //Global.ShowAds(true);
#endif

            ScreenManager.Scale(currentScale);
            ScreenManager.MainScreen = "Title";

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            ScreenManager.Update(gameTime);


#if WINDOWS

            KeyBoardManager keyboard = KeyBoardManager.Instance;

            if(keyboard.IsKeyPressed(Keys.P))
            {
                using(System.IO.Stream stream = System.IO.File.Create(String.Format("ScreenShots//{0}.png", ScreenManager.MainScreen)))
                {
                    Texture2D image = ScreenManager.GetScreen(ScreenManager.MainScreen).RenderTarget;
                    image.SaveAsPng(stream, image.Width, image.Height);
                }
            }



#else

            if (ScreenManager.MainScreen != "Game")
            {
                elapsedAdRefreshTime += gameTime.ElapsedGameTime;
                if (elapsedAdRefreshTime >= adRefreshTime)
                {
                    elapsedAdRefreshTime = TimeSpan.Zero;
                    Global.LoadAds();
                }
            }

#endif

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Setting setting = Setting.Create();

            setting.Set("music", Global.MusicEnabled, typeof(bool));
            setting.Set("sfx", Global.SFXEnabled, typeof(bool));

            for (int i = 0; i < Global.HighScores.Length; i++)
            {
                setting.Set(String.Format("HighScore{0}", i), Global.HighScores[i].Ticks, typeof(long));
                setting.Set(String.Format("HighScoreControl{0}", i), Global.HighScoresType[i].ToInt(), typeof(int));
                setting.Set(String.Format("HighScoresDate{0}", i), Global.HighScoreDates[i].Ticks, typeof(long));
            }

            ScreenManager.Clear();
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            ScreenManager.Render(spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();

            ScreenManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
