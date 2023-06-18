using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public abstract class Tile2 : Sprite
    {
        public char type;

        public int sprit;

        public string textureDir;

        public Texture2D tileTexture = GlobalVars.Content.Load<Texture2D>("Blue Husk");

        public Color color;

        public Tile2(char c, Rectangle r, string td) : base(r)
        {
            type = c;
            textureDir = td;

            color = Color.White;
        }

        public override void loadContent()
        {
            try
            {
                tileTexture = GlobalVars.Content.Load<Texture2D>(textureDir);
            }
            catch (Exception e)
            {
                tileTexture = GlobalVars.Content.Load<Texture2D>("Blue Husk");
            }
        }

        public override void draw()
        {
            
            GlobalVars.spriteBatch.Draw(tileTexture, rect, color);
        }
    }


}
