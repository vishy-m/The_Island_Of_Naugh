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
    class ImpassableTile : Tile2
    {
        public string textureDir;

        public ImpassableTile(char c, Rectangle r, string textureDir) : base(c, r, textureDir)
        {
        }

        public override void update()
        {
            int playerRectXCenter = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width / 2);
            int playerRectYCenter = GlobalVars.player.playerRect.Y + (GlobalVars.player.playerRect.Height / 2);

            if (playerRectXCenter < rect.X && playerRectYCenter > rect.Y && playerRectYCenter < rect.Y + rect.Height - 10)
            {
                type = 'r';
            }
            else if (playerRectXCenter > rect.X + rect.Width && playerRectYCenter > rect.Y && playerRectYCenter < rect.Y + rect.Height - 10)
            {
                type = 'l';
            }
            else if (playerRectYCenter > rect.Y + rect.Height && playerRectXCenter > rect.X && playerRectXCenter < rect.X + rect.Width - 10)
            {
                type = 'u';
            }
            else if (playerRectYCenter < rect.Y && playerRectXCenter > rect.X && playerRectXCenter < rect.X + rect.Width - 10)
            {
                type = 'd';
            }
        }
    }
}
