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
    class BossRoom : Floor
    {
        public static string dungeonInfoText = "The name of the game is simple. You must find and collect the right key among the keys before you. #However, be careful with your choice, as it may result in a negative outcome";

        public List<Texture2D> playerSprites;

        public SpriteEffects flip;

        public List<Texture2D> swordOpeningAnimFloor1;

        public static int timer;

        public static Boss boss = new Boss(new Rectangle(500, 500, 200, 200));

        public Rectangle screenWhite;
        public Color screenColor;
        public Texture2D colorT;
        public int screenTimer;


        public BossRoom() : base()
        {
            swordOpeningAnimFloor1 = new List<Texture2D>();
            playerSprites = new List<Texture2D>();

            flip = SpriteEffects.None;

            oldKB = Keyboard.GetState();

            textureMaps.Add("swordOP", swordOpeningAnimFloor1);
            textureMaps.Add("player", playerSprites);

            mapDirs = new Dictionary<char, string>();
            mapDirs.Add('_', "Sprites/Floor1/Tiles/FloorTile");
            mapDirs.Add('a', "Sprites/Floor1/Tiles/grass");
            mapDirs.Add('I', "Sprites/Floor1/Tiles/Column");
            mapDirs.Add('x', "Sprites/Floor1/Tiles/FloorTile");

            timer = 0;

            screenWhite = new Rectangle(GlobalVars.camera.transformX, GlobalVars.camera.transformY, Game1.screenW, Game1.screenH);

            screenColor = Color.Transparent;

            screenTimer = 0;
            
        }

        public override void loadContent()
        {
            if (dirCount == 0)
            {
                Directory.SetCurrentDirectory(GlobalVars.Content.RootDirectory);
                dirCount++;
            }

            TileManager.tileLoad("Text Files/FloorBossRoom.txt");
            TileManager.tileLoad("Text Files/FloorBossRoomOT.txt");

            TileManager.loadAllTiles();
            //EnemyManager.spawnEnemy();
            GlobalVars.player.load();
            playerSprites = Loader.loadMultipleTextures("Sprites/Player/Normal");
            swordOpeningAnimFloor1 = Loader.loadMultipleTextures("Sprites/Floor1/sword");

            colorT = GlobalVars.Content.Load<Texture2D>("white");
        }

        public override void update(KeyboardState kbr, GameTime gameTime)
        {

            if (timer == 0)
            {
                HUD.drawText("WOW YOU MANAGED TO FIGURE IT OUT # Well, well, well # I guess I have no other choice,but TO DEFEAT YOU MYSELF", "BeginningCutSceneSprites/Sprites/chaxRegular");
                timer++;
            }
            TileManager.updateAllTiles();
            if (boss.HP >= 0f)
            {
                boss.update();
            }
            else
            {
                screenWhite.X = GlobalVars.camera.transformX;
                screenWhite.Y = GlobalVars.camera.transformY;
                screenWhite.Width = Game1.screenW;
                screenWhite.Height = Game1.screenH;
                boss.reset();
                if (screenColor != Color.Black)
                {
                    screenColor.A++;
                }
                else
                {
                    screenTimer++;
                    if (screenTimer > 60)
                    {
                        GlobalVars.gState = GlobalVars.GAMESTATE.WINGAMESCENE;
                    }
                }
            }
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
            boss.draw(GlobalVars.spriteBatch);
            GlobalVars.player.drawHealthBar();
            if (playerSprites != null)
                drawPlayer();
            if (swordOpeningAnimFloor1 != null)
                drawSword();
            HUD.drawHUD();

            if (boss.HP < 0)
            {
                GlobalVars.spriteBatch.Draw(colorT, screenWhite, screenColor);
            }
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
    }
}
