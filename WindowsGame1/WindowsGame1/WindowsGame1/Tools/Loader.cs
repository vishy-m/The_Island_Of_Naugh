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
    class Loader
    {
        public static string[] getFileNames(string dir)
        {
            string[] files = Directory.GetFiles(dir);

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = replaceFileName(files[i]);
                files[i] = fileName;
            }

            return files;
        }
        public static void consolePrint(string[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Console.WriteLine(s[i]);
            }
        }
        public static string replaceFileName(string fileName)
        {
            string lastFileName = "";
            int dotIndex = fileName.IndexOf(".");
            char[] fileNameChars = fileName.ToArray<char>();

            for (int j = 0; j < fileNameChars.Length; j++)
            {
                char c = fileNameChars[j];
                if (c == '.') break;
                if (c == 92) c = '/';

                lastFileName += c;
            }

            return lastFileName;
        }
        public static List<Texture2D> loadMultipleTextures(string[] fileNames)
        {
            List<Texture2D> textures = new List<Texture2D>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                Texture2D tex = GlobalVars.Content.Load<Texture2D>(fileNames[i]);
                textures.Add(tex);
            }

            return textures;
        }

        public static List<Texture2D> loadMultipleTextures(string dir)
        {
            string[] fileNames = getFileNames(dir);

            List<Texture2D> textures = new List<Texture2D>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                Texture2D tex = GlobalVars.Content.Load<Texture2D>(fileNames[i]);
                textures.Add(tex);
            }

            return textures;
        }
    }
}
