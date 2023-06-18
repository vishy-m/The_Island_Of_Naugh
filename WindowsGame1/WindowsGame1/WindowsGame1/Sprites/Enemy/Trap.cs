using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WindowsGame1
{
    class Trap
    {
        public static bool isPlayerTrapped = false;

        public List<Texture2D> animation;

        public int sprit;
        public int animTimer;
        public int frameDelay;
        public int lifeTime;

        public int trapTimer;
        public int initialTrapTimer;

        public bool shouldRemove;

        public Texture2D currentText;

        public Rectangle rect;

        public bool retractAnim;

        const float damage = .01f;

        public Trap(Rectangle r, string dir, int ft, int tt)
        {
            rect = r;

            //Loading animations
            if (Directory.Exists(dir))
            {
                animation = Loader.loadMultipleTextures(dir);
                currentText = animation[0];
            }

            sprit = 0;
            lifeTime = ft;
            frameDelay = lifeTime / animation.Count();
            shouldRemove = false;
            trapTimer = tt;
            initialTrapTimer = tt;
            animTimer = 0;
            retractAnim = false;

        }

        public void update()
        {
            MouseState mouse = Mouse.GetState();

            lifeTime--;
            if (lifeTime < 0)
            {
                retractAnim = true;
            }

            if (rect.Intersects(new Rectangle(mouse.X, mouse.Y, 100, 100)))
            {
                isPlayerTrapped = true;
            }

            //if (isPlayerTrapped)
            //{
            //    Console.WriteLine("Player Trapped");
            //    trapTimer--;
            //    //make player not move
            //    if (trapTimer < 0)
            //    {
            //        trapTimer = initialTrapTimer;
            //        isPlayerTrapped = false;
            //    }
            //}

            if (!retractAnim)
            {
                animTimer++;
                if (animTimer % frameDelay == 0)
                {
                    if (sprit < animation.Count() - 1)
                    {
                        sprit++;
                    }
                }
            }
            if (retractAnim)
            {
                animTimer--;
                if (animTimer % frameDelay == 0)
                {
                    if (sprit > 0)
                    {
                        sprit--;
                    }

                }
                if (sprit == 0)
                {
                    shouldRemove = true;
                }
            }

            if (rect.Intersects(GlobalVars.player.playerRect))
            {
                GlobalVars.player.takeDamage(damage);
            }

        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(animation[sprit], rect, Color.White);
        }
    }
}

