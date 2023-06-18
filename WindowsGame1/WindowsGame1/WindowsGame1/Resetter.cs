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
    class Resetter
    {
        public static void reset()
        {
            GlobalVars.player.percentage = 1f;
            FloorManager.reset();
            EnemyManager.reset();
            TileManager.reset();
            Collectible.reset();
            Sprite.reset();
            HUD.reset();
            GlobalVars.isDungeonComplete = false;
            DungeonKey.reset();
            GlobalVars.gState = GlobalVars.GAMESTATE.DUNGEON;
            GlobalVars.retTimer++;
            Game1.reset();
        }
    }
}
