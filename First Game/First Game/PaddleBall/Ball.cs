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
    // Two Public Enumerations For Movement //
    public enum Direction { Left, Right};
    public enum Diagonal { UpLeft, DownLeft, UpRight, DownRight };
    
    class Ball : PongSprite // "extends" PongSprite
    {
        public Rectangle collisionRect;
        private int ballmovespeed;
        
        // Last X And Y Are Essential In The Movement Logic, Allowing For Math To Be Done To Figure Out The Current Direction Of The Ball //
        public float lastX;
        public float lastY;

        // Integers That Will Hold Enumeration Values For Immediated Direction (Left || Right) And Diagonal Directions //
        private int direction = (int)Direction.Right;
        public int diagonal = (int)Diagonal.DownRight;

        // Unused Random Object (For Random Integer Math) //
        private Random random = new Random();

        public Ball()
        {
            ballmovespeed = 200;
            lastX = 0;
            lastY = 0;
        }

        // Sets the Collision Rectangle As Well As It's Position //
        public void setCollisionRect(Rectangle rect)
        {
            collisionRect = rect;
            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }
        // Increases Movement Speed Of Ball After Each Successful Paddle Hit //
        private void increaseSpeed()
        {
            ballmovespeed += 20;
        }

        // Main Ball Movement Logic, Calls Sub Functions changeAngel() && move() //
        // Checks If The Ball Is Moving Left || Right, Then Checks The Appropriate Paddle Hitbox For Collision //
        public void moveBall(double gametime, Paddle Rpaddle, Paddle Lpaddle, CriticalArea crit)
        {
            if (direction == (int) Direction.Right)
            {
                if (!this.collisionRect.Intersects(Rpaddle.collisionRect))
                {
                        changeAngle(crit.checkCrits(this));
                        move(gametime);
                }
                else
                        direction = (int)Direction.Left;
            }
            if (direction == (int)Direction.Left)
            {
                if (!this.collisionRect.Intersects(Lpaddle.collisionRect))
                {
                    changeAngle(crit.checkCrits(this));
                    move(gametime);
                }
                else
                    direction = (int)Direction.Right;
            }
        }
        // Angular Collision Logic. Passed An Enumerator That Denotes What Critical Area Was Hit (Either A Wall, Paddle, Or Score Zone) //
        private void changeAngle(int condition)
        {
            if (condition == (int)Intersection.Left)
            {
                Jukebox.playScore();
                RScore.incrementRScore();
                ballmovespeed = 200;
                this.setVector(new Vector2(320, 240));

                if (!TimeCalculator.softpaused)
                {
                    TimeCalculator.softPause();
                }
            }
            else if (condition == (int)Intersection.Right)    
            {
                Jukebox.playScore();
                LScore.incrementLScore();
                ballmovespeed = 200;
                this.setVector(new Vector2(320, 240));

                if (!TimeCalculator.softpaused)
                {
                    TimeCalculator.softPause();
                }
            }
            else if (condition == (int)Intersection.Top)
            {
                Jukebox.playWallHit();
                if (lastX - position.X < 0)
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.UpLeft;
                    else
                        diagonal = (int)Diagonal.DownRight;
                }
                else
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.UpRight;
                    else
                        diagonal = (int)Diagonal.DownLeft; 
                }
            }
            else  if (condition == (int)Intersection.Bot)
            {
                Jukebox.playWallHit();
                if (lastX - position.X < 0)
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.UpRight; 
                    else
                        diagonal = (int)Diagonal.DownLeft; 
                }
                else
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.UpLeft;
                    else
                        diagonal = (int)Diagonal.UpLeft; 
                }
            }
            else if (condition == (int)Intersection.LeftPaddle)
            {
                Jukebox.playPaddleHit();
                if (lastX - position.X < 0)
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.DownLeft;
                    else
                        diagonal = (int)Diagonal.UpLeft;
                }
                else
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.DownRight;
                    else
                        diagonal = (int)Diagonal.UpRight;
                }

                increaseSpeed();
            }
            else if (condition == (int)Intersection.RightPaddle)
            {
                Jukebox.playPaddleHit();
                if (lastX - position.X < 0)
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.DownLeft;
                    else
                        diagonal = (int)Diagonal.UpLeft;
                }
                else
                {
                    if (lastY - position.Y < 0)
                        diagonal = (int)Diagonal.UpRight;
                    else
                        diagonal = (int)Diagonal.DownRight;
                }

                increaseSpeed();
            }
            else { }
        }
        // Actually Does The Movement Math And Applies It To The Ball //
        private void move(double gametime)
        {
            if (diagonal == (int)Diagonal.UpLeft)
            {
                lastX = position.X;
                lastY = position.Y;
                position.X -= ballmovespeed * (float)gametime;
                position.Y -= ballmovespeed * (float)gametime;
            }
            if (diagonal == (int)Diagonal.DownLeft)
            {
                lastX = position.X;
                lastY = position.Y;
                position.X -= ballmovespeed * (float)gametime;
                position.Y += ballmovespeed * (float)gametime;
            }
            if (diagonal == (int)Diagonal.UpRight)
            {
                lastX = position.X;
                lastY = position.Y;
                position.X += (ballmovespeed * (float)gametime);
                position.Y -= ballmovespeed * (float)gametime;
            }
            if (diagonal == (int)Diagonal.DownRight)
            {
                lastX = position.X;
                lastY = position.Y;
                position.X += ballmovespeed * (float)gametime;
                position.Y += ballmovespeed * (float)gametime;
            }
            else
            { }
        }
    }
}
