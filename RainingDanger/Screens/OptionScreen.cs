using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroLibrary.Xna.Graphic.Sprites;

using ZeroLibrary;
using Microsoft.Xna.Framework.Media;
using ZeroLibrary.Xna.Input;
using ZeroLibrary.Xna.Graphic;
using Microsoft.Xna.Framework.Audio;
#if !WINDOWS
using Microsoft.Xna.Framework.Input.Touch;
#if ANDROID
using Android.Content;
using Android.App;
#endif
#endif
namespace RainingDanger.Screens
{
    public class OptionScreen : Screen
    {
        private Setting _setting;
        public OptionScreen(GraphicsDevice graphicsDevice, int width, int height) :
            base("Option", graphicsDevice, Vector2.Zero, Color.White, width, height)
        {
            _setting = Setting.Create();
        }
        //Sprite musicCheck;
        //Sprite musicX;
        //Sprite soundCheck;
        //Sprite soundX;

        Sprite homeButton;

        SoundEffect clickSound;


        Sprite musicButton;
        Sprite soundButton;
        Sprite swipeButton;
        Sprite tapButton;
        Texture2D selectedMusic;
        Texture2D unSelectedMusic;
        Texture2D selectedSound;
        Texture2D unSelectedSound;
        Texture2D selectedTap;
        Texture2D unselectedTap;
        Texture2D unselectedSwipe;
        Texture2D selectedSwipe;
        bool fadeVolume = false;

        float fadeStep = .01f;

        TimeSpan elapsedTime;
        TimeSpan timeToFade = TimeSpan.FromMilliseconds(50);
        public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            SpriteFont font = content.Load<SpriteFont>("Fonts/dnk36");

            Sprite bg = new Sprite("Background", content.Load<Texture2D>("Backgrounds/Game Screen 6"), Vector2.Zero, Color.White);

            Sprite title = new Sprite("title", content.Load<Texture2D>("Screen Images/settingsTitle"), new Vector2(247, 324), Color.White);

            homeButton = new Sprite("homeButton", content.Load<Texture2D>("Screen Images/Home Icon"), new Vector2(0, 1500), Color.White);
            homeButton.Origin = new Vector2(homeButton.Width / 2, homeButton.Height / 2);
            homeButton.X = RenderTarget.Width / 2;

            selectedMusic = content.Load<Texture2D>("Screen Images/musicIcon");
            unSelectedMusic = content.Load<Texture2D>("Screen Images/Music Selected");
            selectedSound = content.Load<Texture2D>("Screen Images/soundIcon");
            unSelectedSound = content.Load<Texture2D>("Screen Images/Sound Selected");
            selectedTap = content.Load<Texture2D>("Screen Images/Tap Selected");
            unselectedTap = content.Load<Texture2D>("Screen Images/tapIcon");
            selectedSwipe = content.Load<Texture2D>("Screen Images/Swipe Selected");
            unselectedSwipe = content.Load<Texture2D>("Screen Images/swipeIcon");

            musicButton = new Sprite("musicButton", unSelectedMusic, new Vector2(653, 949), Color.White);
            soundButton = new Sprite("soundButton", unSelectedSound, new Vector2(125, 949), Color.White);
            swipeButton = new Sprite("swipeButton", unselectedSwipe, new Vector2(653, 520), Color.White);
            tapButton = new Sprite("tapButton", unselectedTap, new Vector2(125, 520), Color.White);

            if (Global.Control == Mechanics.ControlTypes.NoControl)
            {
                swipeButton.Image = unselectedSwipe;
                tapButton.Image = unselectedTap;
            }


            if (Global.MusicEnabled)
            {
                musicButton.Image = selectedMusic;
            }
            else
            {
                musicButton.Image = unSelectedMusic;
            }
            if (Global.SFXEnabled)
            {
                soundButton.Image = selectedSound;
            }
            else
            {
                soundButton.Image = unSelectedSound;
            }

            SpriteCollection.Add(bg);
            SpriteCollection.Add(homeButton);
            SpriteCollection.Add(title);
            SpriteCollection.Add(musicButton);
            SpriteCollection.Add(soundButton);
            SpriteCollection.Add(swipeButton);
            SpriteCollection.Add(tapButton);



#if WINDOWS
            homeButton.MouseClick += homeButton_MouseClick;
            musicButton.MouseClick += musicButton_MouseClick;
            soundButton.MouseClick += soundButton_MouseClick;
            tapButton.MouseClick += tapButton_MouseClick;
            swipeButton.MouseClick += swipeButton_MouseClick;

#else
            TouchManager touch = TouchManager.Instance;
            touch.GestureOccured += touch_GestureOccured;
#endif

