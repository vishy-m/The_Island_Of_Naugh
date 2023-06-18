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
    public abstract class Floor
    {
        public Dictionary<string, List<Texture2D>> textureMaps;

        //public static List<Tile> nextFloorTiles;

        public Dictionary<char, string> mapDirs;

        public KeyboardState oldKB;

        public static int dirCount = 0;

        public Floor()
        {
            textureMaps = new Dictionary<string, List<Texture2D>>();
            //nextFloorTiles = new List<Tile>();

            mapDirs = new Dictionary<char, string>();

            oldKB = Keyboard.GetState();
        }

        public abstract void loadContent();
        public abstract void update(KeyboardState kbr, GameTime gameTime);
        public abstract void draw();
        
        public void loadMultipleTextures(string textureKey, string[] fileNames)
        {
            if (!textureMaps.ContainsKey(textureKey))
            {
                Console.WriteLine("INVALID KEY");
                return;
            }
            for (int i = 0; i < fileNames.Length; i++)
            {
                Texture2D tex = GlobalVars.Content.Load<Texture2D>(fileNames[i]);
                textureMaps[textureKey].Add(tex);
            }
        }
        public void consolePrint(string[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                //Console.WriteLine(s[i]);
            }
        }

    }
}
