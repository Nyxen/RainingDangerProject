using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroLibrary.Xna.Graphic.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Media;

using ZeroLibrary;
using ZeroLibrary.Xna.Input;
using ZeroLibrary.Xna.Graphic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;


#if !WINDOWS
using Microsoft.Xna.Framework.Input.Touch;
#endif


namespace RainingDanger.Screens
{
    public class TitleScreen : Screen
    {

        Random random = new Random();

        public TitleScreen(GraphicsDevice graphicsDevice, int width, int height) :
            base("Title", graphicsDevice, Vector2.Zero, Color.White, width, height)
        {

        }

        SoundEffect clickSound;
        Random gen;
        Sprite playButton;
        TimeSpan animationTime;
        TimeSpan timeToAnimate = new TimeSpan(0, 0, 0, 0, 250);
        Song titleSong;
        bool fadeVolume = false;
        float fadeStep = .09f;

        TimeSpan elapsedTime;
        TimeSpan timeToFade = TimeSpan.FromMilliseconds(50);
        public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            gen = new Random();

            SpriteFont font = content.Load<SpriteFont>("Fonts/dnk24");

            Sprite title = new Sprite("title", content.Load<Texture2D>("Screen Images/Raining Danger"), Vector2.Zero, Color.White);
            title.Position = new Vector2(RenderTarget.Width / 2 - title.Width / 2, RenderTarget.Height / 2 - title.Height * 1.3f);


            playButton = new Sprite("playButton", content.Load<Texture2D>("Screen Images/Play Button"), Vector2.Zero, Color.White);
            playButton.Position = new Vector2((RenderTarget.Width - playButton.Width) / 2, 820);

            //SpriteLabel playLabel = new SpriteLabel("playLabel", font, "Play", Vector2.Zero, Color.White);
            ////playLabel.Origin = new Vector2(playLabel.Width / 2, playLabel.Height / 2);
            //playLabel.Position = new Vector2(334, 1020);
            //playLabel.Scale = new Vector2(9f, 3f);
            //playLabel.IsVisible = true;

            //SpriteLabel optionLabel = new SpriteLabel("optionLabel", font, "Option", playButton.Position, Color.White);
            //optionLabel.Scale = new Vector2(4.5f);



            Sprite highScoreButton = new Sprite("highScore", content.Load<Texture2D>("Screen Images/Rankiings Icon"), Vector2.Zero, Color.White);
            highScoreButton.Scale = new Vector2(.7f);
            highScoreButton.Position = new Vector2(RenderTarget.Width - highScoreButton.Width * 1.2f, RenderTarget.Height - highScoreButton.Height * 2.4f);

            Sprite optionButton = new Sprite("options", content.Load<Texture2D>("Screen Images/Menu Icon"), Vector2.Zero, Color.White);
            optionButton.Scale = new Vector2(.7f);
            optionButton.Position = new Vector2(30, highScoreButton.Y);

            //optionLabel.Position = new Vector2((playButton.X + playButton.Width) /2.1f, highScoreButton.Y - optionLabel.Height * 0.9f);

            Sprite backGround = new Sprite("backGround", content.Load<Texture2D>("Backgrounds/Game Screen 6"), Vector2.Zero, Color.White);


            SpriteCollection.Add(backGround);
            SpriteCollection.Add(title);
            SpriteCollection.Add(playButton);
            //SpriteCollection.Add(optionLabel);
            SpriteCollection.Add(optionButton);
            SpriteCollection.Add(highScoreButton);

            string themeSong = String.Empty;

#if WINDOWS
            themeSong = "Sounds/Raining Danger";
            playButton.MouseClick += playLabel_MouseClick;
            optionButton.MouseClick += optionButton_MouseClick;
            //optionLabel.MouseClick += optionLabel_MouseClick;
            highScoreButton.MouseClick += highScoreButton_MouseClick;

#else
            TouchManager touchManger = TouchManager.Instance;
            touchManger.GestureOccured += touchManger_GestureOccured;
            themeSong = "Sounds/Raining Danger";
#endif
            titleSong = content.Load<Song>(themeSong);
            Global.BackgroundSong = titleSong;
            clickSound = content.Load<SoundEffect>("Sounds/Effects/RD click Sound");
        }


        void transitionToGame()
        {
            ScreenManager.MainScreen = "Game";
            MediaPlayer.Stop();
            
            Global.PlayTutorial = true;

            Global.CurrentBackgroundIndex = random.Next(0, Global.Backgrounds.Count);
            Global.currentSongIndex = random.Next(0, Global.GameplaySongs.Count);
        }

#if WINDOWS
        void optionButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            ScreenManager.MainScreen = "Option";
        }
        //void optionLabel_MouseClick(object sender, ZeroXNALibrary.Args.MouseArgs e)
        //{
        //    ScreenManager.MainScreen = "Option";
        //}

        void playLabel_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            transitionToGame();
        }
        void highScoreButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            ScreenManager.MainScreen = "HighScore";
        }
#else
        
        void touchManger_GestureOccured(object sender, ZeroLibrary.Xna.Args.GestureArgs e)
        {
            if (ScreenManager.MainScreen == Name)
            {
                GestureSample gesture = e.Gesture;

                if (gesture.GestureType == GestureType.Tap)
                {
                    if (SpriteCollection["playButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        if(Global.Control == Mechanics.ControlTypes.NoControl)
                        {
                            ScreenManager.MainScreen = "ControlSelect";
                            MediaPlayer.Stop();
            
                            Global.PlayTutorial = true;

                            Global.CurrentBackgroundIndex = random.Next(0, Global.Backgrounds.Count);
                            Global.currentSongIndex = random.Next(0, Global.GameplaySongs.Count);
                        }
                        else
                        {
                            transitionToGame();
                        }
                        Global.ShowAds(false);
                    }
                    else if (SpriteCollection["options"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.ShowAds(true);
                        ScreenManager.MainScreen = "Option";
                    }
                    else if (SpriteCollection["highScore"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.ShowAds(true);
                        ScreenManager.MainScreen = "HighScore";
                    }
                }
            }
        }
#endif
        bool direction = true;
        public void ButtonFloat(int minHeight, int maxHeight)
        {
            if (playButton.Y >= minHeight && direction)
            {
                playButton.Y -= .4f;
                if (playButton.Y <= minHeight)
                {
                    direction = false;
                }
            }
            if (playButton.Y <= maxHeight && !direction)
            {
                playButton.Y += .4f;
                if (playButton.Y >= maxHeight)
                {
                    direction = true;
                }
            }

        }


        public override void Update(GameTime gameTime)
        {
            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(titleSong);
            }
            ButtonFloat(810, 850);
            base.Update(gameTime);
        }
    }
}