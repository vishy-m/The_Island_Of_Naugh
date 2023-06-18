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

    public class Dungeon : Floor
    {
       
        public static string dungeonInfoText = "Here are the basic controls: Moving: WASD, # Dashing: Q,# Interact: E,# and Attack: SPACE";

        public List<Texture2D> playerSprites;

        public SpriteEffects flip;

        public List<Texture2D> playerIdleAnimation;
        public List<Texture2D> prisonBarSprites;
        public List<Texture2D> swordOpeningAnimFloor1;

        public Texture2D keyTex;
        public Texture2D husk;
        public Texture2D blankButtonText;
        public static Texture2D textTexture;

        public static SoundEffect dungeonBackgroundMusic;

        public int dungeonBackgroundMusicTimer;
        public int dungeonTimer;
        public int timer;

        int t;

        public Dungeon() : base()
        {
            prisonBarSprites = new List<Texture2D>();
            playerIdleAnimation = new List<Texture2D>();
            swordOpeningAnimFloor1 = new List<Texture2D>();
            playerSprites = new List<Texture2D>();

            flip = SpriteEffects.None;

            dungeonBackgroundMusicTimer = 0;
            dungeonTimer = 60 * 5;
            timer = 0;

            oldKB = Keyboard.GetState();

            textureMaps.Add("prisonBar", prisonBarSprites);
            textureMaps.Add("playerIdle", playerIdleAnimation);
            textureMaps.Add("swordOP", swordOpeningAnimFloor1);
            textureMaps.Add("player", playerSprites);

            mapDirs = new Dictionary<char, string>();
            mapDirs.Add('_', "DungeonSprites/FloorTile");
            mapDirs.Add('a', "DungeonSprites/AnotherFloorTile");
            mapDirs.Add('I', "DungeonSprites/WallTile");
            mapDirs.Add('g', "DungeonSprites/Gate1");
            mapDirs.Add('G', "DungeonSprites/Gate2");
            mapDirs.Add('t', "DungeonSprites/skull");
            mapDirs.Add('n', "DoorOpened");
            mapDirs.Add('N', "DoorClosed");
            t = 0;
        }
        public override void loadContent()
        {
            if (dirCount == 0)
            {
                Directory.SetCurrentDirectory(GlobalVars.Content.RootDirectory);
                dirCount++;
            }

            TileManager.tileLoad("Text Files/tileMap.txt");
            TileManager.tileLoad("Text Files/overTileMap.txt");

            TileManager.loadAllTiles();

            playerIdleAnimation = Loader.loadMultipleTextures("Sprites/Player/IdleAnim");
            swordOpeningAnimFloor1 = Loader.loadMultipleTextures("Sprites/Floor1/sword");
            playerSprites = Loader.loadMultipleTextures("Sprites/Player/normal");

            dungeonBackgroundMusic = GlobalVars.Content.Load<SoundEffect>("DungeonSFX/background music");

            keyTex = GlobalVars.Content.Load<Texture2D>("Sprites/Keys/demonic key");
            husk = GlobalVars.Content.Load<Texture2D>("Blue Husk");

            GlobalVars.player.load();
        }
        public override void update(KeyboardState kbr, GameTime gameTime)
        {
            //updateTiles();
            timer++;

            if (timer == 1)
            {
                string chaxSpeech = "I HAVE BROUGHT YOU TO THIS CASTLE, YOU CANNOT ESCAPE # MUHAHAHHAAHAHAHAHAHAHAHAHAHAH";

                HUD.drawText(chaxSpeech, "BeginningCutSceneSprites/Sprites/chaxRegular");
            }
            TileManager.updateAllTiles();

            GlobalVars.player.update(TileManager.allTiles, gameTime);

            updateKeys(kbr);

            playBackgroundMusic();

            //Collectible.updateCollectibles();

            EnemyManager.updateAllEnemies();
            
            

            if (GlobalVars.player.flip)
                flip = SpriteEffects.FlipHorizontally;
            else
                flip = SpriteEffects.None;

            GlobalVars.camera.follow(GlobalVars.player.playerRect);

            Sprite.checkRemoveSprites();

            oldKB = kbr;
        }
        public override void draw()
        {
            //drawTiles();
            TileManager.drawAllTiles();
            drawPlayer();
            drawSword();
            //drawTimer();
            EnemyManager.drawEnemies(husk);
            DungeonKey.drawKeys(GlobalVars.spriteBatch);
            Sprite.drawInteractibleKey();
            GlobalVars.player.drawHealthBar();
            HUD.drawHUD();
        }
        public void updateKeys(KeyboardState kbr)
        {
            if (DungeonKey.boundTimer == 2)
            {
                DungeonKey.updateKeys();
                if (!DungeonKey.correctKeyCollected)
                {
                    DungeonKey.checkAddKeys(GlobalVars.player, keyTex, kbr, oldKB);
                }
            }
        }
        public void playBackgroundMusic()
        {
            if (dungeonBackgroundMusicTimer == 0)
            {
                //dungeonBackgroundMusic.Play();
                dungeonBackgroundMusicTimer++;
            }
            if (dungeonBackgroundMusicTimer >= 1 && dungeonBackgroundMusicTimer < dungeonBackgroundMusic.Duration.TotalSeconds * 60)
            {
                dungeonBackgroundMusicTimer++;
            }
            else
            {
                dungeonBackgroundMusicTimer = 0;
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
        public void drawTimer()
        {
            double startingNum = Math.Floor(dungeonTimer / 60.0);
            double endingNum = ((dungeonTimer / 100) - startingNum) * 100;
            string clock;
            if (dungeonTimer % 60 > 9)
            {
                clock = (dungeonTimer / 60).ToString() + ":" + (dungeonTimer % 60).ToString();
            }
            //displays the current time remaining
            else
            {
                clock = (dungeonTimer / 60).ToString() + ":0" + (dungeonTimer % 60).ToString();
            }

            Rectangle clockRect = new Rectangle(GlobalVars.camera.transformX, GlobalVars.camera.transformY, 95, 50);
            Vector2 clockTextPos = new Vector2(GlobalVars.camera.transformX + 25, GlobalVars.camera.transformY + 14);

            GlobalVars.spriteBatch.Draw(HUD.textBackgroundText, clockRect, Color.White);
            GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, clock, clockTextPos, Color.White);
        }

        public static void stopBackgroundMusic()
        {
            if (dungeonBackgroundMusic != null)
            {
                dungeonBackgroundMusic.Dispose();
            }
        }

    }
}
