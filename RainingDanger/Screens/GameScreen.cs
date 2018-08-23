using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroLibrary.Xna.Graphic;
using ZeroLibrary.Xna.Graphic.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainingDanger.Mechanics;
using Microsoft.Xna.Framework.Content;

using XnaAudio = Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using ZeroLibrary.Xna.Interfaces;
using System.Diagnostics;

using Microsoft.Xna.Framework.Media;
using ZeroLibrary;
using ZeroLibrary.Xna.Input;
using Microsoft.Xna.Framework.Input.Touch;



#if ANDROID || __IOS__
using Microsoft.Xna.Framework.Input.Touch;
#endif


namespace RainingDanger.Screens
{
    public class GameScreen : Screen
    {
        Character player;

        List<Item> badItems;

        UniqueNumberGenerator _random;

        TimeSpan spawningTime;
        TimeSpan elapsedSpawningTime;

        TimeSpan difficultyTime;
        TimeSpan elapsedDifficultyTime;

        TimeSpan arrowFadeTime;
        TimeSpan elapsedArrowTime;

        Vector2[] fallingLocation;

        bool swipeRegister = false;
        SpriteLabel _fpsLabel;

        Texture2D pixel;

        int _drawCounter;
        TimeSpan _elapsedDrawTime;
        TimeSpan _drawTime;

        Column[] _columns;
        Column _lastSpwaned;

        Direction direction;
        Direction directionToGo;
        int movingSpeed = 37;

        int playerIndex;

        int startingSpeed = 20;
        TimeSpan startingDifficultyTime;

        //XnaAudio.Song leftSwipe;
        //XnaAudio.Song rightSwipe;
        //XnaAudio.Song deathSound;

        SoundEffect leftSwipe;
        SoundEffect rightSwipe;
        SoundEffect deathSound;
        SoundEffect highScoreSound;

        bool debug = false;

        Sprite leftHelpButton;
        Sprite rightHelpButton;

        ContentManager _content;

        //bool isFirstTap = true;

        bool hitDuringAnimation = false;

        private int _level = 0;

        SpriteLabel timeLabel;

        //List<Texture2D> itemTextures;

        public GameScreen(GraphicsDevice graphicDevice, int width, int height) :
            base("Game", graphicDevice, Vector2.Zero, Color.White, width, height)
        {
            IsRendering = true;
            _backgroundColor = Color.White;
            _random = new UniqueNumberGenerator();


            spawningTime = TimeSpan.FromMilliseconds(150);
            elapsedSpawningTime = TimeSpan.Zero;

            elapsedDifficultyTime = TimeSpan.Zero;
            difficultyTime = TimeSpan.FromSeconds(20);
            elapsedArrowTime = TimeSpan.Zero;

            fallingLocation = new Vector2[4];
            fallingLocation[0] = new Vector2(125, 0);
            fallingLocation[1] = new Vector2(385, 0);
            fallingLocation[2] = new Vector2(655, 0);
            fallingLocation[3] = new Vector2(935, 0);



            _drawTime = TimeSpan.FromSeconds(1);
            _elapsedDrawTime = TimeSpan.Zero;
            _drawCounter = 0;
        }

        /// <summary>
        /// loads all the assets for all the objects
        /// </summary>
        /// <param name="content"></param>
        private void loadingDropingObjects(ContentManager content)
        {
            badItems = new List<Item>();

            //itemTextures = new List<Texture2D>();

            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Rock"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Crate"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Hammer"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Knife"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Cactus"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Jackhammer"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Chainsaw"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Sword"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Lawnmower"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/TV"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Safe"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Anvil"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Anchor"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Piano"));
            //itemTextures.Add(content.Load<Texture2D>("Bad Images/Shark"));

