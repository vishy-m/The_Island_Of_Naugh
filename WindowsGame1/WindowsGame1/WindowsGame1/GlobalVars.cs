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
    public static class GlobalVars
    {
        public static Player player;
        public static bool isDungeonComplete = false;
        public static SpriteBatch spriteBatch;
        public static Texture2D genericTestText;
        public static SpriteFont spriteFont;
        public static ContentManager Content;
        public static Camera camera;

        public static Floor currentFloor = new Floor1();

        public static int retTimer = 0;
        public enum GAMESTATE
        {
            CUTSCENE1, DUNGEON, FOREST, BOSS, DEATHCUTSCENE, WINGAMESCENE

        }
        public static GAMESTATE gState;

        public static void loadVars(SpriteBatch s, Texture2D t, SpriteFont sf, ContentManager Content)
        {
            spriteBatch = s;
            genericTestText = t;
            spriteFont = sf;
            GlobalVars.Content = Content;
            player = new Player(1);
            gState = GAMESTATE.CUTSCENE1;
            camera = new Camera();
        }
    }
}
