using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace First_Game
{
    class Paddle : PongSprite // "extends" PongSprite
    {
        public Rectangle collisionRect;
        public Rectangle extreamTop; // use for more complicated hitbox
        public Rectangle extreamBot; // // // // // // // // // // // // 
        public bool recentlyHit;
        public int resetdistance;


        public Paddle()
        {
            recentlyHit = false;
        }
        // Set Collision Rectangles && Position Of The Rectangle Of The Paddle //
        public void setCollisionRect(Rectangle rect)
        {
            collisionRect = rect;
            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }
        public void setReseter(int distance)
        {
            resetdistance = (int)this.position.X + distance;
        }
    }
}
