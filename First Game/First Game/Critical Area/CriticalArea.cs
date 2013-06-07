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
using First_Game.Main;

namespace First_Game
{
    public enum Intersection { Left, Right, Top, Bot, None, LeftPaddle, RightPaddle };

    class CriticalArea : PongSprite
    {
        // All Critical Areas Are Represented As Rectangular Hitboxes //
        private Rectangle LeftBound;
        private Rectangle RightBound;
        private Rectangle TopBound;
        private Rectangle BotBound;

        private Rectangle LeftPaddle;
        private Rectangle RightPaddle;

        public CriticalArea(Rectangle left, Rectangle right, Rectangle top, Rectangle bot)
        {
            LeftBound = left;
            RightBound = right;
            TopBound = top;
            BotBound = bot;
        }

        // Checks All Critical Area Hitboxes Against The Ball's Hitbox //
        public int checkCrits(Ball ball)
        {
            if (ball.collisionRect.Intersects(LeftBound))
                return (int)Intersection.Left;

            else if (ball.collisionRect.Intersects(RightBound))
                return (int)Intersection.Right;

            else if (ball.collisionRect.Intersects(TopBound))
                return (int)Intersection.Top;

            else if (ball.collisionRect.Intersects(BotBound))
                return (int)Intersection.Bot;

            else if (ball.collisionRect.Intersects(LeftPaddle))
                return (int)Intersection.LeftPaddle;

            else if (ball.collisionRect.Intersects(RightPaddle))
                return (int)Intersection.RightPaddle;

            else
                return (int)Intersection.None;

        }
        // Takes In Both Paddle Hitboxes And Stores Them As Critical Areas //
        public void setPaddles(Rectangle LPHitbox, Rectangle RPHitbox)
        {
            LeftPaddle = LPHitbox;
            RightPaddle = RPHitbox;
        }
    }
}
