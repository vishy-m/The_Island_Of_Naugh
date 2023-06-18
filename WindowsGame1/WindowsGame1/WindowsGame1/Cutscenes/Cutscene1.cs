using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsGame1
{
    public class Cutscene1
    {
        SpriteFont font;
        Rectangle waterRect;
        Texture2D waterText;
        Rectangle boatRect;
        Texture2D boatText;
        Rectangle lightningRect;
        Texture2D lightningText;
        SoundEffect thunderClip;
        Rectangle chaxRect;
        Texture2D chaxText;
        Texture2D brokenBoatText;
        Boolean playSoundOnce = false;
        Boolean lightningHitBoat = false;
        Color c = Color.Black;
        Rectangle cloudRect;
        Rectangle playerRect;
        Texture2D playerText;
        Texture2D cloudText;
        SoundEffect lightningSound;
        Random r = new Random();
        int anotherTimer;
        int timer;
        int finalTimer;
        SoundEffect chaxSound;
        int speed;

        public Cutscene1(SpriteBatch spriteBatch)
        {
            waterRect = new Rectangle(0, 500, 1600, 300);
            boatRect = new Rectangle(0, 625, 100, 100);
            lightningRect = new Rectangle(50, 0, 600, 600);
            timer = 0;
            finalTimer = 0;
            chaxRect = new Rectangle(175, 150, 450, 400);
            cloudRect = new Rectangle(0, 0, 200, 100);
            playerRect = new Rectangle(0, 575, 100, 100);
            speed = 5;
        }

        public void loadContent()
        {
            font = GlobalVars.spriteFont;
            waterText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/water");
            boatText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/boat");
            lightningText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/lightning");
            chaxText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/chaxRegular");
            thunderClip = GlobalVars.Content.Load<SoundEffect>("BeginningCutSceneSprites/Sounds/thunder");
            cloudText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/clouds");
            lightningSound = GlobalVars.Content.Load<SoundEffect>("BeginningCutSceneSprites/Sounds/ThunderSFX");
            chaxSound = GlobalVars.Content.Load<SoundEffect>("BeginningCutSceneSprites/Sounds/Thriller");
            brokenBoatText = GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/brokenBoat");
            playerText = GlobalVars.Content.Load<Texture2D>("Sprites/Player/Normal/Player-front");
        }

        public void update(GameTime gameTime)
        {
            timer++;
            anotherTimer++;
            finalTimer++;

            playCutscene1();

            if (timer > 1380)
            {
                GlobalVars.gState = GlobalVars.GAMESTATE.DUNGEON;
                FloorManager.loadContent();
                FloorManager.update(gameTime);
                playerRect.X = 100;
                playerRect.Y = 100;
                timer = 0;
            }
            
        }

        public void draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            //spriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);
            if (anotherTimer > 570)
            {
                lightningSound.Play();
                anotherTimer = 0;
            }

            if (timer < 600)
            {
                spriteBatch.Draw(cloudText, cloudRect, Color.White);
                spriteBatch.Draw(waterText, waterRect, Color.White);
                spriteBatch.Draw(boatText, boatRect, Color.White);
                spriteBatch.Draw(playerText, playerRect, Color.White);

                if (!playSoundOnce)
                {
                    thunderClip.Play();

                    playSoundOnce = true;

                }
                if (timer == 560 && timer < 600)
                {
                    lightningRect.X = boatRect.X - 250;

                }
                else
                {
                    if (timer % 35 == 0)
                    {

                        lightningRect.X = cloudRect.X - 150;
                        spriteBatch.Draw(lightningText, lightningRect, Color.White);


                    }
                }




            }
            else if (timer < 900)
            {
                thunderClip.Dispose();
                spriteBatch.Draw(waterText, waterRect, Color.White);
                spriteBatch.Draw(boatText, boatRect, Color.White);
                spriteBatch.Draw(playerText, playerRect, Color.White);

                if (timer % 35 == 0)
                {

                    if (!lightningHitBoat)
                    {
                        lightningRect.X = boatRect.X - 250;
                        lightningHitBoat = true;

                    }
                    else
                    {
                        lightningRect.X = r.Next(0, 600);

                    }

                    spriteBatch.Draw(lightningText, lightningRect, Color.White);

                }
                boatText = brokenBoatText;
            }
            else if (timer > 900 && timer < 1200)
            {
                GraphicsDevice.Clear(c);
                c.R -= 10;
                c.G -= 10;
                c.B -= 10;
                if (finalTimer > 360)
                {
                    chaxSound.Play();
                    finalTimer = 0;

                }
                spriteBatch.Draw(chaxText, chaxRect, Color.AntiqueWhite);
            }
            else if (timer > 1200 && timer < 1380)
            {
                GraphicsDevice.Clear(Color.White);

            }
            else if (timer > 1380)
            {

                
            }
        }
        public void playCutscene1()
        {
            if (timer < 600)
            {
                if (timer % 25 == 0)
                {
                    boatRect.X += 2;
                }
                if (timer % 10 == 0)
                {
                    boatRect.X -= 3;
                }
                boatRect.X += 2;
                playerRect.X = boatRect.X;
                waterRect.X -= 5;
                if (boatRect.X >= 800)
                {
                    boatRect.X = -50;
                }
                playerRect.X = boatRect.X;
                if (waterRect.Right < 800)
                {
                    waterRect.X = 0;
                }
                cloudRect.X += 4;

                if (cloudRect.Right > 800)
                {
                    cloudRect.X = -200;
                }

            }
            else
            {
                waterRect.X -= 5;
                if (waterRect.Right < 800)
                {
                    waterRect.X = 0;
                }
                if (boatRect.Bottom < 850)
                {
                    boatRect.Y += 2;
                    playerRect.Y = boatRect.Y - 75;

                }
                else if (boatRect.Bottom >= 850)
                {
                    boatRect.X -= 2;
                    playerRect.X = boatRect.X;

                }
            }
        }

        public void disposeMusic()
        {
            if (!chaxSound.IsDisposed)
            {
                chaxSound.Dispose();
            }
        }

    }
}
        

