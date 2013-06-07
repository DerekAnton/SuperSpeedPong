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
    public enum GameState { InGame, MainMenu, PauseScreen };

    public class System : Microsoft.Xna.Framework.Game
    {
        // Total Second Each Game Will Last //
        public static int GAME_LENGTH = 90; 

        public float paddlemovespeed;
        public int gamestate;

        private int elapsed = 0;
        private int lastpausedtime = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        SpriteFont timefont;
        Texture2D background;
        Texture2D pausescreen;
        Vector2 backgroundVector;
        KeyboardState keyState;
        CriticalArea critical;
        
        TimeCalculator hardpausedcalc;
        TimeCalculator softpausedcalc;
        TimeCalculator countdowntimer;

        PongSprite[] animatedcrystalarr = new PongSprite[6];
        PongSprite[] outlinedcrystalarr = new PongSprite[6];

        PongSprite countdown = new PongSprite();

        Ball ball = new Ball();
        
        Paddle Lpaddle = new Paddle();
        Paddle Rpaddle = new Paddle();

        ScoreBoard scoreboard = new ScoreBoard();
        DisplayedTime displayedtime = new DisplayedTime();
        LScore lscore = new LScore();
        RScore rscore = new RScore();

        Jukebox jukebox = new Jukebox();

        // System Construction //
        public System()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Basic Init //
        protected override void Initialize()
        {
            MediaPlayer.IsRepeating = true;

            hardpausedcalc = new TimeCalculator();
            softpausedcalc = new TimeCalculator();
            countdowntimer = new TimeCalculator();

            countdowntimer.pausedtime = 1;

            backgroundVector = new Vector2(0, 0);
            Lpaddle.setVector(new Vector2(0, 185));
            Rpaddle.setVector(new Vector2(618, 185));
            ball.setVector(new Vector2(320,240));

            for (int counter = 0; counter < 6; counter++)
            {
                animatedcrystalarr[counter] = new PongSprite();
                outlinedcrystalarr[counter] = new PongSprite();
            }

            paddlemovespeed = 650;

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            base.Initialize();
        }

        // Content Loading Method (Via Content Pipeline) //
        protected override void LoadContent()
        {
            // Set The Spritebatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Music & Sound Effects
            jukebox.maintheme = Content.Load<Song>("Audio/MainTheme");
            Jukebox.beginsound = Content.Load<SoundEffect>("Audio/beginsound");
            Jukebox.wallhit = Content.Load<SoundEffect>("Audio/WallHit");
            Jukebox.paddlehit = Content.Load<SoundEffect>("Audio/PaddleHit");
            Jukebox.score = Content.Load<SoundEffect>("Audio/Score");
           
            jukebox.playMainTheme();

            // Load Fonts //
            font = Content.Load<SpriteFont>("Fonts/Font");
            timefont = Content.Load<SpriteFont>("Fonts/TimeFont");

            initScore(font, timefont);

            // Animated Count Down //
            countdown.setImg(Content.Load<Texture2D>("Sprites/countdownShort"), new Rectangle( 0, 0, 200, 200), 3);

            // Load Paddles //
            Lpaddle.setImg(Content.Load<Texture2D>("Sprites/Paddle"), new Rectangle(0,50,22,94), 1);
            Rpaddle.setImg(Content.Load<Texture2D>("Sprites/Paddle"), new Rectangle(618,50,22,94), 1);

            // Load Background & Pause Screen //
            background = Content.Load<Texture2D>("Sprites/Background");
            pausescreen = Content.Load<Texture2D>("Sprites/PauseScreen");

            // Load Scoreboards //
            scoreboard.setImg(Content.Load<Texture2D>("Sprites/Pallet"), new Rectangle(0, 0, 44, 1098), 1);
            lscore.setImg(Content.Load<Texture2D>("Sprites/Player1Score"), new Rectangle(0, 0, 44, 50), 1);
            rscore.setImg(Content.Load<Texture2D>("Sprites/Player2Score"), new Rectangle(0, 0, 44, 50), 1);

            // Load & Prepare The Crystals And Their Respective Containers //
            for (int counter = 0; counter < 6; counter++)
            {
                animatedcrystalarr[counter].setImg(Content.Load<Texture2D>("Sprites/blackedit"), new Rectangle(0, 0, 21, 16), 32);
                outlinedcrystalarr[counter].setImg(Content.Load<Texture2D>("Sprites/outlinedblackemerald"), new Rectangle(0, 0, 21, 16), 1);
                if (counter < 3)
                {
                    outlinedcrystalarr[counter].setVector(new Vector2(60 + lscore.positionholder, 5));
                    lscore.positionholder += outlinedcrystalarr[counter].boundingrect.Width;
                    animatedcrystalarr[counter].setVector(outlinedcrystalarr[counter].position);
                }
                else
                {
                    outlinedcrystalarr[counter].setVector(new Vector2(520 + rscore.positionholder, 5));
                    rscore.positionholder += outlinedcrystalarr[counter].boundingrect.Width;
                    animatedcrystalarr[counter].setVector(outlinedcrystalarr[counter].position);
                }
            }

            rscore.setVector( new Vector2(635 - rscore.boundingrect.Width, 0));

            // Different Sizes Of Sphere //
            // ball.setImg(Content.Load<Texture2D>("Sprites/Large_Sized_Sphere"), new Rectangle(0, 0, 64, 63), 32); //Large Ball
            // ball.setImg(Content.Load<Texture2D>("Sprites/Small_Sized_Sphere"), new Rectangle(0, 0, 16, 17), 32); // Small Ball
            ball.setImg(Content.Load<Texture2D>("Sprites/Medium_Sized_Sphere"), new Rectangle(0, 0, 32, 32), 32); // Medium Ball

            // Set Collision Rectangles //
            ball.setCollisionRect(ball.boundingrect);
            Lpaddle.setCollisionRect(Lpaddle.boundingrect);
            Rpaddle.setCollisionRect(Rpaddle.boundingrect);

            // Critical Areas Set //
            // Example: A Critical Area Would Be 1 Of The 4 Sides Of The Bounding Rectangle Of The Playing Field OR The Paddle's Hitboxes
            critical = new CriticalArea(new Rectangle(0, 0, 1, 480), new Rectangle(640, 0, 1, 480), new Rectangle(0, 43, 640, 1), new Rectangle(0, 481, 640, 1));

            TimeCalculator.softPause();

        }

        // Unload Function //
        protected override void UnloadContent()
        {

        }

        // Main Update Loop //
        protected override void Update(GameTime gameTime)
        {
            checkKeys(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            // Updates Collision Rectangles After A Move //
            ball.setCollisionRect(ball.boundingrect);
            Lpaddle.setCollisionRect(Lpaddle.boundingrect);
            Rpaddle.setCollisionRect(Rpaddle.boundingrect);

            // Time Calculation //
            updateTimes(gameTime);

            // Updates Score & Time //
            lscore.update(LScore.score);
            rscore.update(RScore.score);
            scoreboard.update(ScoreBoard.gtime);

            base.Update(gameTime);
        }

        // Main Draw Loop //
        protected override void Draw(GameTime gameTime)
        {

                GraphicsDevice.Clear(Color.SlateGray);
                spriteBatch.Begin();  // Begin                                      // drawing is done inbetween begin and end functions.

                // Passes Critical Areas Collision Rectangles Of The Paddles // 
                critical.setPaddles(Lpaddle.collisionRect, Rpaddle.collisionRect);

                // Draw Background //
                spriteBatch.Draw(background, backgroundVector, Color.White);

                // Draw Scoreboard // 
                spriteBatch.Draw(scoreboard.spritesheet, scoreboard.position, Color.White);
                spriteBatch.Draw(rscore.spritesheet, rscore.position, Color.White);
                spriteBatch.Draw(lscore.spritesheet, lscore.position, Color.White);

                // Draw Loop For Crystals //
                for (int counter = 0; counter < 6; counter++)
                {
                    spriteBatch.Draw(outlinedcrystalarr[counter].spritesheet, outlinedcrystalarr[counter].position, outlinedcrystalarr[counter].boundingrect, Color.White);

                    if (counter < 3 && lscore.positionsocket[LScore.gameswon] >= counter)
                        spriteBatch.Draw(animatedcrystalarr[counter].spritesheet, animatedcrystalarr[counter].position, animatedcrystalarr[counter].boundingrect, Color.White);
                    else if (counter >= 3 && Math.Abs(rscore.positionsocket[RScore.gameswon]) <= counter)
                        spriteBatch.Draw(animatedcrystalarr[counter].spritesheet, animatedcrystalarr[counter].position, animatedcrystalarr[counter].boundingrect, Color.White);
                    else { }
                }

                // Draw Integer Scores & Time // 
                spriteBatch.DrawString(lscore.font, lscore.drawstring, lscore.stringpos, Color.SteelBlue);
                spriteBatch.DrawString(rscore.font, rscore.drawstring, rscore.stringpos, Color.SteelBlue);
                spriteBatch.DrawString(scoreboard.font, scoreboard.drawstring, scoreboard.stringpos, Color.PaleGreen);

                // Draw Paddles // 
                spriteBatch.Draw(Lpaddle.spritesheet, Lpaddle.position, Color.White);
                spriteBatch.Draw(Rpaddle.spritesheet, Rpaddle.position, Color.White);

                // Draw Ball //
                spriteBatch.Draw(ball.spritesheet, ball.position, ball.boundingrect, Color.White);

         
                if (!TimeCalculator.hardpaused && !TimeCalculator.softpaused)
                {
                    // Ball Movement Logic //
                    ball.moveBall(gameTime.ElapsedGameTime.TotalSeconds, Rpaddle, Lpaddle, critical);

                    // Animate Ball && Crystals//
                    animateSprites(gameTime);
                    spriteBatch.End(); // End
                    base.Draw(gameTime);
                }
                else if (TimeCalculator.hardpaused)
                {
                    spriteBatch.Draw(pausescreen, new Rectangle(0, 0, 640, 420), Color.White);
                    spriteBatch.End();
                    base.Draw(gameTime);
                }             
                else if (TimeCalculator.softpaused && !TimeCalculator.hardpaused)
                {
                    // Count Down //
                    spriteBatch.Draw(countdown.spritesheet, new Vector2(225,150), countdown.boundingrect, Color.White);
                    animateSprites(gameTime);
                    spriteBatch.End();
                    base.Draw(gameTime);
                }
                else { }
        }

        // Check Keyboard Input //
        private void checkKeys(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            // KeyPresses Allow For Paddle Movement //
            if (keyState.IsKeyDown(Keys.W))
            {
                if (!TimeCalculator.hardpaused)
                {
                    Lpaddle.alterVectorY(-paddlemovespeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (Lpaddle.position.Y < 0 + lscore.boundingrect.Width)
                        Lpaddle.position.Y = 1 + lscore.boundingrect.Width;
                }
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                if (!TimeCalculator.hardpaused)
                {
                    Lpaddle.alterVectorY(paddlemovespeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (Lpaddle.position.Y > (480 - Lpaddle.boundingrect.Height))
                        Lpaddle.position.Y = (480 - Lpaddle.boundingrect.Height);
                }
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                if (!TimeCalculator.hardpaused)
                {
                    Rpaddle.alterVectorY(-paddlemovespeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (Rpaddle.position.Y < 0 + lscore.boundingrect.Width)
                        Rpaddle.position.Y = 1 + lscore.boundingrect.Width;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                if (!TimeCalculator.hardpaused)
                {
                    Rpaddle.alterVectorY(paddlemovespeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (Rpaddle.position.Y > (480 - Lpaddle.boundingrect.Height))
                        Rpaddle.position.Y = (480 - Lpaddle.boundingrect.Height);
                }
            }
            if (keyState.IsKeyDown(Keys.Escape))
            {
                if (!TimeCalculator.hardpaused)
                {
                    hardpausedcalc.pausedtime = 0;
                    TimeCalculator.hardPause();
                }
            }
            if(keyState.IsKeyDown(Keys.F1))
            {
                if (TimeCalculator.hardpaused)
                {
                    lastpausedtime += hardpausedcalc.pausedtime;
                    TimeCalculator.hardResume();
                }
            }
        }

        // Time Calculation //
        private void updateTimes(GameTime gameTime)
        {

            if (ScoreBoard.gtime >= 0)
            {
                if (!TimeCalculator.hardpaused)
                {
                    ScoreBoard.gtime = (GAME_LENGTH + lastpausedtime) - getTime(gameTime);
                    hardpausedcalc.lasttime = getTime(gameTime);
                }
                else
                    hardpausedcalc.pausedtime = getTime(gameTime) - hardpausedcalc.lasttime;

                if (TimeCalculator.softpaused)
                {
                    if (getTime(gameTime) - softpausedcalc.lasttime >= 3)
                    {
                        TimeCalculator.softResume();
                    }
                }
                else if (!TimeCalculator.softpaused)
                {
                    softpausedcalc.lasttime = getTime(gameTime);
                }
                else { }

            }
            else
            {
                if (RScore.score > LScore.score)
                    RScore.incrementRGames();
                else if (LScore.score > RScore.score)
                    LScore.incrementLGames();
                else if (LScore.score == RScore.score)
                {
                    LScore.incrementLGames();
                    RScore.incrementRGames();
                }
                else { }

                resetValues(gameTime);
                TimeCalculator.softPause();  
            }
        }
        
        
        // Inits Scoreboard's Fonts And Positions //
        private void initScore(SpriteFont font, SpriteFont timefont)
        {
            lscore.setFont(font);
            rscore.setFont(font);
            scoreboard.setFont(timefont);

            lscore.setPosition(new Vector2(20, 10));
            rscore.setPosition(new Vector2(610, 10));
            scoreboard.setPosition(new Vector2(300,2));
        }


        // Animation Initiation Functions //
        private void animateBall()
        {
            if (ball.boundingrect.X < 32 * ball.boundingrect.Width)
                ball.animate();
            else
            {
                ball.boundingrect.X = 0;
                spriteBatch.Draw(ball.spritesheet, ball.position, ball.boundingrect, Color.White);
            }
        }

        private void animateCountDown(GameTime gameTime)
        {
            if (countdown.boundingrect.X < 3 * countdown.boundingrect.Width && countdowntimer.pausedtime + countdowntimer.lasttime <= getTime(gameTime))
            {
                countdowntimer.pausedtime++;
                countdown.animate();
            }
            else if(!TimeCalculator.softpaused)
            {
                countdown.boundingrect.X = -200;
                countdowntimer.pausedtime = 0;
                countdowntimer.lasttime = getTime(gameTime);
            }
        }

        private void animateCrystal()
        {
            for (int counter = 0; counter < 6; counter++)
            {
                if (counter < 3 && lscore.positionsocket[LScore.gameswon] >= counter)
                {
                    if (animatedcrystalarr[counter].boundingrect.X < 32 * animatedcrystalarr[counter].boundingrect.Width)
                        animatedcrystalarr[counter].animate();
                    else
                    {
                        animatedcrystalarr[counter].boundingrect.X = 0;
                        spriteBatch.Draw(animatedcrystalarr[counter].spritesheet, animatedcrystalarr[counter].position, animatedcrystalarr[counter].boundingrect, Color.White);
                    }
                }
                else if (counter >= 3 && Math.Abs(rscore.positionsocket[RScore.gameswon]) <= counter)
                {
                    if (animatedcrystalarr[counter].boundingrect.X < 32 * animatedcrystalarr[counter].boundingrect.Width)
                        animatedcrystalarr[counter].animate();
                    else
                    {
                        animatedcrystalarr[counter].boundingrect.X = 0;
                        spriteBatch.Draw(animatedcrystalarr[counter].spritesheet, animatedcrystalarr[counter].position, animatedcrystalarr[counter].boundingrect, Color.White);
                    }
                }
                else { }
            }
        }
        
        public void animateSprites(GameTime gameTime)
        {
            animateBall();
            animateCrystal();
            animateCountDown(gameTime);
        }


        // Reset Time And Score Values //
        private void resetValues(GameTime gameTime)
        {
            LScore.score = 0;
            RScore.score = 0;
            hardpausedcalc.pausedtime = 0;
            softpausedcalc.lasttime = 0;
            countdowntimer.lasttime = 0;
            lastpausedtime = 0;
            elapsed = (int)gameTime.TotalGameTime.TotalSeconds;
            ScoreBoard.gtime = 99;
            ball.position = new Vector2(320, 240);
        }

        // Get The Calculated Time //
        private int getTime(GameTime gameTime)
        {
            return ((int)gameTime.TotalGameTime.TotalSeconds - elapsed);
        }

    }
}
