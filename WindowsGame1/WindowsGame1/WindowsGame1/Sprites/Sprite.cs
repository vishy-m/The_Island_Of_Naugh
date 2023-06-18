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
    public abstract class Sprite
    {
        public static List<Sprite> sprites = new List<Sprite>();

        public Rectangle rect;

        public bool isInteractible = false;
        public string interactionKey = "";
        public bool shouldRemove;

        public Sprite(Rectangle r)
        {
            rect = r;
            sprites.Add(this);
        }
        public Sprite(Rectangle r, string s)
        {
            rect = r;
            sprites.Add(this);
            isInteractible = true;
            interactionKey = s;
        }

        public abstract void loadContent();
        public abstract void update();

        public abstract void draw();

        public static void checkRemoveSprites()
        {
            for (int i = sprites.Count() - 1; i > -1; i--)
            {
                if (sprites[i].shouldRemove)
                {
                    sprites.RemoveAt(i);
                }
            }
        }

        public static void drawInteractibleKey()
        {
            for (int i = 0; i < sprites.Count(); i++)
            {
                Sprite s = sprites[i];
                if (s.isInteractible)
                {
                    if (GlobalVars.player.playerRect.Intersects(s.rect))
                    {
                        Vector2 dimensons = new Vector2(25, 25);
                        Rectangle position = new Rectangle(s.rect.X + (s.rect.Width / 2) - ((int)dimensons.X / 2), s.rect.Y - (int)dimensons.Y, (int)dimensons.X, (int)dimensons.Y);

                        Vector2 keyTextPos = new Vector2(position.X + (position.Width / 2) - 7, position.Y + 2);


                        GlobalVars.spriteBatch.Draw(GlobalVars.genericTestText, position, Color.White);
                        GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, s.interactionKey, keyTextPos, Color.White);
                    }
                }
            }
        }

        public static void reset()
        {
            sprites = new List<Sprite>();
        }



    }
}
