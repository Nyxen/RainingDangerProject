using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ZeroLibrary.Xna.Graphic.Sprites;
using Microsoft.Xna.Framework;

using ZeroLibrary;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ZeroLibrary.Xna.Graphic;

#if !WINDOWS
using ZeroLibrary.Xna.Input;
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace RainingDanger.Screens
{
    class ControlSelectScreen : Screen
    {
        private Setting _setting;
        public ControlSelectScreen(GraphicsDevice graphicsDevice, int width, int height) :
            base("ControlSelect", graphicsDevice, Vector2.Zero, Color.White, width, height)
        {
            _setting = Setting.Create();
        }

        SoundEffect clickSound;

        public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Sprite bg = new Sprite("Background", Global.Backgrounds[Global.CurrentBackgroundIndex], Vector2.Zero, Color.White);
            Sprite title = new Sprite("title", content.Load<Texture2D>("Screen Images/Game Type"), new Vector2(103, 237), Color.White);
            Sprite tapButton = new Sprite("tap", content.Load<Texture2D>("Screen Images/tapSelect"), new Vector2(259, 573), Color.White);
            Sprite or = new Sprite("or", content.Load<Texture2D>("Screen Images/Or"), new Vector2(456, 1116), Color.White);
            Sprite swipeButton = new Sprite("swipe", content.Load<Texture2D>("Screen Images/swipeSelect"), new Vector2(259, 1298), Color.White);


            SpriteCollection.Add(bg);
            SpriteCollection.Add(title);
            SpriteCollection.Add(tapButton);
            SpriteCollection.Add(or);
            SpriteCollection.Add(swipeButton);

#if WINDOWS
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
                    if (SpriteCollection["tap"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.Control = Mechanics.ControlTypes.Tap;
                        _setting.Set("control", Global.Control.ToInt(), typeof(int));
                    }
                    else if (SpriteCollection["swipe"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if (Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }
                        Global.Control = Mechanics.ControlTypes.Swipe;
                        _setting.Set("control", Global.Control.ToInt(), typeof(int));
                    }

                    if (Global.Control != Mechanics.ControlTypes.NoControl)
                    {
                        ScreenManager.MainScreen = "Game";
                    }
                }
            }
        }


#else
        void tapButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {

        }
        void swipeButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {

        }
#endif
        public override void Update(GameTime gameTime)
        {
            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Global.GameplaySongs[Global.currentSongIndex]);
            }

            base.Update(gameTime);
        }
    }
}
