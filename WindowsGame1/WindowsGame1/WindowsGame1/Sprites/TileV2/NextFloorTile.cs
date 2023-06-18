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
    class NextFloorTile : Tile2
    {

        public int index;

        public static bool eventOccured = false;

        public string afterEventTextureDir;

        public NextFloorTile(char c, Rectangle r, string textureDir, string afterEventTexture) : base(c, r, textureDir)
        {
            index = FloorManager.currentFloorIndex;

            eventOccured = false;

            afterEventTextureDir = afterEventTexture;
        }

        public override void update()
        {
            if (eventOccured)
            {
                textureDir = afterEventTextureDir;
                //FloorManager.checkNextFloor();
                this.loadContent();
                
            }
            
        }

        public static void checkEventOccurance()
        {
            if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON)
            {
                if (EnemyManager.enemies.Count() == 0 && EnemyManager.initialspawn)
                {
                    eventOccured = true;
                    FloorManager.checkNextFloor();
                }
            }
            else if (GlobalVars.gState == GlobalVars.GAMESTATE.FOREST)
            {
                eventOccured = true;
                FloorManager.checkNextFloor();
            }
        }
    }
}
