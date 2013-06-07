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
    class TimeCalculator
    {

        public int currenttime;
        public int lasttime;
        public int pausedtime;

        public static bool softpaused;
        public static bool hardpaused;

        public TimeCalculator()
        {
            currenttime = 0;
            lasttime = 0;
            pausedtime = 0;
            softpaused = false;
            hardpaused = false;
        }
        
        public static void softResume()
        {
            if (softpaused)
                softpaused = false;
        }
        public static void softPause()
        {
            if(!softpaused)
            {
                softpaused = true;
                Jukebox.playBeginSound();
            }
        }
        public static void hardResume()
        {
            if (hardpaused)
            {
                hardpaused = false;
                MediaPlayer.Resume();
            }
        }
        public static void hardPause()
        {
            if (!softpaused)
            {
                hardpaused = true;
                MediaPlayer.Pause();
            }
        }
        public static int getTime(GameTime gameTime, int elapsed)
        {
            return ((int)gameTime.TotalGameTime.TotalSeconds - elapsed);
        }

    }
}
