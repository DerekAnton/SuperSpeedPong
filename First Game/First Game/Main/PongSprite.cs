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
    // Generic Parent Class With Nessisary Variables To Make A Game //
    class PongSprite
    {
        // Allows For SpriteSheets, Position, BoudingRectangles (Which Are Different From Collision Rectangles)
        // And The Frame Amount Of An Animated Sprite //
        public Texture2D spritesheet;
        public Vector2 position;
        public Rectangle boundingrect;
        public int frameamount;
        public int sheetlength = 9;
        private int sheetcounter = 0;

        public PongSprite()
        { }
        // Animate Logic //
        public void animate()
        {
            boundingrect.X += boundingrect.Width;
        }

        public void animateSheet() // specific to 3 - 2 - 1 // mainly for testing //
        {
            if (boundingrect.X < 3 * boundingrect.Width)
                boundingrect.X += boundingrect.Width;
            else if (sheetcounter == 8 && boundingrect.X >= 9 * boundingrect.Width)
            {
                boundingrect.X = 0;
                boundingrect.Y = 0;
                sheetcounter = 0;
            }
            else
            {
                boundingrect.X = 0;
                boundingrect.Y += boundingrect.Height;
                sheetcounter++;
            }
        }

        // Initalizes A PongSprite With It's Texture2D, Bounding Rect, And Frame Amount
        public void setImg(Texture2D sprite, Rectangle boundingrect, int frameamount)
        {
            spritesheet = sprite;
            this.boundingrect = boundingrect;
            this.frameamount = frameamount;
        }

        // Sets The Vector Of A PongSprite //
        public void setVector(Vector2 pos)
        {
            position = pos;
        }
        
        // Increases The Y Of A PongSprite //
        public void alterVectorY(float val)
        {
            position.Y += val;
        }
    }
}
