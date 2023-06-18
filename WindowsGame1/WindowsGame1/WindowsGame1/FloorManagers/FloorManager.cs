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
    static class FloorManager
    {

        public static Floor[] floors = new Floor[3]
        {
            new Dungeon(), new Floor1(), new BossRoom()
        };

        public static int currentFloorIndex = 0;

        public static Floor currentFloor = floors[currentFloorIndex];
        
        public static void loadContent()
        {
            currentFloorIndex = (int)GlobalVars.gState - 1;
            if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON)
                currentFloorIndex = 0;
            if (GlobalVars.gState > GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                floors[currentFloorIndex].loadContent();
            }
        }

        public static void update(GameTime gameTime)
        {
            
            //currentFloor = floors[currentFloorIndex];
            if (GlobalVars.gState > GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                currentFloorIndex = (int)GlobalVars.gState - 1;
                KeyboardState kbr = Keyboard.GetState();

                Console.WriteLine(GlobalVars.gState.ToString() + " " + currentFloorIndex);

                floors[currentFloorIndex].update(kbr, gameTime);
            }
            if (GlobalVars.gState > 0 && GlobalVars.gState != GlobalVars.GAMESTATE.DUNGEON && floors[0] is Dungeon)
            {
                Dungeon.stopBackgroundMusic();
            }
        }
        public static void draw()
        {
            if (GlobalVars.gState > GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                floors[currentFloorIndex].draw();
            }
        }

        public static void checkNextFloor()
        { 
            if ((int)GlobalVars.gState > 0 && (int)GlobalVars.gState < (int)GlobalVars.GAMESTATE.DEATHCUTSCENE - 1)
            {
                if (checkcolliding())
                {
                    GlobalVars.gState++;
                    currentFloorIndex++;
                    TileManager.refreshTiles();
                    floors[currentFloorIndex].loadContent();
                    EnemyManager.initialspawn = false;
                    DungeonKey.correctKeyCollected = false;
                    NextFloorTile.eventOccured = false;
                }
            }
        }

        public static bool checkcolliding()
        {
            for (int i = 0; i < TileManager.allTiles.Count(); i++)
            {
                if (TileManager.allTiles[i] is NextFloorTile)
                {
                    if (GlobalVars.player.playerRect.Intersects(TileManager.allTiles[i].rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void reeGame()
        {
            GlobalVars.gState = GlobalVars.GAMESTATE.DUNGEON;
            currentFloorIndex = 0;
            TileManager.refreshTiles();
            floors[currentFloorIndex].loadContent();
            EnemyManager.initialspawn = false;
            DungeonKey.correctKeyCollected = false;
            NextFloorTile.eventOccured = false;
            Game1.gameTimer = 60 * 5;
            Game1.timer = 0;
            GlobalVars.player.percentage = 1f;
        }

        public static void reset()
        {
            floors = new Floor[3]
            {
                new Dungeon(), new Floor1(), new BossRoom()
            };

            currentFloorIndex = 0;

            currentFloor = floors[currentFloorIndex];
        }
    }
}