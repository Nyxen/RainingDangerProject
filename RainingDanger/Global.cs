using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using RainingDanger.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RainingDanger
{
    public static class Global
    {
        public static TimeSpan ElapsedGameTime;
        public static bool MusicEnabled;
        public static bool SFXEnabled;
        public static Texture2D Pixel;

        public static bool SelectControl =  false;
        public static ControlTypes Control;
        public static int gameCount;

        public static List<Texture2D> Backgrounds = new List<Texture2D>();
        public static List<Song> GameplaySongs = new List<Song>();
        public static int CurrentBackgroundIndex = 0;
        public static int currentSongIndex = 0;

        /// <summary>
        /// Index 0 should be the highest score
        /// </summary>
        public static TimeSpan[] HighScores = new TimeSpan[5];
        public static ControlTypes[] HighScoresType = new ControlTypes[5];
        public static DateTime[] HighScoreDates = new DateTime[5];
        


        public static bool PlayTutorial;

        public static bool GotHighScore = false;

        public static Action<bool> ShowAds;
        public static Action LoadAds;

        public static Song BackgroundSong;


        public static string AndroidAdKey = "ca-app-pub-9891692935787220/1430007197";
        public static string IosAdKey =     "ca-app-pub-9891692935787220/4383473598";

    }
}