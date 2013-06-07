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
    class ScoreBoard : PongSprite
    {
        public Vector2 stringpos;
        public SpriteFont font;
        public String drawstring;

        // Game Time //
        public static int gtime;

        // Initial Time Is 99 //
        public ScoreBoard()
        {
            gtime = 99;
        }

        // Sets Position Of The Timer //
        public void setPosition(Vector2 pos)
        {
            stringpos = pos;
        }

        //Sets The Font Of The Timer //
        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        // Updates The Time By Converting The Integer To A String So It Can Be Drawn To The Screen //
        public void update(int score)
        {
            drawstring = score.ToString();
        }
    }
}
