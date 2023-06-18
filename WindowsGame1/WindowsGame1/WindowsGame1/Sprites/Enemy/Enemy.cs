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
    class Enemy : Sprite
    {

        public int xVel;
        public int yVel;
        Vector2 velocity;
        public int hp;
        public float damage;

        public bool hasTakenDamage;
        public bool shouldRemove;

        public SpriteEffects flip;

        public Color color;

        Texture2D healthBarText;
        Rectangle healthBar;
        public double percentage;
        public int initialHealthBarWidth;
        public Color healthBarColor;

        public int collidingTimer;

        public int rangeOfSight = 1000;

        int randomMovementOnCollision;

        const float speed = 3f;

        Random random = new Random();

        int spawnCooldownTimer;

        bool enemiesSpawned;
        public Enemy(Rectangle r,int hp) : base(r)
        {
            updateHealthBar();

            xVel = 0;
            yVel = 0;
            this.hp = hp;
            flip = SpriteEffects.None;
            color = Color.White;

            healthBar = new Rectangle(100, 100, rect.Width, 10);
            percentage = 1;
            initialHealthBarWidth = healthBar.Width;
            healthBarColor = Color.White;

            collidingTimer = 0;
            velocity = Vector2.Zero;
            loadContent();

            damage = .1f;
            if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS)
            {
                damage = .1f / 60;
            }

            randomMovementOnCollision = random.Next(0, 2);

            spawnCooldownTimer = 0;

            enemiesSpawned = false;
        }

        public override void loadContent()
        {
            healthBarText = GlobalVars.Content.Load<Texture2D>("white");
        }

        public override void update()
        {
            updateHealthBar();
            velocity = Vector2.Zero;

            int x = rect.X - rangeOfSight / 2;
            int y = rect.Y - rangeOfSight / 2;
            int w = rect.Width + rangeOfSight;
            int h = rect.Height + rangeOfSight;
            //Console.WriteLine("Rect:\n" + "X: " + rect.X + "\nY: " + rect.Y + "\nW: " + rect.Width + "\nH: " + rect.Height);
            //Console.WriteLine("Sight:\n" + "X: " + x + "\nY: " + y + "\nW: " + w + "\nH: " + h);
            Rectangle sight = new Rectangle(x, y, w, h);

            if (GlobalVars.player.canMove)
                if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON && DungeonKey.correctKeyCollected)
                    trackPlayer(sight);
                else if (GlobalVars.gState != GlobalVars.GAMESTATE.DUNGEON)
                    trackPlayer(sight);
            if (GlobalVars.player.sword != null && !GlobalVars.player.sword.isSlashing)
            {
                hasTakenDamage = false;
            }
            if (percentage <= 0)
            {
                shouldRemove = true;
            }
            if (hasTakenDamage)
            {
                collidingTimer++;
                if (collidingTimer > 60)
                {
                    
                    hasTakenDamage = false;
                    collidingTimer = 0;
                }
            }
            checkFlip();

            damageReaction();

            checkCollisionWithPlayer();

            rect.X += (int)velocity.X;
            rect.Y += (int)velocity.Y;
        }
        public override void draw()
        {
            throw new NotImplementedException();
        }

        public void trackPlayer(Rectangle sight)
        {
            if (GlobalVars.player.playerRect.Intersects(sight) || GlobalVars.player.playerRect.Intersects(EnemyManager.enemyBoundList[0]))
            {
                
                float dx = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width / 2) - rect.X;
                float dy = GlobalVars.player.playerRect.Y + (GlobalVars.player.playerRect.Height / 2) - rect.Y;
                float hyp = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                Vector2 playerpos = new Vector2(GlobalVars.player.playerRect.X + GlobalVars.player.playerRect.Width / 2, GlobalVars.player.playerRect.Y + GlobalVars.player.playerRect.Height / 2); 
                Vector2 enemyPos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                velocity = playerpos - enemyPos;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
                velocity *= speed;
                //float speed = Math.Min(hyp, 5);
                //velocity.X = dx / (hyp / (speed / 2));
                //velocity.Y = dy / (hyp / (speed / 2));
                //velocity.Normalize();
                Console.WriteLine("Velocity: (" + velocity.X + ", " + velocity.Y + ")");
            }
        }

        public void takeDamage()
        {
            if (!hasTakenDamage)
            {
                hp--;
                hasTakenDamage = true;
            }
        }

        public void checkFlip()
        {
            float x = velocity.X;
            if (x < 0)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flip = SpriteEffects.None;
            }
        }
        public void damageReaction()
        {

            if (hasTakenDamage)
            {
                color = Color.Red;
            }
            else
            {
                color = Color.White;
            }
        }
        public void checkCollisionWithPlayer()
        {
            int x = GlobalVars.player.playerRect.X + 20;
            int y = GlobalVars.player.playerRect.Y + 20;
            int w = GlobalVars.player.playerRect.Width - 20;
            int h = GlobalVars.player.playerRect.Height - 20;
            Rectangle prect = new Rectangle(x, y, w, h);
            if (rect.Intersects(prect))
            {
                if (GlobalVars.gState != GlobalVars.GAMESTATE.BOSS)
                {
                    GlobalVars.player.takeDamage(damage / (float)EnemyManager.getNumberOfEnemiesCollidingWithPlayer() / 30f);
                }
                else
                {
                    GlobalVars.player.takeDamage(damage);
                }
                
            }
        }
        public void updateHealthBar()
        {
            healthBar.Y = rect.Y - 20 - healthBar.Height;
            Console.WriteLine("Health Y: " + healthBar.Y + " Rect Y: " + rect.Y + " Rect Width: " + rect.Width);
            healthBar.Width = (int)(initialHealthBarWidth * percentage);
            healthBar.X = (rect.X + rect.Width / 2) - (healthBar.Width / 2) - 15;
            if (percentage > .80)
                healthBarColor = Color.Green;
            else if (percentage > .60)
                healthBarColor = Color.YellowGreen;
            else if (percentage > .40)
                healthBarColor = Color.Orange;
            else if (percentage > .2)
                healthBarColor = Color.OrangeRed;
            else
                healthBarColor = Color.Red;
        }

        public void drawHealthBar()
        {
            GlobalVars.spriteBatch.Draw(healthBarText, healthBar, healthBarColor);
        }

        public void takeDamage(double percentage)
        {
            if (!hasTakenDamage)
            {
                this.percentage -= percentage;
                hasTakenDamage = true;
            }
        }
        public bool checkCollidingOtherEnemies()
        {
            for (int i = 0; i < EnemyManager.enemies.Count(); i++)
            {
                if (EnemyManager.enemies[i] == this) continue;

                if (EnemyManager.enemies[i].rect.Intersects(rect)) return true;
            }
            return false;
        }
    }
}
