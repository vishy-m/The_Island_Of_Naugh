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
    public abstract class Weapon : Collectible
    {
        public float damage;
        public Weapon(Rectangle r) : base(r)
        {
            damage = 0f;
        }

        public void checkRotation()
        {
            if (hasBeenCollected)
            {
                rect.Width = 75;
                rect.Height = 75;
                //Console.WriteLine(GlobalVars.player.sprit + " ");
                if (GlobalVars.player.flip && !GlobalVars.player.isFacingBack)
                {
                    rect.X = GlobalVars.player.playerRect.X - 30;
                }
                else if (GlobalVars.player.isFacingBack && !GlobalVars.player.flip)
                {
                    rect.X = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width / 2);
                }
                else if (GlobalVars.player.isFacingBack && GlobalVars.player.flip)
                {
                    rect.X = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width / 2) - 35;
                }
                else
                {
                    rect.X = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width) - 15;
                }


                rect.Y = GlobalVars.player.playerRect.Y + 20;
                if (GlobalVars.player.isFacingBack)
                {
                    rect.Y = GlobalVars.player.playerRect.Y + 30;
                }
                
            }

        }

        public override void update()
        {

        }

    }
}