            clickSound = content.Load<SoundEffect>("Sounds/Effects/RD click Sound");
        }

#if !WINDOWS
        void touch_GestureOccured(object sender, ZeroLibrary.Xna.Args.GestureArgs e)
        {
            if (ScreenManager.MainScreen == Name)
            {
                GestureSample gesture = e.Gesture;

                if (gesture.GestureType == GestureType.Tap)
                {
                    if (SpriteCollection["homeButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        
                        ScreenManager.MainScreen = "Title";
                        Global.ShowAds(true);
                        _setting.Set("music", Global.MusicEnabled, typeof(bool));
                        _setting.Set("sfx", Global.SFXEnabled, typeof(bool));
                        _setting.Set("control", Global.Control.ToInt(), typeof(int));
                    }
                    else if (SpriteCollection["soundButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        Global.SFXEnabled = !Global.SFXEnabled;

                        if (Global.SFXEnabled)
                        {
                            soundButton.Image = selectedSound;
                        }
                        else
                        {
                            soundButton.Image = unSelectedSound;
                        }
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                    }

                    else if (SpriteCollection["musicButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        
                        Global.MusicEnabled = !Global.MusicEnabled;

                        if (Global.MusicEnabled)
                        {
                            musicButton.Image = selectedMusic;
                            MediaPlayer.Resume();

                        }
                        else
                        {
                            musicButton.Image = unSelectedMusic;
                            MediaPlayer.Pause();

                        }
                    }
                    else if (SpriteCollection["tapButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.Control = Mechanics.ControlTypes.Tap;
                        tapButton.Image = selectedTap;
                        swipeButton.Image = unselectedSwipe;
                    }
                    else if (SpriteCollection["swipeButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.Control = Mechanics.ControlTypes.Swipe;
                        swipeButton.Image = selectedSwipe;
                        tapButton.Image = unselectedTap;
                    }
                }
            }
        }
#else

        void tapButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if(Global.SFXEnabled)
            {
                clickSound.Play();
            }

            Global.Control = Mechanics.ControlTypes.Tap;

            tapButton.Image = selectedTap;
            swipeButton.Image = unselectedSwipe;


        }

        void swipeButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            Global.Control = Mechanics.ControlTypes.Swipe;

            swipeButton.Image = selectedSwipe;
            tapButton.Image = unselectedTap;

        }

        void soundButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            Global.SFXEnabled = !Global.SFXEnabled;
            if (Global.SFXEnabled)
            {
                soundButton.Image = selectedSound;
            }
            else
            {
                soundButton.Image = unSelectedSound;   
            }

            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            
        }

        void musicButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
           
            Global.MusicEnabled = !Global.MusicEnabled;

            if (Global.MusicEnabled)
            {
                musicButton.Image = selectedMusic;
                  MediaPlayer.Resume();
            }
            else
            {
                musicButton.Image = unSelectedMusic;
                  MediaPlayer.Pause();    
            }
        }

        void homeButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if (Global.SFXEnabled)
            {
                clickSound.Play();
            }
            if (MediaPlayer.Volume < 1)
            {
                MediaPlayer.Volume = 1;
            }
            _setting.Set("music", Global.MusicEnabled, typeof(bool));
            _setting.Set("sfx", Global.SFXEnabled, typeof(bool));
            _setting.Set("control", Global.Control.ToInt(), typeof(int));
            
            ScreenManager.MainScreen = "Title";
        }

#endif

        public override void Update(GameTime gameTime)
        {
            if (Global.SFXEnabled)
            {
                soundButton.Image = selectedSound;
            }
            else
            {
                soundButton.Image = unSelectedSound;
            }
            if (Global.MusicEnabled)
            {
                musicButton.Image = selectedMusic;

            }
            else
            {
                musicButton.Image = unSelectedMusic;

            }
            if (Global.Control == Mechanics.ControlTypes.Swipe)
            {
                swipeButton.Image = selectedSwipe;
                tapButton.Image = unselectedTap;
            }
            else
            {
                tapButton.Image = selectedTap;
                swipeButton.Image = unselectedSwipe;
            }
            if (Global.Control == Mechanics.ControlTypes.NoControl)
            {
                swipeButton.Image = unselectedSwipe;
                tapButton.Image = unselectedTap;
            }



            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Global.BackgroundSong);
            }

            base.Update(gameTime);
        }

    }
}
