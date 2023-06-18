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
    
    class Gate : ImpassableTile
    {
        public static bool hasBeenOpened;

        public string floorTextureDir;

        public Gate(char c, Rectangle r, string textureDir, string floorTextureDir) : base(c, r, textureDir)
        {
            this.floorTextureDir = floorTextureDir;

            hasBeenOpened = false;
        }
        public override void update()
        {
            base.update();
            //Console.WriteLine("Pos: (" + rect.X + ", " + rect.Y + ") " + floorTextureDir);
            if (DungeonKey.correctKeyCollected)
            {
                if (rect.Intersects(GlobalVars.player.playerRect))
                {
                    if (!hasBeenOpened)
                    {
                        HUD.drawText("Wow, I'm surpised you figured that out, however, # YOU STAND NO CHANCE AGAINST MY MINIONS # Here take this sword # Even with a sword, someone so puny such as you cannot possibly defeat my MINIONS", "BeginningCutSceneSprites/Sprites/chaxRegular");
                    }
                    hasBeenOpened = true;

                }
            }
            
        }

    }

    
}
