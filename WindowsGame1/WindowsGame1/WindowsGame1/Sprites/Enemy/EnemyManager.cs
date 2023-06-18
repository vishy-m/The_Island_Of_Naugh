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
    class EnemyManager
    {
        public static Rectangle startingEnemyBounds;
        public static Rectangle endingEnemyBounds;
        public static Rectangle enemyBounds;

        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Rectangle> initialSpawnRects = new List<Rectangle>();
        public static List<Rectangle> endingSpawnRects = new List<Rectangle>();
        public static List<Rectangle> enemyBoundList = new List<Rectangle>();

        //public static Texture2D enemyTexture;

        static Random random = new Random();

        public static Vector2 enemyDimensions = new Vector2(50, 50);

        public static int displacement = 0;
        public static int enemyBoundCallCount = 0;

        public static bool initialspawn = false;

        public static void spawnEnemy(int boundInt)
        {

            Rectangle enemyRect;
            Rectangle playerBounds = GlobalVars.player.playerRect;

            playerBounds.X -= displacement;
            playerBounds.Y -= displacement;
            playerBounds.Width += displacement;
            playerBounds.Height += displacement;

            do
            {
                int x = random.Next(enemyBoundList[boundInt].X + displacement, (enemyBoundList[boundInt].X + enemyBoundList[boundInt].Width) - displacement);
                int y = random.Next(enemyBoundList[boundInt].Y + displacement, (enemyBoundList[boundInt].Y + enemyBoundList[boundInt].Height) - displacement);
                enemyRect = new Rectangle(x, y, (int)enemyDimensions.X, (int)enemyDimensions.Y);
                
            }
            while (enemyRect.Intersects(playerBounds));

            if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS && enemies.Count() == 0)
            {
                enemyRect.Width = 200;
                enemyRect.Height = 200;
            }
            Enemy enemy = new Enemy(enemyRect, 3);
            enemies.Add(enemy);
        }
        public static Enemy spawnEnemy(Rectangle r)
        {

            Rectangle enemyRect;
            Rectangle playerBounds = GlobalVars.player.playerRect;

            playerBounds.X -= displacement;
            playerBounds.Y -= displacement;
            playerBounds.Width += displacement;
            playerBounds.Height += displacement;

            do
            {
                int x = random.Next(r.X + displacement, (r.X + r.Width) - displacement);
                int y = random.Next(r.Y + displacement, (r.Y + r.Height) - displacement);
                enemyRect = new Rectangle(x, y, (int)enemyDimensions.X, (int)enemyDimensions.Y);

            }
            while (enemyRect.Intersects(playerBounds));

            Enemy enemy = new Enemy(enemyRect, 3);
            return enemy;
        }

        public static void makeEnemyBounds(Rectangle rect, char type, int index)
        {

            if (type == 'E')
            {
                for (int i = index - 1; i > -1; i--)
                {
                    if (TileManager.allTiles[i].type == 'e')
                    {
                        Rectangle r  = TileManager.allTiles[i].rect;
                        int x = r.X;
                        int y = r.Y;
                        int w = rect.X + rect.Width - r.X;
                        int h = rect.Y + rect.Height - r.Y;

                        Rectangle finalRect = new Rectangle(x, y, w, h);
                        enemyBoundList.Add(finalRect);
                        break;
                    }
                }
            }
            
        }

        public static void updateAllEnemies()
        {
            if (!initialspawn)
            {
                if (enemyBoundList.Count() > 0)
                {
                    if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            spawnEnemy(0);
                        }
                    }
                    if (GlobalVars.gState == GlobalVars.GAMESTATE.FOREST)
                    {
                        for (int i = 0; i < enemyBoundList.Count(); i++)
                        {
                            int numEnemies = random.Next(5, 10);
                            for (int j = 0; j < numEnemies; j++)
                            {
                                spawnEnemy(i);
                            }
                        }
                    }
                    if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS)
                    {
                        spawnEnemy(0);
                        //enemies[0].hp = 5;
                        enemies[0].percentage = 5f;
                        enemies[0].damage = .2f;

                    }
                    initialspawn = true;
                }
                Console.WriteLine(" num enemy bound: " + enemyBoundList.Count());
                for (int i = 0; i < enemyBoundList.Count(); i++)
                {
                    Rectangle r = enemyBoundList[i];
                    Console.WriteLine("(" + r.X + ", " + r.Y + ", " + r.Width + ", " + r.Height + ")");
                }
            }

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].update();

            }
            for (int i = enemies.Count() - 1; i > -1; i--)
            {
                if (enemies[i].shouldRemove)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        public static void drawEnemies(Texture2D enemyTexture)
        {
            for (int i = 0; i < enemies.Count(); i++)
            {
                Vector2 center = new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2);
                
                if (GlobalVars.gState != GlobalVars.GAMESTATE.BOSS || (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS && i != 0))
                {
                    enemies[i].drawHealthBar();
                   
                }
                if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS && i == 0)
                {
                    GlobalVars.spriteBatch.Draw(GlobalVars.Content.Load<Texture2D>("BeginningCutSceneSprites/Sprites/chaxRegular"), enemies[i].rect, null, enemies[i].color, 0f, center, enemies[i].flip, 0);
                }
                else
                {
                    GlobalVars.spriteBatch.Draw(enemyTexture, enemies[i].rect, null, enemies[i].color, 0f, center, enemies[i].flip, 0);
                }

            }
        }


        public static void checkEnemyLifeLost(Rectangle r)
        {
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].rect.Intersects(r) && !enemies[i].hasTakenDamage)
                {
                    enemies[i].takeDamage(GlobalVars.player.sword.damage);
                }
            }
        }
        public static int getNumberOfEnemiesCollidingWithPlayer()
        {
            int num = 0;
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].rect.Intersects(GlobalVars.player.playerRect))
                {
                    num++;
                }
            }
            return num;
        }

        public static void reset()
        {
            enemies = new List<Enemy>();
            initialSpawnRects = new List<Rectangle>();
            endingSpawnRects = new List<Rectangle>();
            enemyBoundList = new List<Rectangle>();
            enemyDimensions = new Vector2(50, 50);
            displacement = 0;
            enemyBoundCallCount = 0;
            initialspawn = false;
        }
    }
}
