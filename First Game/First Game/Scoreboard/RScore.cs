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
    class RScore : ScoreBoard
    {
        public static int score;
        public static int gameswon;
        public int positionholder;
        public int[] positionsocket;

        // Initalizes Score Of Player 2 //
        public RScore()
        {
            positionholder = 0;
            score = 0;
            gameswon = 0;
            positionsocket = new int[4];
            init();
        }

        // Increments Player 2's Score //
        public static void incrementRScore()
        {
            score++;
        }

        public static void incrementRGames()
        {
            gameswon++;
        }
        private void init()
        {
            positionsocket[0] =  6;
            positionsocket[1] =  5;
            positionsocket[2] =  4;
            positionsocket[3] =  3; 
        }
    }
}
