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
    public static class TileManager
    {
        public static List<Tile2> allTiles = new List<Tile2>();

        public static void tileLoad(String directory)
        {
            using (StreamReader fileReader = new StreamReader(directory))
            {
                int l = 0;
                int w = 0;
                for (int i = 0; !fileReader.EndOfStream; i++)
                {
                    String line = fileReader.ReadLine();
                    char[] temp = line.ToCharArray();
                    int y = i * 100;
                    for (int j = 0; j < temp.Length; j++)
                    {
                        int x = j * 100;
                        allTiles.Add(makeVarietyTile(temp[j], new Rectangle(x, y, 100, 100)));                        
                    }
                }
            }
        }

        public static Tile2 makeVarietyTile(char c, Rectangle rect)
        {
            Tile2 t = null;

            Dictionary<char, string> dirs = FloorManager.floors[FloorManager.currentFloorIndex].mapDirs;

            //Console.WriteLine(FloorManager.currentFloor.mapDirs.ToString() + " " + GlobalVars.gState.ToString() + " " + FloorManager.currentFloorIndex);

            string currentTileDir = "";

            if (!dirs.ContainsKey(c))
            {
                currentTileDir = "Blue Husk";
            }
            else
            {
                currentTileDir = dirs[c];
            }
            

            switch (c)
            {
                case 'p':
                    t = new PrisonBar(c, rect);
                    break;
                case '_':
                case 'a':
                    t = new FloorTile(c, rect, currentTileDir);
                    break;
                case 'r':
                case 'd':
                case 'u':
                case 'l':
                case 'I':
                    currentTileDir = dirs['I'];
                    t = new ImpassableTile(c, rect, currentTileDir);
                    break;
                case 'b':
                    DungeonKey.makeBounds(rect);
                    t = new InvisibleTile(c, rect);
                    break;
                case 'e':
                case 'E':
                    EnemyManager.makeEnemyBounds(rect, c, allTiles.Count());
                    t = new InvisibleTile(c, rect);
                    break;
                case 's':
                    GlobalVars.player.setSword(rect);
                    t = new InvisibleTile(c, rect);
                    break;
                case '.':
                    t = new InvisibleTile(c, rect);
                    break;
                case 'g':
                case 'G':
                    t = new Gate(c, rect, currentTileDir, dirs['a']);
                    break;
                case 'n':
                    t = new NextFloorTile(c, rect, dirs['N'], currentTileDir);
                    break;
                case 't':
                    t = new InfoTile(c, rect, currentTileDir);
                    break;
                case 'x':
                    GlobalVars.player.playerRect.X = rect.X;
                    GlobalVars.player.playerRect.Y = rect.Y;
                    t = new FloorTile(c, rect, currentTileDir);
                    break;
                default:
                    t = new FloorTile(c, rect, "Blue Husk");
                    break;
            }

            return t;
        }

        public static void updateAllTiles()
        {
            NextFloorTile.checkEventOccurance();
            if (!checkIntersectWithPrisonBar())
            {
                DungeonKey.timer++;
            }
            for (int i = allTiles.Count() - 1; i > -1; i--)
            {
                if (allTiles[i] != null)
                {
                    allTiles[i].update();
                    if (allTiles[i].shouldRemove)
                    {
                        allTiles.RemoveAt(i);
                    }
                }
                if (Gate.hasBeenOpened)
                {
                    if (allTiles[i] is Gate)
                    {
                        Gate g = (Gate)allTiles[i];
                        allTiles[i] = new FloorTile('a', g.rect, g.floorTextureDir);
                        allTiles[i].loadContent();
                    }
                }
            }
        }

        public static void loadAllTiles()
        {
            for (int i = 0; i < allTiles.Count(); i++)
            {
                if (allTiles[i] != null)
                {
                    allTiles[i].loadContent();
                }
            }
        }

        public static void drawAllTiles()
        {
            for (int i = 0; i < allTiles.Count(); i++)
            {
                if (allTiles[i] != null)
                {
                    allTiles[i].draw();
                }
            }
        }

        public static void refreshTiles()
        {
            allTiles = new List<Tile2>();
            EnemyManager.initialSpawnRects = new List<Rectangle>();
            EnemyManager.endingSpawnRects = new List<Rectangle>();
            EnemyManager.enemyBoundList = new List<Rectangle>();
        }

        public static bool checkIntersectWithPrisonBar()
        {
            for (int i = 0; i < allTiles.Count(); i++)
            {
                if (allTiles[i] is PrisonBar)
                {
                    if (GlobalVars.player.playerRect.Intersects(allTiles[i].rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void reset()
        {
            allTiles = new List<Tile2>();
            NextFloorTile.eventOccured = false;
        }
    }
}
