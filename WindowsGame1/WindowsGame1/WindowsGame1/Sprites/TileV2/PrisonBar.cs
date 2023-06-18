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
    public class PrisonBar : Tile2
    {
        public int prisonTimer;
        public double damageDealt = .2 / 60;

        public bool barIsElectric;

        public List<Texture2D> prisonSprites;

        public PrisonBar(char type, Rectangle r) : base(type, r, "Blue Husk")
        {
            prisonTimer = 0;
            prisonSprites = new List<Texture2D>();
        }

        public override void loadContent()
        {
            prisonSprites = Loader.loadMultipleTextures("DungeonSprites/PrisonBars");
        }

        public override void update()
        {
            prisonTimer++;

            if (sprit == 0)
            {
                if (prisonTimer > 60)
                {
                    sprit++;
                    prisonTimer = 0;
                }
            }
            if (prisonTimer > 15 && sprit != 0)
            {
                sprit++;

                if (sprit > 3)
                {
                    sprit = 0;
                    
                }
                prisonTimer = 0;

            }
            if (sprit != 0)
            {
                if (rect.Intersects(GlobalVars.player.playerRect))
                {
                    checkElectrify();
                }
            }
        }

        public void checkElectrify()
        {
            KeyboardState kbr = Keyboard.GetState();

            if (checkPressedKeys(GlobalVars.player.movingKeys, kbr))
            {
                GlobalVars.player.takeDamage(damageDealt);
            }
            
        }
        public bool checkPressedKeys(List<Keys> keys, KeyboardState kbr)
        {
            for (int i = 0; i < keys.Count(); i++)
            {
                if (kbr.GetPressedKeys().Contains(keys[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public override void draw()
        {
            GlobalVars.spriteBatch.Draw(prisonSprites[sprit], rect, Color.White);
        }
    }
}
