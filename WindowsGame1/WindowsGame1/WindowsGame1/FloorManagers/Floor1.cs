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
    class Floor1 : Floor
    {
        public static string dungeonInfoText = "The name of the game is simple. You must find and collect the right key among the keys before you. #However, be careful with your choice, as it may result in a negative outcome";

        public List<Texture2D> playerSprites;

        public SpriteEffects flip;

        public List<Texture2D> playerIdleAnimation;
        public List<Texture2D> swordOpeningAnimFloor1;

        public static int timer;

        public Texture2D enTex;


        public Floor1() : base()
        {
            playerIdleAnimation = new List<Texture2D>();
            swordOpeningAnimFloor1 = new List<Texture2D>();
            playerSprites = new List<Texture2D>();

            flip = SpriteEffects.None;

            oldKB = Keyboard.GetState();

            textureMaps.Add("playerIdle", playerIdleAnimation);
            textureMaps.Add("swordOP", swordOpeningAnimFloor1);
            textureMaps.Add("player", playerSprites);

            mapDirs = new Dictionary<char, string>();
            mapDirs.Add('_', "Sprites/Floor1/Tiles/FloorTile");
            mapDirs.Add('a', "Sprites/Floor1/Tiles/grass");
            mapDirs.Add('I', "Sprites/Floor1/Tiles/Column");
            mapDirs.Add('x', "Sprites/Floor1/Tiles/FloorTile");
            mapDirs.Add('n', "DoorOpened");
            mapDirs.Add('N', "DoorClosed");

            timer = 0;
        }

        public override void loadContent()
        {
            if (dirCount == 0)
            {
                Directory.SetCurrentDirectory(GlobalVars.Content.RootDirectory);
                dirCount++;
            }
            
            TileManager.tileLoad("Text Files/Floor1Tiles.txt");
            TileManager.tileLoad("Text Files/Floor1OverTiles.txt");

            TileManager.loadAllTiles();
            //EnemyManager.spawnEnemy();
            GlobalVars.player.load();
            
            playerIdleAnimation = Loader.loadMultipleTextures("Sprites/Player/IdleAnim");
            playerSprites = Loader.loadMultipleTextures("Sprites/Player/Normal");
            swordOpeningAnimFloor1 = Loader.loadMultipleTextures("Sprites/Floor1/sword");

            enTex = GlobalVars.Content.Load<Texture2D>("redEnemy");

            GlobalVars.player.playerRect.X -= 3;
        }

        public override void update(KeyboardState kbr, GameTime gameTime)
        {
            if (timer == 0)
            {
                HUD.drawText("Im surprised you got through my minions # Why dont we try a more puzzling approach # Lets see if you have the brain to match your brawn", "BeginningCutSceneSprites/Sprites/chaxRegular");
                timer++;
            }
            EnemyManager.updateAllEnemies();
            TileManager.updateAllTiles();

            GlobalVars.player.update(TileManager.allTiles, gameTime);

            if (GlobalVars.player.flip)
                flip = SpriteEffects.FlipHorizontally;
            else
                flip = SpriteEffects.None;

            GlobalVars.camera.follow(GlobalVars.player.playerRect);

            Sprite.checkRemoveSprites();
        }
        public override void draw()
        {
            
            if (TileManager.allTiles != null)
                TileManager.drawAllTiles();
            if (playerSprites != null)
                drawPlayer();
            if (swordOpeningAnimFloor1 != null)
                drawSword();
            Sprite.drawInteractibleKey();
            EnemyManager.drawEnemies(enTex);
            GlobalVars.player.drawHealthBar();
            HUD.drawHUD();
        }
        public void drawPlayer()
        {
            GlobalVars.player.draw();
        }
        public void drawSword()
        {
            if (GlobalVars.player.sword != null)
            {
                Sword s = GlobalVars.player.sword;
                Vector2 swordCenter = new Vector2(s.rect.Width / 2, s.rect.Height / 2);
                if (!s.hasBeenCollected)
                    GlobalVars.spriteBatch.Draw(swordOpeningAnimFloor1[s.sprit], s.rect, Color.White);
                else
                    GlobalVars.spriteBatch.Draw(swordOpeningAnimFloor1[s.sprit], s.rect, null, Color.White, 0, swordCenter, flip, 0);
            }
        }

        //public void drawTimer()
        //{
        //    double startingNum = Math.Floor(dungeonTimer / 60.0);
        //    double endingNum = ((dungeonTimer / 100) - startingNum) * 100;
        //    string clock;
        //    if (dungeonTimer % 60 > 9)
        //    {
        //        clock = (dungeonTimer / 60).ToString() + ":" + (dungeonTimer % 60).ToString();
        //    }
        //    //displays the current time remaining
        //    else
        //    {
        //        clock = (dungeonTimer / 60).ToString() + ":0" + (dungeonTimer % 60).ToString();
        //    }

        //    Rectangle clockRect = new Rectangle(0, 0, 95, 50);

        //    GlobalVars.spriteBatch.Draw(HUD.textBackgroundText, clockRect, Color.White);
        //    GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, clock, new Vector2(25, 14), Color.White);
        //}

    }
}
