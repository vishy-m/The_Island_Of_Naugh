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
    class InvisibleTile : Tile2
    {

        public InvisibleTile(char c, Rectangle r) : base(c, r, "Blue Husk")
        {
            color = Color.Transparent;
        }
        public override void update()
        {
            
        }
    }
}
