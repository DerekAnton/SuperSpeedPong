using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace First_Game
{
    class Jukebox
    {
        // Sound Effects Need .WAV's
        public static SoundEffect beginsound;
        public static SoundEffect paddlehit;
        public static SoundEffect score;
        public static SoundEffect wallhit;

        //WMA or MP3 For Songs
        public Song maintheme;

        public Jukebox()
        {
            maintheme = null;
            beginsound = null;
            paddlehit = null;
            score = null;
            wallhit = null;
        }
        // Theme Music
        public void playMainTheme()
        {
            MediaPlayer.Play(maintheme);
        }

        // Sound Effects
        public static void playBeginSound()
        {
            beginsound.Play();
        }
        public static void playPaddleHit()
        {
            paddlehit.Play();
        }
        public static void playWallHit()
        {
            wallhit.Play();
        }
        public static void playScore()
        {
            score.Play();
        }
    }
}
