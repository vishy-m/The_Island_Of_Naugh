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
    class Boss
    {
        // basics
        Vector2 pos;
        Vector2 velocity;

        float speed = 5f;

        public Rectangle rect;
        Texture2D text;

        public float HP;
        const float tartingHp = 5f;

        bool canTakeDamage;

        SpriteEffects flip;

        //Nomrmal attacking
        bool attack;
        int attackTimer;
        int attackDelay;

        //Animations
        int attackAnimTimer;
        int attackAnimSprit;
        List<Texture2D> attackAnimation = new List<Texture2D>();

        //Moves / attacks
        bool isDoingMove;
        bool[] attacks;
        const int numAttacks = 3;
        float attackWaitTime;
        const float attackWaitInitial = 60f * 1f;

        //Attack indexes
        const int Dash = 0;
        const int Slam = 1;
        const int Field = 2;

        //Dash Vars
        Vector2 dashPoint;
        const float dashSpeed = 10f;
        float dashTimer;
        const float dashTime = 60f * 1f;
        bool isDashing;
        const float dashInitiateDistance = 500f;
        const float dashDamage = .3f;

        //Slam Vars
        Rectangle slamCircle;
        float slamTime;
        float slamWaitTime;
        const float slamDuration = 60f;
        const float startingSlamWaitTime = 60f;
        const float slamInitiateDistance = 200f;
        bool isSlamming;
        List<Trap> slamTraps;

        //Field Vars
        int attackCount;
        const int numAttacksToTriggerField = 5;
        const int numEnemiesSpawned = 5;
        public List<Enemy> enemies;
        bool initialSpawn;
        const int boundize = 100;
        Texture2D enemyT;

        //Health Bar
        Texture2D healthBarText;
        Rectangle healthBar;
        int initialHealthBarWidth;
        Color healthBarColor;


        public Boss(Rectangle r)
        {
            pos = new Vector2(r.X, r.Y);
            velocity = Vector2.Zero;

            isDoingMove = false;

            rect = r;

            attackTimer = 0;
            attackDelay = 60;

            attack = false;
            canTakeDamage = true;

            attackAnimTimer = 0;
            attackAnimSprit = 0;

            flip = SpriteEffects.None;

            attacks = new bool[numAttacks];
            for (int i = 0; i < numAttacks; i++)
            {
                attacks[i] = false;
            }

            HP = tartingHp;

            //Dash Vars
            dashPoint = Vector2.Zero;
            dashTimer = dashTime;
            isDashing = false;
            attackWaitTime = attackWaitInitial;

            //Slam Vars
            slamCircle = Rectangle.Empty;
            slamTime = 0f;
            slamWaitTime = 0f;
            isSlamming = false;
            slamTraps = new List<Trap>();

            //Field Vars
            attackCount = 0;
            enemies = new List<Enemy>();
            initialSpawn = false;

            //health bar
            healthBar = new Rectangle(100, 100, rect.Width, 10);
            initialHealthBarWidth = healthBar.Width;
            healthBarColor = Color.White;

            // Loading textures and animations
            text = GlobalVars.Content.Load<Texture2D>("DeathCutScene/chax");

            enemyT = GlobalVars.Content.Load<Texture2D>("knight");

            healthBarText = GlobalVars.Content.Load<Texture2D>("white");
        }

        public void update()
        {

            velocity = Vector2.Zero;

            updateHealthBar();

            if (!HUD.writingText)
            {

                if (!isDoingMove)
                {
                    trackPlayer();
                    if (rect.Intersects(GlobalVars.player.playerRect) && !attack)
                    {
                        attackTimer++;
                        if (attackTimer >= attackDelay)
                        {
                            attack = true;
                            attackTimer = 0;
                        }
                    }

                    if (attack)
                    {
                        //deal damage to player
                    }
                }

                checkAttacks();
            }


            if (velocity.X > 0)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            if (velocity.X < 0)
            {
                flip = SpriteEffects.None;
            }

            rect.X += (int)(velocity.X * speed);
            rect.Y += (int)(velocity.Y * speed);

            pos.X = rect.X;
            pos.Y = rect.Y;

            //for (int i = 0; i < attacks.Length; i++)
            //{
            //Console.WriteLine("Attack " + i + ": " + attacks[i]);
            //}

        }

        public void draw(SpriteBatch batch)
        {
            //batch.Draw(slamText, slamCircle, Color.Red);
            for (int i = 0; i < slamTraps.Count(); i++)
            {
                slamTraps[i].draw(batch);
            }
            Vector2 cent = new Vector2(rect.Width / 2, rect.Height / 2);
            batch.Draw(text, rect, Color.White);
            GlobalVars.spriteBatch.Draw(healthBarText, healthBar, healthBarColor);
            for (int i = 0; i < enemies.Count(); i++)
            {
                Vector2 center = new Vector2(enemyT.Width / 2, enemyT.Height / 2);
                GlobalVars.spriteBatch.Draw(enemyT, enemies[i].rect, null, enemies[i].color, 0f, center, enemies[i].flip, 0);
            }

        }

        public void trackPlayer()
        {
            Vector2 playerpos = new Vector2(GlobalVars.player.playerRect.X, GlobalVars.player.playerRect.Y);
            velocity = playerpos - pos;
            velocity.Normalize();
        }

        //managing attack
        public void checkAttacks()
        {
            Vector2 playerpos = new Vector2(GlobalVars.player.playerRect.X, GlobalVars.player.playerRect.Y);
            Vector3 playerV3 = new Vector3(playerpos, 0);
            Vector3 posV3 = new Vector3(pos, 0);

            float distance = Vector3.Distance(playerV3, posV3);
            //Console.WriteLine("Distance: " + distance);
            //Console.WriteLine("attack count: " + attackCount + " enemie: " + enemies.Count());

            if (!isDoingMove)
            {
                //attack Checking
                checkDash(distance);
                checkSlam(distance);
                checkField();
            }

            for (int i = slamTraps.Count() - 1; i > -1; i--)
            {
                slamTraps[i].update();
                if (slamTraps[i].shouldRemove)
                {
                    slamTraps.RemoveAt(i);
                }
            }

            for (int i = enemies.Count() - 1; i > -1; i--)
            {
                enemies[i].update();
                if (enemies[i].shouldRemove)
                {
                    enemies.RemoveAt(i);
                }
            }

            if (isDoingMove)
            {
                if (attacks[Dash])
                {
                    dashMechanics(playerpos);
                }
                if (attacks[Slam])
                {
                    slamMechanics(playerpos);
                    //Console.WriteLine("lamming RN ");
                }
                if (attacks[Field])
                {
                    FieldMechanics(); 
                    //Console.WriteLine("Fielding RN " + enemies.Count()); 
                }
            }


        }

        //Checking for the activation of attacks
        public void checkDash(float distance)
        {
            if (distance > dashInitiateDistance)
            {
                attacks[Dash] = true;
                isDoingMove = true;
                isDashing = true;
                return;
            }
        }
        public void checkSlam(float distance)
        {
            if (distance <= slamInitiateDistance)
            {
                attacks[Slam] = true;
                isDoingMove = true;
                isSlamming = true;
                return;
            }
        }
        public void checkField()
        {
            if (attackCount >= numAttacksToTriggerField)
            {
                attacks[Field] = true;
                isDoingMove = true;
                return;
            }
        }


        //Performing the attacks
        public void dashMechanics(Vector2 playerpos)
        {
            if (isDashing)
            {

                dashTimer++;

                if (dashTimer > dashTime)
                {
                    velocity = dashPoint - pos;
                    velocity.Normalize();
                    speed = dashSpeed;

                    Rectangle dashPR = new Rectangle((int)dashPoint.X - 50, (int)dashPoint.Y - 50, 100, 100);

                    if (rect.Intersects(dashPR) || rect.Intersects(new Rectangle((int)playerpos.X, (int)playerpos.Y, 100, 100)))
                    {
                        if (rect.Intersects(new Rectangle((int)playerpos.X, (int)playerpos.Y, 100, 100)))
                        {
                            GlobalVars.player.takeDamage(dashDamage);
                        }
                        speed = 5f; //set back to original 
                        isDashing = false;
                        dashTimer = 0;
                        dashPoint = Vector2.Zero;
                    }

                }
                else
                {
                    dashPoint = playerpos;
                }
            }
            else
            {
                attackWaitTime--;
                if (attackWaitTime <= 0)
                {
                    attackWaitTime = attackWaitInitial;
                    isDoingMove = false;
                    attacks[Dash] = false;
                    attackCount++;
                }
            }
        }
        public void slamMechanics(Vector2 playerpos)
        {
            if (isSlamming)
            {
                slamWaitTime++;

                if (slamWaitTime >= startingSlamWaitTime)
                {
                    if (slamWaitTime == startingSlamWaitTime)
                    {
                        slamCircle = new Rectangle(rect.X - rect.Width, rect.Y - rect.Height, rect.Width * 3, rect.Height * 3);
                        slamTraps.Add(new Trap(slamCircle, "Eye", (int)(slamDuration / 1.25), 60));
                    }

                    slamTime++;

                    Rectangle playerRectangle = new Rectangle((int)playerpos.X, (int)playerpos.Y, 100, 100);

                    if (slamTime > slamDuration)
                    {
                        isSlamming = false;
                    }

                    if (!isSlamming)
                    {
                        slamWaitTime = 0f;
                        slamTime = 0f;
                    }
                }
            }
            else
            {
                attackWaitTime--;
                if (attackWaitTime <= 0)
                {
                    attackWaitTime = attackWaitInitial;
                    isDoingMove = false;
                    attacks[Slam] = false;
                    attackCount++;
                }
            }
        }
        public void FieldMechanics()
        {
            //Console.WriteLine("Should be Feilding");
            if (!initialSpawn)
            {
                for (int i = 0; i < numEnemiesSpawned; i++)
                {
                    //spawn enemies || make modifier to the spawn enemy method that returns an enemy || add that enemy to the enemies list
                    Rectangle panR = new Rectangle(rect.X - boundize, rect.Y - boundize, rect.Width + boundize, rect.Height + boundize);
                    enemies.Add(EnemyManager.spawnEnemy(panR));
                }

                canTakeDamage = false;
                initialSpawn = true;
            }
            if (enemies.Count() == 0)
            {

                canTakeDamage = true;
                initialSpawn = false;
                isDoingMove = false;
                attacks[Field] = false;
                attackCount = 0;
            }
        }

        public void takeDamage(float percent)
        {
            if (canTakeDamage)
            {
                HP -= percent;
            }
        }
        public void updateHealthBar()
        {
            healthBar.Y = rect.Y - 20 - healthBar.Height;
            Console.WriteLine("Health Y: " + healthBar.Y + " Rect Y: " + rect.Y + " Rect Width: " + rect.Width);
            healthBar.Width = (int)((initialHealthBarWidth * HP) / tartingHp);
            healthBar.X = (rect.X + rect.Width / 2) - (healthBar.Width / 2);
            if (HP > tartingHp * .8f)
                healthBarColor = Color.Green;
            else if (HP > tartingHp * .6f)
                healthBarColor = Color.YellowGreen;
            else if (HP > tartingHp * .4f)
                healthBarColor = Color.Orange;
            else if (HP > tartingHp * .2f)
                healthBarColor = Color.OrangeRed;
            else
                healthBarColor = Color.Red;
        }
        public void reset()
        {

            isDoingMove = false;
            attack = false;
            canTakeDamage = true;

            attacks = new bool[numAttacks];
            for (int i = 0; i < numAttacks; i++)
            {
                attacks[i] = false;
            }
            isDashing = false;
            slamTime = 0f;
            slamWaitTime = 0f;
            isSlamming = false;
        }
    }
}

