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
    class LScore : ScoreBoard
    {
        public static int score;
        public static int gameswon;
        public int positionholder;
        public int[] positionsocket;

        // Initalizes Score Of Player 1 //
        public LScore()
        {
            positionholder = 0;
            score = 0;
            gameswon = 0;
            positionsocket = new int[4];
            init();
        }

        // Increments Player 1's Score //
        public static void incrementLScore()
        {
            score++;
        }

        public static void incrementLGames()
        {
            gameswon++;
        }
        private void init()
        {
            positionsocket[0] = -1;
            positionsocket[1] = 0;
            positionsocket[2] = 1;
            positionsocket[3] = 2;
        }
    }
}
