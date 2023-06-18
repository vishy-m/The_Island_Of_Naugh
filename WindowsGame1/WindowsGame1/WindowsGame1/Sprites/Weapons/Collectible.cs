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
    public abstract class Collectible : Sprite
    {
        //public static Player player;
        public static List<Collectible> collectibles = new List<Collectible>();

        public bool hasBeenCollected;
        public int sprit;
        public Collectible(Rectangle r) : base(r,"E")
        {
            //player = pl;
            hasBeenCollected = false;
            sprit = 0;
            collectibles.Add(this);
        }

        public void checkCollected()
        {
            if (!hasBeenCollected)
            {
                KeyboardState kbr = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (GlobalVars.player.playerRect.Intersects(rect))
                {
                    if (kbr.IsKeyDown(Keys.E) || pad.IsButtonDown(Buttons.X))
                    {
                        rect.X = GlobalVars.player.playerRect.X + 20;
                        rect.Y = GlobalVars.player.playerRect.Y + 20;
                        hasBeenCollected = true;
                        Tile.removeGates = false;
                    }
                }
            }
        }

        //public override void update()
        //{

        //}

        //public override void loadContent()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void draw()
        //{
        //    throw new NotImplementedException();
        //}
        public static void updateCollectibles()
        {


            for (int i = 0; i < collectibles.Count(); i++)
            {
                //Console.WriteLine("Rotation: " + collectibles[i].rotation);
                if (!collectibles[i].hasBeenCollected)
                {
                    collectibles[i].checkCollected();
                }
                else
                {
                    collectibles[i].update();
                }
            }
        }

        public static void reset()
        {
            collectibles = new List<Collectible>();
        }
    }
}
