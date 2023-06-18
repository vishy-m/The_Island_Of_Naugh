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
    public class Particle
    {
        public Rectangle rect;
        public int ptimer = 0;
        public bool remove = false;
        public Color c = Color.White;
        public int initialc;
        public Vector2 w;

        public List<Texture2D> anim = new List<Texture2D>();
        public int animTimer = 0;
        public int sprit = 0;

        SpriteEffects flip = SpriteEffects.None;

        public Particle(Rectangle r)
        {
            rect = new Rectangle((r.X + (r.Width / 2)) - (r.Width / 4), (r.Y + (r.Height / 2)) - (r.Height / 4), r.Width / 2, r.Height / 2);
            w = new Vector2(rect.Width / 30, rect.Height / 30);
            initialc = c.A / 30;
            anim = Loader.loadMultipleTextures("dashAnim");
        }

        public Particle(Rectangle r, string p)
        {
            rect = r;
            w = new Vector2(rect.Width / 30, rect.Height / 30);
            initialc = c.A / 30;
            anim = Loader.loadMultipleTextures(p);
        }

        public void update()
        {
            ptimer++;
            rect.X -= (int)w.X;
            rect.Y -= (int)w.Y;
            rect.Width += (int)w.X * 2;
            rect.Height += (int)w.Y * 2;
            //Console.WriteLine("X: " + rect.X + " Y: " + rect.Y + " W: " + rect.Width + " H: " + rect.Height);
            if (ptimer > 30)
            {
                remove = true;
            }

            animTimer++;
            if (animTimer % 2 == 0 && sprit + 1 < anim.Count())
            {
                sprit++;
            }
            if (ptimer == 1)
            {
                if (GlobalVars.player.direction == 1)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }
        }

        public void draw()
        {
            Vector2 origin = new Vector2(anim[sprit].Width / 2, (anim[sprit].Height / 2) - 20);
            Rectangle r = anim[sprit].Bounds;
            Game1.spriteBatch.Draw(anim[sprit], rect, null, c, 0, origin, flip, 0f);
        }
    }
}
