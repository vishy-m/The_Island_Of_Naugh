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
    public class DeathCutScene : Microsoft.Xna.Framework.Game
    {

        List<Rectangle> whiteRects;
        Texture2D whiteText;
        Rectangle playerRect;
        Texture2D playerText;

        Rectangle chaxRect;
        Texture2D chaxText;

        int timer;
        int anotherTimer;

        KeyboardState oldKB = Keyboard.GetState();

        Boolean left;

        Color c = Color.Black;
        Color chaxColor = new Color(0, 0, 0);

        Random r = new Random();

        Rectangle rect = new Rectangle(0, 0, 800, 800);//GAME OVER
        Texture2D text;

        SoundEffect thriller;

        SoundEffect laugh;

        public DeathCutScene(ContentManager c)
        {
            Content.RootDirectory = "Content";

            playerRect = new Rectangle(250, 250, 400, 400);
            chaxRect = new Rectangle(175, 150, 450, 400);
            timer = 0;
            timer++;
            anotherTimer = 0;
            whiteRects = new List<Rectangle>(100);
            //Console.WriteLine(whiteRects.Count);
            for (int i = 0; i < 25; i++)
            {
                Rectangle whiteRect = new Rectangle(playerRect.X + i * 5, playerRect.Y - 10, 5, 5);
                whiteRects.Add(whiteRect);
                //Console.WriteLine("x: " + whiteRects[i].X);
            }

            loadContent(c);
        }

        public void loadContent(ContentManager Content)
        {
            playerText = Content.Load<Texture2D>("DeathCutScene/player");
            chaxText = Content.Load<Texture2D>("DeathCutScene/chax");
            laugh = Content.Load<SoundEffect>("DeathCutScene/laugh");
            whiteText = Content.Load<Texture2D>("DeathCutScene/white");
            text = Content.Load<Texture2D>("DeathCutScene/gameOver");
            thriller = Content.Load<SoundEffect>("DeathCutScene/Thriller");
        }

        public void play(GraphicsDevice g)
        {
            timer++;
            anotherTimer++;
            playCutscene(g);
        }

        public void remove5()
        {
            if (whiteRects.Count > 0)
            {
                whiteRects.RemoveAt(whiteRects.Count - 1);

            }
        }

        public void playCutscene(GraphicsDevice GraphicsDevice)
        {
            string str = "PRESS E TO RESTART FROM BEGINNING";
            Vector2 textPos = new Vector2(400 - GlobalVars.spriteFont.MeasureString(str).X / 2, 30);
            GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, str, textPos, Color.White);
            if (timer < 285)
            {

                if (anotherTimer > 240)
                {
                    laugh.Play();
                    anotherTimer = 0;
                }
                if (left)
                {
                    chaxRect.X -= 5;
                    if (chaxRect.X <= 0)
                    {
                        left = false;
                    }
                }
                else
                {
                    chaxRect.X += 5;
                    if (chaxRect.X + chaxRect.Width > 800)
                    {
                        left = true;
                    }
                }

                GraphicsDevice.Clear(c);


                c.R -= 4;
                c.G -= 4;
                c.B -= 4;
                if (chaxColor.R < 255)
                {
                    chaxColor.R += 1;

                }
                if (anotherTimer > 120)
                {
                    laugh.Play();
                    anotherTimer = 0;
                }
                GlobalVars.spriteBatch.Draw(chaxText, chaxRect, chaxColor);
            }
            else if (timer < 600)
            {
                if (timer % 600 > 0)
                {
                    thriller.Play();
                }

                if (playerRect.Width > 0)
                {
                    GlobalVars.spriteBatch.Draw(playerText, playerRect, c);
                    playerRect.Width = playerRect.Width - 1;
                }


                if (playerRect.Height > 0)
                {
                    playerRect.Height = playerRect.Height - 1;

                }
            }
            else
            {
                thriller.Dispose();
                GraphicsDevice.Clear(Color.Blue);
                GlobalVars.spriteBatch.Draw(text, rect, Color.White);
                
            }
        }
    }
}
