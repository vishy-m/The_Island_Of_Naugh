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
    public class DungeonKey : Key
    {
        public static List<DungeonKey> fakeKeys = new List<DungeonKey>();
        static Random rnd = new Random();
        static Boolean addKey = false;
        public static Rectangle startingBoundRect;
        public static Rectangle endingBoundRect;
        public static int boundTimer;
        public static Rectangle boundRect;
        static int keyTimer = 0;
        static int maxNumKeys = 10;
        static int minNumKeys = 3;
        public static bool correctKeyCollected = false;
        public static bool addCorrectKey = true;

        public bool isCorrect;
        public int xVel, yVel;
        public int lifeTimer;
        public Boolean startLifeTimer;
        public int idleCollectedKeyTimer;
        Color keyColor;
        public static int timer = 0;
        public static GamePadState oldPad = GamePad.GetState(PlayerIndex.One);


        public DungeonKey(Rectangle rectangle, Texture2D texture, bool status, int xVelocity, int yVelocity, Player player)
            : base(rectangle, texture, player)
        {
            isCorrect = status;
            xVel = xVelocity;
            yVel = yVelocity;
            lifeTimer = rnd.Next(300, 600);
            shouldRemove = false;
            startLifeTimer = false;
            idleCollectedKeyTimer = 0;
            keyColor = Color.Red;
        }
        public override void update() // updates the Dungeon Key, takes control of the bouncing off of the bounds
        {
            if (isCorrect)
            {
                if (timer > 60)
                {
                    keyColor = Color.White;
                }
            }
            if (!correctKeyCollected)
            {
                rect.X += xVel;
                rect.Y += yVel;
                if (rect.X + rect.Width >= boundRect.X + boundRect.Width || rect.X <= boundRect.X)
                {
                    xVel *= -1;
                }
                if (rect.Y + rect.Height >= boundRect.Y + boundRect.Height || rect.Y <= boundRect.Y)
                {
                    yVel *= -1;
                }
            }
            else
            {
                isInteractible = false;
                if (isCorrect)
                {
                    if (idleCollectedKeyTimer == 0)
                    {
                        rect.X = player.playerRect.X - 25;
                        rect.Y = player.playerRect.Y - 25;
                        idleCollectedKeyTimer++;
                    }
                    else
                    {
                        trackPlayer();
                        rect.X += xVel;
                        rect.Y += yVel;
                    }
                }
                else
                {
                    shouldRemove = true;
                }
                if (Gate.hasBeenOpened)
                {
                    shouldRemove = true;
                }
            }
            checkRemoval();
        }
        public void checkRemoval() // checks if a dungeon key needs to be removed from the list
        {
            if (!isCorrect)
            {
                if (DungeonKey.getNumKeys() == DungeonKey.getMaxNumKeys())
                {
                    startLifeTimer = true;
                }

                if (startLifeTimer)
                {
                    lifeTimer--;
                    if (lifeTimer <= 0)
                    {
                        shouldRemove = true;
                    }
                }

                if (fakeKeys.Count() <= minNumKeys)
                {
                    startLifeTimer = false;
                    lifeTimer = rnd.Next(300, 600);
                }
            }

        }
        public static void checkAddKeys(Player player, Texture2D tex, KeyboardState kb, KeyboardState oldKB) //Checks if a key should be added
        {
            int boundsX = boundRect.X;
            int boundsY = boundRect.Y;
            int boundsXEnd = boundsX + boundRect.Width;
            int boundsYEnd = boundsY + boundRect.Height;
            Rectangle playerRect = player.playerRect;

            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            int xvel;
            int yvel;
            do
            {
                xvel = rnd.Next(-5, 5);
                yvel = rnd.Next(-5, 5);
            }
            while (xvel == 0 || yvel == 0);

            if (addCorrectKey)
            {
                addDungeonKey(player, tex, true, 0, 0);
                addCorrectKey = false;
            }

            if (fakeKeys.Count() < minNumKeys)
            {
                addDungeonKey(player, tex, false, 0, 0);
            }


            for (int i = 0; i < fakeKeys.Count(); i++)
            {
                Rectangle keyR = fakeKeys[i].rect;
                if (playerRect.Intersects(keyR) && addKey)
                {
                    if (kb.IsKeyDown(InputMaps.keyboardInputs[InputMaps.INTERACT]) && !oldKB.IsKeyDown(InputMaps.keyboardInputs[InputMaps.INTERACT]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.INTERACT]) && !oldPad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.INTERACT]))
                    {
                        if (!fakeKeys[i].isCorrect)
                        {
                            if (fakeKeys.Count() < maxNumKeys)
                            {
                                addDungeonKey(player, tex, false, 1, i);
                                addKey = false;
                            }
                        }
                        else
                        {
                            correctKeyCollected = true;

                        }
                    }
                }
            }

            Console.WriteLine("key collected: " + correctKeyCollected.ToString());

            if (!addKey)
            {
                keyTimer++;
                if (keyTimer > 30)
                {
                    keyTimer = 0;
                    addKey = true;
                }
            }
            oldPad = pad;
        }
        public static void addDungeonKey(Player player, Texture2D tex, bool status, int spawnType, int index) // type is for where to spawn the key,either random or on another key
        {
            int boundsX = boundRect.X;
            int boundsY = boundRect.Y;
            int boundsXEnd = boundsX + boundRect.Width;
            int boundsYEnd = boundsY + boundRect.Height;
            Rectangle playerRect = player.playerRect;
            //Console.WriteLine(boundsX + " = X, " + boundsY + " = Y, " + boundsXEnd + " = boundsXEnd, " + boundsYEnd + " = boundYEnd.");
            //Console.WriteLine(boundRect.X + " = X, " + boundRect.Y + " = Y, " + boundRect.Width + " = boundsWidth, " + boundRect.Height + " = boundsHeight.");
            int xvel;
            int yvel;
            do
            {
                xvel = rnd.Next(-5, 5);
                yvel = rnd.Next(-5, 5);
            }
            while (xvel == 0 || yvel == 0);

            if (spawnType == 0)
            {
                Rectangle keyRect = new Rectangle(rnd.Next(boundsX + 100, boundsXEnd - 100), rnd.Next(boundsY + 100, boundsYEnd - 100), 50, 50);
                fakeKeys.Add(new DungeonKey(keyRect, tex, status, xvel, yvel, player));
            }
            if (spawnType == 1)
            {
                Rectangle spawnLoc = new Rectangle(fakeKeys[index].rect.X, fakeKeys[index].rect.Y, 50, 50);
                fakeKeys.Add(new DungeonKey(spawnLoc, tex, false, xvel, yvel, player));
            }
        }
        public static void updateKeys() // updates all keys in the list
        {
            for (int i = 0; i < fakeKeys.Count(); i++)
            {
                fakeKeys[i].update();
                if (fakeKeys[i].shouldRemove)
                {
                    fakeKeys.RemoveAt(i);
                }
            }
            //Console.WriteLine("Num Keys: " + fakeKeys.Count());
        }
        public static void makeBounds(Rectangle rect) // makes the bounds for the keys to bounce in
        {
            if (boundTimer == 0)
            {
                startingBoundRect = rect;
            }
            else if (boundTimer == 1)
            {
                endingBoundRect = rect;
                int x = startingBoundRect.X;
                int y = startingBoundRect.Y;
                int w = endingBoundRect.X + endingBoundRect.Width - startingBoundRect.X;
                int h = endingBoundRect.Y + endingBoundRect.Height - startingBoundRect.Y;
                boundRect = new Rectangle(x, y, w, h);
            }
            else
            {
                boundTimer = 0;
                makeBounds(rect);
            }
            boundTimer++;
        }
        public static int getNumKeys()// gets the number of keys in the list
        {
            return fakeKeys.Count();
        }
        public static int getMaxNumKeys()// gets the max numbers of keys that can exist in the list
        {
            return maxNumKeys;
        }
        public void trackPlayer()// allows the correct key to track the player
        {
            if (!rect.Intersects(player.playerRect))
            {
                double dx = player.playerRect.X - rect.X;
                double dy = player.playerRect.Y - rect.Y;
                double hyp = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                double speed = Math.Min(hyp, 5);
                xVel = (int)(dx / (hyp / (speed * 2)));
                yVel = (int)(dy / (hyp / (speed * 2)));
            }

        }
        public static bool doesKeyIntersectPlayerOrRect(Rectangle r)// checks if a key intersects the rect passed in
        {
            for (int i = 0; i < fakeKeys.Count(); i++)
            {
                if (fakeKeys[i].rect.Intersects(player.playerRect) || fakeKeys[i].rect.Intersects(r))
                {
                    return true;
                }
            }
            return false;
        }
        public static void drawKeys(SpriteBatch spriteBatch) // draws all of the keys to the screen
        {
            //spriteBatch.Draw(fakeKeys[0].tex, boundRect, Color.White);
            for (int i = 0; i < fakeKeys.Count(); i++)
            {
                if (fakeKeys[i].isCorrect)
                    spriteBatch.Draw(fakeKeys[i].tex, fakeKeys[i].rect, fakeKeys[i].keyColor);
                else
                {
                    spriteBatch.Draw(fakeKeys[i].tex, fakeKeys[i].rect, Color.White);
                }
            }
        }

        public static void reset()
        {
            fakeKeys = new List<DungeonKey>();
            addCorrectKey = true;
            correctKeyCollected = false;
            boundTimer = 0;
            timer = 0;
            keyTimer = 0;
            maxNumKeys = 10;
            minNumKeys = 3;
            addKey = false;

        }
    }
}