            //badItems.Add(new Item(itemTextures[0], ItemType.Bad, true));


            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Cactus"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Chainsaw"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Hammer"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Jackhammer"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Knife"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Lawnmower"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Shark"), ItemType.Bad, false));

            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Anchor"), ItemType.Bad, false));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Anvil"), ItemType.Bad, false));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Crate"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Piano"), ItemType.Bad, false));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Rock"), ItemType.Bad, true));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Safe"), ItemType.Bad, false));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/Sword"), ItemType.Bad, false));
            badItems.Add(new Item(content.Load<Texture2D>("Bad Images/TV"), ItemType.Bad, true));


            Column.BadItems = badItems;
        }


        private void loadingBackgrounds(ContentManager content)
        {
            for (int i = 1; i < 7; i++)
            {
                Global.Backgrounds.Add(content.Load<Texture2D>(String.Format("Backgrounds/Game Screen {0}", i)));
            }
        }

        private void loadingSongs(ContentManager content)
        {
            for (int i = 1; i < 4; i++)
            {
                Global.GameplaySongs.Add(content.Load<Song>(string.Format("Sounds/Raining Danger Gameplay {0} 5 min 5.6",i)));
            }
        }

        Sprite backGround;
        float startingAlpha = 1f;

        float endingAlpha = .5f;

        float startingArrowAlpha = .3f;

        HighScoreControl controlLabel;

        public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {

            _content = content;
            loadingDropingObjects(content);
            loadingBackgrounds(content);
            loadingSongs(content);



            FallingItem.FallingSpeed = startingSpeed;
            startingDifficultyTime = new TimeSpan(0, 0, 3);
            elapsedArrowTime = TimeSpan.Zero;
            difficultyTime = startingDifficultyTime;

            arrowFadeTime = new TimeSpan(0, 0, 0, 5);

            //SpriteLabel tutorialText = new SpriteLabel("tutorialText", content.Load<SpriteFont>("Fonts/FS14"), "Press the arrows to move!", Vector2.Zero, Color.Black);
            //tutorialText.Scale = new Vector2(3);
            //tutorialText.Position = new Vector2(RenderTarget.Width / 2 - tutorialText.Width / 2, RenderTarget.Height - tutorialText.Height * 2);
            //tutorialText.IsVisible = Global.Tutorial;

            _columns = new Column[fallingLocation.Length];
            for (int i = 0; i < _columns.Length; i++)
            {
                _columns[i] = new Column(fallingLocation[i].X.ToInt(true), RenderTarget.Height);
            }

            _columns[0].NextColumns.Add(_columns[1]);

            //_columns[1].NextColumns.Add(_columns[0]);
            _columns[1].NextColumns.Add(_columns[2]);

            //_columns[2].NextColumns.Add(_columns[1]);
            _columns[2].NextColumns.Add(_columns[3]);

            _columns[3].NextColumns.Add(_columns[2]);


            backGround = new Sprite("backGround", Global.Backgrounds[Global.CurrentBackgroundIndex], Vector2.Zero, Color.White);


            leftHelpButton = new Sprite("leftButton", content.Load<Texture2D>("Screen Images/Left Arrow"), Vector2.Zero, Color.White);
            leftHelpButton.Scale = new Vector2(.8f);
            leftHelpButton.Position = new Vector2(0, RenderTarget.Height - leftHelpButton.Height * 3f);
            leftHelpButton.Color = Color.Lerp(Color.White, Color.Transparent, .3f);

            //leftHelpButton.Effect = SpriteEffects.FlipHorizontally;

            rightHelpButton = new Sprite("rightButton", content.Load<Texture2D>("Screen Images/Right Arrow"), Vector2.Zero, Color.White);
            rightHelpButton.Scale = leftHelpButton.Scale;
            rightHelpButton.Position = new Vector2(RenderTarget.Width - rightHelpButton.Width, leftHelpButton.Y);
            rightHelpButton.Color = Color.Lerp(Color.White, Color.Transparent, .3f);



            _fpsLabel = new SpriteLabel("SpeedLabel", content.Load<SpriteFont>("Fonts/dnk24"), String.Empty, Vector2.Zero, Color.Black);
            _fpsLabel.Text = String.Format("Speed: {0}", FallingItem.FallingSpeed);
            _fpsLabel.Scale = new Vector2(3);
            _fpsLabel.Y = RenderTarget.Height - _fpsLabel.Height;
            _fpsLabel.IsVisible = false;

            //death
            Texture2D firstDeathFrame = content.Load<Texture2D>("Characters/Effects/Death/explosion1");
            SpriteSheet deathSprite = new SpriteSheet("RedPanda", Vector2.Zero, Color.White, firstDeathFrame);

            for (int i = 2; i < 6; i++)
            {
                Sprite deathFrame = new Sprite("RedPanda", content.Load<Texture2D>(String.Format("Characters/Effects/Death/explosion{0}", i)), Vector2.Zero, Color.White);
                deathSprite.Add(deathFrame);
            }

            foreach (Sprite frame in deathSprite)
            {
                frame.Origin = new Vector2(frame.Width / 2, frame.Height / 3 * 2);
                frame.Scale = new Vector2(0.8f);
            }
            deathSprite.AnimateTime = TimeSpan.FromMilliseconds(100);
            deathSprite.AnimationFinish += deathSprite_AnimationFinish;

            playerIndex = 1;

            //ria load
            Sprite idleRiaFrame = new Sprite("RedPanda", content.Load<Texture2D>("Characters/RedPanda/Ria/0"), Vector2.Zero, Color.White);
            idleRiaFrame.Origin = new Vector2(380, idleRiaFrame.Height);


            Texture2D test = content.Load<Texture2D>("Characters/RedPanda/Ria/1");
            SpriteSheet movingRiaFrames = new SpriteSheet("RedPanda", Vector2.Zero, Color.White, test);

            for (int i = 2; i < 7; i++)
            {
                Sprite frame = new Sprite("RedPanda", content.Load<Texture2D>(String.Format("Characters/RedPanda/Ria/{0}", i)), Vector2.Zero, Color.White);
                movingRiaFrames.Add(frame);
            }
            movingRiaFrames.Origin = new Vector2(380, test.Height);


            movingRiaFrames.AnimateTime = TimeSpan.FromMilliseconds(25 / (_level + 1));//speed animation up according to level. Kevin

            movingRiaFrames.AnimationFinish += movingRiaFrames_AnimationFinish;

            player = new Character(new ISprite[] { idleRiaFrame, movingRiaFrames, deathSprite });

            player.Position = new Vector2(fallingLocation[playerIndex].X - 1, RenderTarget.Height - 50);

            direction = Direction.None;
            //SpriteCollection.Add(tutorialText);
            SpriteCollection.Add(backGround);
            SpriteCollection.Add(_fpsLabel);

            SpriteCollection.Add(leftHelpButton);
            SpriteCollection.Add(rightHelpButton);

            string leftSwipeFile = String.Empty;
            string rightSwipeFile = String.Empty;

            XnaAudio.MediaPlayer.Volume = 1;

            SpriteCollection.NameCheck = false;
            SpriteCollection.Add(player);

            foreach (Column column in _columns)
            {
                foreach (FallingItem item in column.Pool)
                {
                    SpriteCollection.Add(item);
                }
            }
            if (Global.Control == ControlTypes.Swipe)
            {
                leftHelpButton.IsVisible = false;
                rightHelpButton.IsVisible = false;
            }
            else
            {
                leftHelpButton.IsVisible = true;
                rightHelpButton.IsVisible = true;
            }
            _lastSpwaned = _columns[0];
            Column.OffSet = player.BoundingBox.Height + player.BoundingBox.Height / 4;
            leftSwipeFile = "Sounds/Effects/Air 1 swipe to left";
            rightSwipeFile = "Sounds/Effects/Air 2 swipe to right";
#if !WINDOWS

            TouchManager touch = TouchManager.Instance;
            touch.GestureOccured += touch_GestureOccured;
#endif

            SpriteLabel helperLabel = new SpriteLabel("helperLabel", content.Load<SpriteFont>("Fonts/dnk48"), String.Empty, new Vector2(RenderTarget.Width / 2, RenderTarget.Height / 2), Color.White);

            helperLabel.Y += 425;
            helperLabel.Origin = new Vector2(helperLabel.Width / 2, helperLabel.Height / 2);
            //helperLabel.Scale = new Vector2(1);
            SpriteCollection.Add(helperLabel);


            timeLabel = new SpriteLabel("timeLabel", content.Load<SpriteFont>("Fonts/dnk72"), String.Empty, Vector2.Zero, Color.Black);
            timeLabel.Position = new Vector2(RenderTarget.Width / 2, 30);
            //timeLabel.Scale = new Vector2(2);
            SpriteCollection.Add(timeLabel);

            controlLabel = new HighScoreControl("controlLabel", ControlTypes.Tap, new Vector2(0, 30));
            controlLabel.Scale = new Vector2(0.5f);
            controlLabel.X = RenderTarget.Width - controlLabel.Width - 30;

            controlLabel.IsVisible = false;
            SpriteCollection.Add(controlLabel);

            deathSound = content.Load<SoundEffect>("Sounds/Effects/DeathSound");//content.Load<XnaAudio.Song>("Sounds/DeathSound");
            leftSwipe = content.Load<SoundEffect>(leftSwipeFile); //content.Load<XnaAudio.Song>(leftSwipeFile);
            rightSwipe = content.Load<SoundEffect>(rightSwipeFile);// content.Load<XnaAudio.Song>(rightSwipeFile);
            highScoreSound = content.Load<SoundEffect>("Sounds/Effects/RD High Score");
            _level = 0;



            SpriteCollection.IsReadOnly = true;
        }

        void movingRiaFrames_AnimationFinish(object sender, EventArgs e)
        {
            player.X = fallingLocation[playerIndex].X;
            direction = Direction.None;
            player.CurrentState = CharacterState.Idle;
            if (player.Effect == SpriteEffects.FlipHorizontally)
            {
                player.Effect = SpriteEffects.None;
            }


            if (hitDuringAnimation)
            {
                player.CurrentState = CharacterState.Death;
                foreach (Column colum in _columns)
                {
                    colum.Reset();
                }

                if (Global.SFXEnabled)
                {
                    deathSound.Play();
                    //XnaAudio.MediaPlayer.Play(deathSound);
                }
            }

            hitDuringAnimation = false;

        }

        //game over event
        void deathSprite_AnimationFinish(object sender, EventArgs e)
        {
            controlLabel.IsVisible = false;
            player.CurrentState = CharacterState.Idle;
            FallingItem.FallingSpeed = startingSpeed;
            _level = 0;
            _fpsLabel.IsVisible = false;
            debug = false;
            startingAlpha = 1f;
            startingArrowAlpha = .3f;
            leftHelpButton.Color = Color.Lerp(Color.White, Color.Transparent, startingArrowAlpha);
            rightHelpButton.Color = leftHelpButton.Color;
            _drawCounter = 0;
            difficultyTime = startingDifficultyTime;
            elapsedDifficultyTime = TimeSpan.Zero;
            swipeRegister = false;
            playerIndex = 1;

            scaleStep = 0;

            positionStep = 0;
            timeLabel.Color = Color.Black;

            controlLabel.Color = Color.White;

#if !WINDOWS
            Global.ShowAds(true);
#endif

            if (Global.MusicEnabled)
            {
                MediaPlayer.Resume();
            }

            direction = Direction.None;
            ScreenManager.MainScreen = "GameOver";
            player.Effect = SpriteEffects.None;
            player.X = fallingLocation[1].X;
            isTouchAlready = true;

            timeLabel.Text = String.Format("{0} :{1}", TimeSpan.Zero.Minutes.ToString("D2"), TimeSpan.Zero.Seconds.ToString("D2"));
            timeLabel.Text = String.Empty;
            elapsedBlinkTime = TimeSpan.Zero;
            elapsedHelperTime = TimeSpan.Zero;
            elapsedGraceTime = TimeSpan.Zero;
            elapsedArrowTime = TimeSpan.Zero;

            if (Global.HighScores[Global.HighScores.Length - 1].Ticks < Global.ElapsedGameTime.Ticks)
            {
                Global.HighScores[Global.HighScores.Length - 1] = Global.ElapsedGameTime;
                Global.HighScoresType[Global.HighScoresType.Length - 1] = Global.Control;
                Global.HighScoreDates[Global.HighScoreDates.Length - 1] = DateTime.Today;

                //need to sort
                bool isSorted;
                do
                {
                    isSorted = true;
                    for (int i = 0; i < Global.HighScores.Length - 1; i++)
                    {
                        if (Global.HighScores[i].Ticks < Global.HighScores[i + 1].Ticks)
                        {
                            isSorted = false;
                            TimeSpan tempTime = Global.HighScores[i];
                            ControlTypes tempControl = Global.HighScoresType[i];
                            DateTime tempDate = Global.HighScoreDates[i];

                            Global.HighScores[i] = Global.HighScores[i + 1];
                            Global.HighScoresType[i] = Global.HighScoresType[i + 1];
                            Global.HighScoreDates[i] = Global.HighScoreDates[i + 1];

                            Global.HighScores[i + 1] = tempTime;
                            Global.HighScoresType[i + 1] = tempControl;
                            Global.HighScoreDates[i + 1] = tempDate;
                        }
                    }
                } while (!isSorted);

                if (Global.HighScores[0] == Global.ElapsedGameTime)
                {
                    Global.GotHighScore = true;
                    if (Global.SFXEnabled)
                    {
                        highScoreSound.Play();
                    }
                }

            }
        }

        Stopwatch sw = new Stopwatch();


        TimeSpan elapsedBlinkTime = new TimeSpan();
        TimeSpan blinkTime = TimeSpan.FromMilliseconds(200);

        TimeSpan elapsedHelperTime = TimeSpan.Zero;
        TimeSpan helperTime = TimeSpan.FromSeconds(3);

        TimeSpan elapsedGraceTime = TimeSpan.Zero;
        TimeSpan graceTime = TimeSpan.FromSeconds(2);

        TimeSpan timeToMove = TimeSpan.FromSeconds(1);
        Vector2 endingScale = Vector2.One;
        float scaleStep = 0f;
        float positionStep = 0f;
        public override void Update(GameTime gameTime)
        {

            controlLabel.Control = Global.Control;
            if (Global.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Global.GameplaySongs[Global.currentSongIndex]);
            }

            backGround.Image = Global.Backgrounds[Global.CurrentBackgroundIndex];
            if (Global.Control == ControlTypes.Swipe)
            {
                leftHelpButton.IsVisible = false;
                rightHelpButton.IsVisible = false;
                //SpriteCollection["helperLabel"].Cast<SpriteLabel>().Text = "Swipe your finger left\n and right to move!";
            }
            else
            {
                leftHelpButton.IsVisible = true;
                rightHelpButton.IsVisible = true;
                //SpriteCollection["helperLabel"].Cast<SpriteLabel>().Text = "Press the arrows to \nmove the character";

            }
            SpriteCollection["helperLabel"].Cast<SpriteLabel>().IsVisible = false;

            if (Global.PlayTutorial)
            {
                
                controlLabel.IsVisible = true;
                controlLabel.Scale = new Vector2(1.5f);
                controlLabel.Position = new Vector2((RenderTarget.Width - controlLabel.Width) / 2, (RenderTarget.Height - controlLabel.Height) / 2);
                SpriteCollection["helperLabel"].Cast<SpriteLabel>().IsVisible = true;
                elapsedBlinkTime += gameTime.ElapsedGameTime;
                elapsedHelperTime += gameTime.ElapsedGameTime;
                elapsedGraceTime += gameTime.ElapsedGameTime;

                if (elapsedGraceTime > timeToMove)
                {
                    controlLabel.Scale = Vector2.SmoothStep(controlLabel.Scale, new Vector2(.5f), scaleStep);
                    scaleStep += 0.03f;
                    controlLabel.Position = Vector2.SmoothStep(controlLabel.Position, new Vector2(RenderTarget.Width - controlLabel.Width - 30, 30), positionStep);
                    positionStep += 0.03f;
                }

                if (elapsedBlinkTime > blinkTime)
                {
                    SpriteCollection["helperLabel"].Color = SpriteCollection["helperLabel"].Color == Color.White ? Color.Red : Color.White;

                    elapsedBlinkTime = TimeSpan.Zero;
                }

                if (elapsedHelperTime > helperTime)
                {
                    SpriteCollection["helperLabel"].Cast<SpriteLabel>().IsVisible = false;
                }

                if (elapsedGraceTime > graceTime)
                {
                    Global.PlayTutorial = false;
                }

            }
            else
            {
                if (player.CurrentState != CharacterState.Death)
                {


                    Global.ElapsedGameTime += gameTime.ElapsedGameTime;
                    elapsedSpawningTime += gameTime.ElapsedGameTime;
                    elapsedDifficultyTime += gameTime.ElapsedGameTime;
                    elapsedArrowTime += gameTime.ElapsedGameTime;
                    timeLabel.Text = String.Format("{0} :{1}", Global.ElapsedGameTime.Minutes.ToString("D2"), Global.ElapsedGameTime.Seconds.ToString("D2"));
                    timeLabel.Origin = new Vector2(timeLabel.Width / 2, 0);
                    controlLabel.Control = Global.Control;
                    controlLabel.IsVisible = true;
                    if (elapsedArrowTime >= arrowFadeTime)
                    {
                        leftHelpButton.Color = Color.Lerp(Color.White, Color.Transparent, startingArrowAlpha);
                        rightHelpButton.Color = leftHelpButton.Color;
                        startingArrowAlpha += 0.02f;
                    }

                    if (elapsedDifficultyTime >= difficultyTime)
                    {
                        elapsedDifficultyTime = TimeSpan.Zero;
                        _level++;

                        switch (_level)
                        {
                            case 0:
                                FallingItem.FallingSpeed = 20;
                                difficultyTime = startingDifficultyTime;
                                break;
                            case 1:
                                FallingItem.FallingSpeed = 23;
                                difficultyTime = TimeSpan.FromSeconds(20f);
                                //badItems.Add(new Item(itemTextures[1], ItemType.Bad, true));
                                break;
                            case 2:
                                FallingItem.FallingSpeed = 24;
                                //badItems.Add(new Item(itemTextures[2], ItemType.Bad, true));
                                break;
                            case 3:
                                FallingItem.FallingSpeed = 26;
                                //badItems.Add(new Item(itemTextures[3], ItemType.Bad, true));
                                difficultyTime -= TimeSpan.FromSeconds(1f);
                                break;
                            case 4:
                                FallingItem.FallingSpeed = 28;
                                //badItems.Add(new Item(itemTextures[4], ItemType.Bad, true));
                                break;
                            case 5:
                                FallingItem.FallingSpeed = 30;
                                //badItems.Add(new Item(itemTextures[5], ItemType.Bad, true));
                                break;
                            case 6:
                                FallingItem.FallingSpeed = 32;
                                //badItems.Add(new Item(itemTextures[6], ItemType.Bad, true));
                                difficultyTime -= TimeSpan.FromSeconds(1f);
                                break;
                            case 7:
                                FallingItem.FallingSpeed = 34;
                                //badItems.Add(new Item(itemTextures[7], ItemType.Bad, true));
                                break;
                            case 8:
                                FallingItem.FallingSpeed = 36;
                                //badItems.Add(new Item(itemTextures[8], ItemType.Bad, true));
                                break;
                            case 9:
                                FallingItem.FallingSpeed = 38;
                                //badItems.Add(new Item(itemTextures[9], ItemType.Bad, true));
                                difficultyTime -= TimeSpan.FromSeconds(1f);
                                break;
                            case 10:
                                FallingItem.FallingSpeed = 40;
                                //badItems.Add(new Item(itemTextures[10], ItemType.Bad, true));
                                break;
                            case 11:
                                FallingItem.FallingSpeed = 42;
                                //badItems.Add(new Item(itemTextures[11], ItemType.Bad, true));
                                break;
                            case 12:
                                FallingItem.FallingSpeed = 44;
                                //badItems.Add(new Item(itemTextures[12], ItemType.Bad, true));
                                break;
                            case 13:
                                FallingItem.FallingSpeed = 46;
                                //badItems.Add(new Item(itemTextures[13], ItemType.Bad, true));
                                break;
                            case 14:
                                FallingItem.FallingSpeed = 48;
                                //badItems.Add(new Item(itemTextures[14], ItemType.Bad, true));
                                break;
                            default:
                                FallingItem.FallingSpeed = 50;
                                //badItems.Add(new Item(itemTextures[15], ItemType.Bad, true));
                                break;
                        }
                    }
#if WINDOWS
                    if (debug)
                    {

                        _elapsedDrawTime += gameTime.ElapsedGameTime;
                        if (_elapsedDrawTime > _drawTime)
                        {
                            _elapsedDrawTime = TimeSpan.Zero;
                            _fpsLabel.Text = String.Format("FPS: {0} Speed: {1}", _drawCounter, FallingItem.FallingSpeed);
                            _drawCounter = 0;
                        }
                        _fpsLabel.IsVisible = true;
                    }
#endif
                    if (_lastSpwaned.CanSpawn)
                    {
                        SpawnFallingObjects();
                    }

                    if (_level >= 1)
                    {
                        if (startingAlpha > endingAlpha)
                        {
                            startingAlpha -= 0.02f;
                        }
                    }
                    timeLabel.Color = new Color(0, 0, 0, startingAlpha);
                    controlLabel.Color = new Color(255, 255, 255, startingAlpha);

                    //checks if any items hit the ground
                    for (int i = 0; i < _columns.Length; i++)
                    {
                        if (_columns[i].FallingItems.Count > 0)
                        {
                            FallingItem item = _columns[i].FallingItems[0];
                            if (item.BoundingBox.Bottom > player.BoundingBox.Top + player.BoundingBox.Height / 2)
                            {
                                item.Fade = true;
                            }
                        }

                    }

                    if (_columns[playerIndex].CheckHit(player))
                    {
                        if (player.CurrentState == CharacterState.Idle)
                        {
                            player.CurrentState = CharacterState.Death;
                            foreach (Column colum in _columns)
                            {
                                colum.Reset();
                            }

                            if (Global.SFXEnabled)
                            {
                                deathSound.Play();

                            }
                        }
                        else
                        {
                            hitDuringAnimation = true;
                        }
                    }
                }
            }

            if (player.CurrentState != CharacterState.Death)
            {
                switch (direction)
                {
                    case Direction.None:
                        player.CurrentState = CharacterState.Idle;

                        if (Global.Control == ControlTypes.Tap)
                        {
                            handleInput();
                            if (direction != Direction.None)
                            {
                                sw.Start();
                                player.CurrentState = CharacterState.Moving;
                            }
                            if (direction == Direction.Right)
                            {
                                player.Effect = SpriteEffects.FlipHorizontally;
                            }
                            else if (direction == Direction.Left)
                            {
                                player.Effect = SpriteEffects.None;
                            }
                        }

                        break;

                    case Direction.Left:

                        CheckLocation();

                        break;
                    case Direction.Right:
                        CheckLocation();

                        break;
                }
            }

            base.Update(gameTime);
        }

        private void SpawnFallingObjects()
        {
            int numberOfItems = _random.Next(1, 100);
            _random.Clear();

            int index = _random.Next(_columns.Length);
            Column column = _columns[index];

            if (numberOfItems > 30)
            {
                Column column1;

                int index1 = _random.Next(_columns.Length);
                int index1min = MathHelper.Clamp(index1 - 1, 0, _columns.Length - 1).ToInt();
                int index1max = MathHelper.Clamp(index1 + 1, 0, _columns.Length - 1).ToInt();
                while (index == index1min || index == index1max)
                {
                    index1 = _random.Next(_columns.Length);
                    index1min = MathHelper.Clamp(index1 - 1, 0, _columns.Length - 1).ToInt();
                    index1max = MathHelper.Clamp(index1 + 1, 0, _columns.Length - 1).ToInt();
                }

                column1 = _columns[index1];
                column1.Pop();
            }

            _lastSpwaned = column;
            column.Pop();
            _random.Clear();
        }

        /// <summary>
        /// Checks if the player has reached the location of the next column
        /// </summary>
        /// <param name="index"></param>
        private void CheckLocation()
        {
            if (playerIndex < 0)
            {
                playerIndex = 0;
            }
            else if (playerIndex > fallingLocation.Length - 1)
            {
                playerIndex = fallingLocation.Length - 1;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _drawCounter++;

            base.Draw(spriteBatch);
        }

        bool isTouchAlready = true;

        private void handleInput()
        {
#if WINDOWS
            KeyBoardManager keybaord = KeyBoardManager.Instance;

            if (keybaord.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left) && playerIndex != 0)
            {
                direction = Direction.Left;
            }
            else if (keybaord.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right) && playerIndex != fallingLocation.Length - 1)
            {
                direction = Direction.Right;
            }
            else if (keybaord.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
            {
                debug = !debug;
            }

            
#else
            
            TouchManager touch = TouchManager.Instance;

            if(touch.TouchPoints.Count == 1 && !isTouchAlready)
            {
                isTouchAlready = true;
                foreach(var touchPoint in touch.TouchPoints)
                {
                    Vector2 tapLocationTranslation = touchPoint.Position / Scale;

                    if (tapLocationTranslation.X < RenderTarget.Width / 2)
                    {
                        direction = Direction.Left;
                    }
                    else
                    {
                        direction = Direction.Right;
                    }
                }
            }
            else if(touch.TouchPoints.Count == 0)
            {
                isTouchAlready = false;
            }
#endif

            

            switch (direction)
            {
                case Direction.Right:

                    playerIndex++;

                    if (playerIndex < 0 || playerIndex > fallingLocation.Length - 1)
                    {
                        player.CurrentState = CharacterState.Idle;
                        direction = Direction.None;
                    }
                    else
                    {
                        player.CurrentState = CharacterState.Moving;
                        player.Effect = SpriteEffects.FlipHorizontally;

                        if (Global.SFXEnabled)
                        {
                            rightSwipe.Play();
                            //XnaAudio.MediaPlayer.Play(rightSwipe);
                        }
                    }

                    break;
                case Direction.Left:

                    playerIndex--;
                    if (playerIndex < 0 || playerIndex > fallingLocation.Length - 1)
                    {
                        player.CurrentState = CharacterState.Idle;
                        direction = Direction.None;
                    }
                    else
                    {
                        player.CurrentState = CharacterState.Moving;
                        player.Effect = SpriteEffects.None;

                        if (Global.SFXEnabled)
                        {
                            leftSwipe.Play();
                            //XnaAudio.MediaPlayer.Play(leftSwipe);
                        }
                    }
                    break;
                default:
                    //do nothing
                    break;
            }
            CheckLocation();


            if (direction == Direction.Right)
            {
                player.X = fallingLocation[playerIndex].X;
            }
        }

