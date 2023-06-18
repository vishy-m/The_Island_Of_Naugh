using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Key : Sprite
    {
        public Texture2D tex;
        public static Player player;
        //public Vector2 bounds;
        //public Vector2 startingBounds;

        public Key(Rectangle rectangle, Texture2D texture, Player p) : base(rectangle, "E")
        {
            tex = texture;
            player = p;
        }

        public override void update()
        {

        }

        public override void loadContent()
        {
            
        }

        public override void draw()
        {
            
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rect, Color.White);
        }

    }
}
