using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using ZeroLibrary.Xna.Graphic.Sprites;
using ZeroLibrary.Xna.Graphic;
using ZeroLibrary;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZeroLibrary.Xna.Input;
using RainingDanger.Mechanics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#if !WINDOWS
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace RainingDanger.Screens
{
    public class GameOverScreen : Screen
    {

        private Setting _setting;
        Random rand;
        public GameOverScreen(GraphicsDevice graphicsDevice, int width, int height) :
            base("GameOver", graphicsDevice, Vector2.Zero, Color.White, width, height)
        {
            _setting = Setting.Create();
        }
        Sprite background;
        SpriteLabel highScore;
        SoundEffect clickSound;
        SpriteLabel newHighScore;

        float fadeStep = .09f;

        TimeSpan elapsedTime;
        TimeSpan timeToFade = TimeSpan.FromMilliseconds(50);
        bool fadeVolume = false;

        public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            rand = new Random();

            SpriteLabel gameOver = new SpriteLabel("gameOver", content.Load<SpriteFont>("Fonts/dnk72"), "Game Over!", Vector2.Zero, Color.White);
            gameOver.Origin = new Vector2(gameOver.Width / 2, 0);
            gameOver.Position = new Vector2(RenderTarget.Width / 2, 240);

            highScore = new SpriteLabel("HighScoreLabel", content.Load<SpriteFont>("Fonts/dnk121"), "Test", Vector2.Zero, Color.White);
            //highScore.Position = new Vector2(557, 886);
            //highScore.Scale = new Vector2(5);

            Sprite playButton = new Sprite("Play", content.Load<Texture2D>("Screen Images/Replay"), Vector2.Zero, Color.White);
            //playButton.Scale = new Vector2(.2f);
            playButton.Position = new Vector2(557, 1124);


            Sprite house = new Sprite("House", content.Load<Texture2D>("Screen Images/Home Icon"), Vector2.Zero, Color.White);
            //house.Scale = new Vector2(.5f);
            house.Position = new Vector2(235, 1124);

            Sprite highScoreButton = new Sprite("statButton", content.Load<Texture2D>("Screen Images/Rankiings Icon"), Vector2.Zero, Color.White);
            highScoreButton.Position = new Vector2(235, 1407);

            Sprite settingsButton = new Sprite("settingsButton", content.Load<Texture2D>("Screen Images/Menu Icon"), Vector2.Zero, Color.White);
            settingsButton.Position = new Vector2(557, 1407);

            Sprite timeBox = new Sprite("timeBox", content.Load<Texture2D>("Screen Images/Time"), Vector2.Zero, Color.White);
            timeBox.Position = new Vector2(257, 589);




            newHighScore = new SpriteLabel("newHighScore", content.Load<SpriteFont>("Fonts/dnk64"), "New High Score!", Vector2.Zero, Color.White);
            //newHighScore.Scale = new Vector2(4);
            newHighScore.Origin = new Vector2(newHighScore.Width / 2, 0);
            newHighScore.Position = new Vector2(RenderTarget.Width / 2, 397);

            background = new Sprite("Background", Global.Backgrounds[Global.CurrentBackgroundIndex], Vector2.Zero, Color.White);


#if WINDOWS
            house.MouseClick += house_MouseClick;
            playButton.MouseClick += playButton_MouseClick;
            highScoreButton.MouseClick += highScoreButton_MouseClick;
            settingsButton.MouseClick += settingsButton_MouseClick;
#else
            TouchManager touch = TouchManager.Instance;
            touch.GestureOccured += touch_GestureOccured;
#endif


            SpriteCollection.Add(background);
            SpriteCollection.Add(gameOver);
            SpriteCollection.Add(playButton);
            SpriteCollection.Add(highScoreButton);
            SpriteCollection.Add(settingsButton);
            SpriteCollection.Add(timeBox);
            SpriteCollection.Add(house);
            SpriteCollection.Add(highScore);
            SpriteCollection.Add(newHighScore);


            clickSound = content.Load<SoundEffect>("Sounds/Effects/RD click Sound");

        }
       
        void transtitionToTitle()
        {
            Global.ElapsedGameTime = TimeSpan.Zero;
            MediaPlayer.Stop();
            MediaPlayer.Volume = 1;
            ScreenManager.MainScreen = "Title";
            Global.GotHighScore = false;
        }
        void transitionToHighScore()
        {
            Global.ElapsedGameTime = TimeSpan.Zero;
            Global.GotHighScore = false;
            ScreenManager.MainScreen = "HighScore";
        }
        void trasitionToOption()
        {
            Global.ElapsedGameTime = TimeSpan.Zero;
            Global.GotHighScore = false;
            ScreenManager.MainScreen = "Option";
        }
        void transitionToGame()
        {
            Global.ElapsedGameTime = TimeSpan.Zero;
            Global.GotHighScore = false;
            ScreenManager.MainScreen = "Game";
            Global.CurrentBackgroundIndex = rand.Next(0, Global.Backgrounds.Count);
            MediaPlayer.Stop();
            MediaPlayer.Volume = 1;
            Global.currentSongIndex = rand.Next(0, Global.GameplaySongs.Count);
        }

#if WINDOWS

        void house_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            transtitionToTitle();
        }

        void highScoreButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            transitionToHighScore();
        }

        void settingsButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            trasitionToOption();
        }

        void playButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            transitionToGame();
        }

        //void storeButton_MouseClick(object sender, ZeroXNALibrary.Args.MouseArgs e)
        //{
        //    if (Global.SFXEnabled)
        //    {
        //        clickSound.Play();
        //    }
        //    Global.GotHighScore = false;
        //    Global.ElapsedGameTime = TimeSpan.Zero;
        //    ScreenManager.MainScreen = "Store";
        //}
