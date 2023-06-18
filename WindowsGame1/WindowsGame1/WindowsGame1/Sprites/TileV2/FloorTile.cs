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
    class FloorTile : Tile2
    {

        public string textureDir;

        public FloorTile(char t, Rectangle r, string textureDir) : base(t, r, textureDir)
        {
        }

        public override void update()
        {
            
        }

    }
}
