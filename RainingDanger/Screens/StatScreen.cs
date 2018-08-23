using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ZeroLibrary.Xna.Graphic.Sprites;
using RainingDanger;

using ZeroLibrary;
using ZeroLibrary.Xna.Input;
using ZeroLibrary.Xna.Graphic;
using RainingDanger.Mechanics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#if !WINDOWS


//using Android.Gestures;

using Microsoft.Xna.Framework.Input.Touch;
#endif
namespace RainingDanger.Screens
{
    class StatScreen : Screen
    {
        public StatScreen(GraphicsDevice graphicsDevice, int width, int height) :
            base("HighScore", graphicsDevice, Vector2.Zero, Color.White, width, height)
        {

        }

        SoundEffect clickSound;

        public override void Load(ContentManager content)
        {
            Sprite bg = new Sprite("Background", content.Load<Texture2D>("Backgrounds/Game Screen 6"), Vector2.Zero, Color.White);
            SpriteCollection.Add(bg);

            Sprite homeButton = new Sprite("homeButton", content.Load<Texture2D>("Screen Images/Home Icon"), new Vector2(0, 1500), Color.White);
            homeButton.Origin = new Vector2(homeButton.Width / 2, homeButton.Height / 2);
            homeButton.X = RenderTarget.Width / 2;
            SpriteCollection.Add(homeButton);


            Sprite title = new Sprite("title", content.Load<Texture2D>("Screen Images/ScoreTitle"), new Vector2(RenderTarget.Width / 2, 182), Color.White);
            title.Origin = new Vector2(title.Width / 2, 0);
            SpriteCollection.Add(title);

            SpriteLabel timeTitle = new SpriteLabel("timeTitle", content.Load<SpriteFont>("Fonts/dnk48"), "Time:", new Vector2(58, 286), Color.White);
            SpriteLabel typeTitle = new SpriteLabel("typeTitle", content.Load<SpriteFont>("Fonts/dnk48"), "Type:", new Vector2(449, 286), Color.White);
            SpriteLabel dateTitle = new SpriteLabel("dateTitle", content.Load<SpriteFont>("Fonts/dnk48"), "Date:", new Vector2(836, 286), Color.White);

            SpriteCollection.Add(timeTitle);
            SpriteCollection.Add(typeTitle);
            SpriteCollection.Add(dateTitle);

            SpriteLabel highScoreLabel = new SpriteLabel("highScoreLabel", content.Load<SpriteFont>("Fonts/dnk48"), String.Empty, new Vector2(RenderTarget.Width / 2, 350), Color.Black);
            SpriteCollection.Add(highScoreLabel);

            int y = 441;
            int difference = 200;

            for (int i = 0; i < Global.HighScores.Length; i++)
            {
                SpriteLabel scoreLabel = new SpriteLabel(String.Format("scoreLabel{0}", i), content.Load<SpriteFont>("Fonts/dnk48"), String.Empty, new Vector2(timeTitle.X + timeTitle.Width / 2.4f, y), Color.White);
                SpriteCollection.Add(scoreLabel);

                HighScoreControl scoreTypeLabel = new HighScoreControl(String.Format("scoreTypeLabel{0}", i), ControlTypes.NoControl, scoreLabel.Position);
                scoreTypeLabel.X = typeTitle.X + typeTitle.Width / 2.4f;
                SpriteCollection.Add(scoreTypeLabel);

                SpriteLabel scoreDateLabel = new SpriteLabel(String.Format("scoreDateLabel{0}", i), content.Load<SpriteFont>("Fonts/dnk48"), String.Empty, scoreLabel.Position, Color.White);
                scoreDateLabel.X = dateTitle.X + dateTitle.Width / 2.4f;
                SpriteCollection.Add(scoreDateLabel);


                y += difference;
            }

#if WINDOWS
            homeButton.MouseClick += homeButton_MouseClick;
#else
            TouchManager touchManger = TouchManager.Instance;
            touchManger.GestureOccured += touchManger_GestureOccured;
#endif

            clickSound = content.Load<SoundEffect>("Sounds/Effects/RD click Sound");
        }


#if WINDOWS
        void homeButton_MouseClick(object sender, ZeroLibrary.Xna.Args.MouseArgs e)
        {
            if(Global.SFXEnabled)
            {
                clickSound.Play();
            }
            ScreenManager.MainScreen = "Title";
        }
#else

        void touchManger_GestureOccured(object sender, ZeroLibrary.Xna.Args.GestureArgs e)
        {
            if (ScreenManager.MainScreen == Name)
            {
                GestureSample gesture = e.Gesture;

                if (gesture.GestureType == GestureType.Tap)
                {
                    if (SpriteCollection["homeButton"].BoundingBox.Contains(gesture.Position / Scale))
                    {
                        if(Global.SFXEnabled)
                        {
                            clickSound.Play();
                        }

                        Global.ShowAds(true);
                        ScreenManager.MainScreen = "Title";
                    }
                }
            }
        }


#endif

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Global.HighScores.Length; i++)
            {
                SpriteLabel scoreLabel = SpriteCollection[String.Format("scoreLabel{0}", i)].Cast<SpriteLabel>();
                scoreLabel.Text = String.Format("{0} :{1}", Global.HighScores[i].Minutes.ToString("D2"), Global.HighScores[i].Seconds.ToString("D2"));
                scoreLabel.Origin = new Vector2(scoreLabel.Width / 2, scoreLabel.Height / 2);
                scoreLabel.IsVisible = Global.HighScores[i] != TimeSpan.Zero;


                HighScoreControl scoreTypeLabel = SpriteCollection[String.Format("scoreTypeLabel{0}", i)].Cast<HighScoreControl>();
                scoreTypeLabel.Control = Global.HighScoresType[i];
                if (scoreTypeLabel.Control != ControlTypes.NoControl)
                {
                    //origin needs to be calculated based on original size of the image so setting scale back to 1 just for origin recalculation
                    scoreTypeLabel.Scale = Vector2.One;
                    scoreTypeLabel.Origin = new Vector2(scoreTypeLabel.Width / 2, scoreTypeLabel.Height / 2);
                    scoreTypeLabel.X = RenderTarget.Width / 2;
                    scoreTypeLabel.Scale = new Vector2(.85f); 
                }

                SpriteLabel scoreDateLabel = SpriteCollection[String.Format("scoreDateLabel{0}", i)].Cast<SpriteLabel>();
                scoreDateLabel.Text = Global.HighScoreDates[i].ToShortDateString();
                scoreDateLabel.Origin = new Vector2(scoreDateLabel.Width / 2, scoreDateLabel.Height / 2);
                scoreDateLabel.IsVisible = Global.HighScores[i] != TimeSpan.Zero;

            }

            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Global.BackgroundSong);
            }

            base.Update(gameTime);
        }
    }
}