#else

        void touch_GestureOccured(object sender, ZeroLibrary.Xna.Args.GestureArgs e)
        {
            if (ScreenManager.MainScreen == Name)
            {
                GestureSample gesture = e.Gesture;
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (SpriteCollection["House"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        Global.ShowAds(true);
                        transtitionToTitle();
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }

                    }

                    if (SpriteCollection["statButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        Global.ShowAds(true);
                        transitionToHighScore();
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                    }
                    if (SpriteCollection["settingsButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        trasitionToOption();
                        Global.ShowAds(true);
                    }

                    if (SpriteCollection["Play"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        transitionToGame();
                        Global.ShowAds(false);
                        
                    }

                    for (int i = 0; i < Global.HighScores.Length; i++)
                    {
                        _setting.Set(String.Format("HighScore{0}", i), Global.HighScores[i].Ticks, typeof(long));

                        _setting.Set(String.Format("HighScoreControl{0}", i), Global.HighScoresType[i].ToInt(), typeof(int));
                        _setting.Set(String.Format("HighScoresDate{0}", i), Global.HighScoreDates[i].Ticks, typeof(long));
                    }

                }
            }
        }
#endif

        public override void Update(GameTime gameTime)
        {

            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Global.GameplaySongs[Global.currentSongIndex]);
            }
            background.Image = Global.Backgrounds[Global.CurrentBackgroundIndex];

            SpriteCollection["newHighScore"].Cast<SpriteLabel>().IsVisible = Global.GotHighScore;

            SpriteCollection["HighScoreLabel"].Cast<SpriteLabel>().Text = String.Format("{0} :{1}", Global.ElapsedGameTime.Minutes.ToString("D2"), Global.ElapsedGameTime.Seconds.ToString("D2"));

            highScore.Position = new Vector2((SpriteCollection["timeBox"].Cast<Sprite>().Position.X + SpriteCollection["timeBox"].Cast<Sprite>().Width) / 2.5f, 786);

            base.Update(gameTime);
        }
    }
}