#if !WINDOWS

        float deltaSwipe = 0;
        void touch_GestureOccured(object sender, ZeroLibrary.Xna.Args.GestureArgs e)
        {
            if (player.CurrentState != CharacterState.Death)
            {
                if (ScreenManager.MainScreen == Name)
                {
                    GestureSample gesture = e.Gesture;

                    if (gesture.GestureType == GestureType.DragComplete || gesture.GestureType == GestureType.Flick)
                    {
                        //direction = directionToGo;
                        swipeRegister = false;
                        deltaSwipe = 0;
                    }

                    if (player.CurrentState == CharacterState.Idle)
                    {
                        if (Global.Control == ControlTypes.Swipe)
                        {

                            if (gesture.GestureType == GestureType.HorizontalDrag || gesture.GestureType == GestureType.FreeDrag)
                            {
                                int distance = 5;
                                deltaSwipe += gesture.Delta.X;


                                if (Math.Abs(deltaSwipe) > distance && swipeRegister == false)
                                {
                                    swipeRegister = true;
                                    direction = deltaSwipe > distance ? Direction.Right : Direction.Left;
                                }
                            }
                        }
                        

                        switch (direction)
                        {
                            case Direction.Right:

                                playerIndex++;

                                if (playerIndex < 0 || playerIndex > fallingLocation.Length - 1)
                                {
                                    player.CurrentState = CharacterState.Idle;
                                }
                                else
                                {
                                    player.CurrentState = CharacterState.Moving;
                                    player.Effect = SpriteEffects.FlipHorizontally;

                                    if (Global.SFXEnabled)
                                    {
                                        rightSwipe.Play();
                                        //XnaAudio.MediaPlayer.Play(rightSwipe);
                                    }
                                }

                                break;
                            case Direction.Left:

                                playerIndex--;
                                if (playerIndex < 0 || playerIndex > fallingLocation.Length - 1)
                                {
                                    player.CurrentState = CharacterState.Idle;
                                }
                                else
                                {
                                    player.CurrentState = CharacterState.Moving;
                                    player.Effect = SpriteEffects.None;

                                    if (Global.SFXEnabled)
                                    {
                                        leftSwipe.Play();
                                        //XnaAudio.MediaPlayer.Play(leftSwipe);
                                    }
                                }
                                break;
                            default:
                                //do nothing
                                break;
                        }

                        CheckLocation();

                        if (direction == Direction.Right)
                        {
                            player.X = fallingLocation[playerIndex].X;
                        }
                    }


                }

            }
        }
#endif




    }
}