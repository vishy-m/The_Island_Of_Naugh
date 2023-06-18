using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Player
    {
        public bool isFacingBack;

        public Rectangle playerRect = new Rectangle(350, 350, 100, 100);
        bool alive;
        public int sprit;
        public Boolean flip;
        public int animTimer;
        public Boolean lostLife;
        public int lifeLostTimer;
        public int speed;
        public KeyboardState oldKB;
        public int leftAnimTimer;
        public int rightAnimTimer;
        public bool isIdle;
        public int idleSprit;
        public int idleAnimTimer;
        public int numTimesIdle;
        public int idleAnimFrameDelay;
        public int startingIdleFrameDelay;

        public Sword sword;

        public Color color;

        Texture2D healthBarText;
        Rectangle healthBar;
        public double percentage;
        public int initialHealthBarWidth;
        public Color healthBarColor;

        public List<Keys> movingKeys;


        public bool canMove;

        public Vector2 velocity;
        public const float playerSpeed = 5f;
        public const float dashSpeed = 20f;
        public float dashTime;
        public const float startDashTime = .2f;
        public bool isDashing;
        public int direction = 0;
        public bool cannotMove = false;
        public List<Particle> particles = new List<Particle>();
        public int dashTimer;
        public int startDashTimer = 0;

        List<Texture2D> normalSprites;
        List<Texture2D> idleAnim;
        List<Texture2D> dashAnim;

        public SpriteEffects flipp;
        public SpriteEffects dashFlip;

        public int dashSprit;
        public int dashAnimTimer;

        public bool reverseAnim;

        public bool swordEquiped;
        public bool flamethrowerEquiped;

        GamePadState oldPad;

        public int dashCooldown;
        public bool canDash;

        public Player(double hp)
        {
            percentage = hp;
            alive = true;
            sprit = 0;
            flip = false;
            animTimer = 0;
            lostLife = false;
            lifeLostTimer = 0;
            speed = 5;
            leftAnimTimer = 0;
            rightAnimTimer = 0;
            idleAnimTimer = 0;
            isIdle = false;
            idleSprit = 0;
            numTimesIdle = 0;
            startingIdleFrameDelay = 10;
            idleAnimFrameDelay = startingIdleFrameDelay;
            flipp = SpriteEffects.None;
            oldKB = Keyboard.GetState();

            canMove = true;

            color = Color.White;

            healthBar = new Rectangle(100, 100, playerRect.Width, 10);
            percentage = 1;
            initialHealthBarWidth = healthBar.Width;
            healthBarColor = Color.White;

            healthBar.Y = playerRect.Y - 20 - healthBar.Height;

            loadHealthBar();

            movingKeys = new List<Keys>();
            movingKeys.Add(Keys.W);
            movingKeys.Add(Keys.A);
            movingKeys.Add(Keys.S);
            movingKeys.Add(Keys.D);

            velocity = Vector2.Zero;

            isDashing = false;
            dashTime = startDashTime;

            dashSprit = 0;
            dashAnimTimer = 0;

            dashTimer = startDashTimer;

            reverseAnim = false;

            dashFlip = SpriteEffects.None;

            swordEquiped = false;
            flamethrowerEquiped = false;

            oldPad = GamePad.GetState(PlayerIndex.One);

            dashCooldown = 0;
            canDash = true;

            setSword(new Rectangle(-1000000, -1000000, 100, 100));
            
        }
        public void load()
        {
            normalSprites = Loader.loadMultipleTextures("Sprites/Player/normal");
            idleAnim = Loader.loadMultipleTextures("Sprites/Player/IdleAnim");
            dashAnim = Loader.loadMultipleTextures("DashAnimP");
        }
        public void update(List<Tile2> tiles, GameTime gameTime)
        {
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            velocity = Vector2.Zero;
            float delta = gameTime.ElapsedGameTime.Milliseconds;
            delta = delta / 1000;

            Vector2 moveMent = Vector2.Zero;

            updateHealthBar();

            List<char> tileType = new List<char>();
            
            KeyboardState kbr = Keyboard.GetState();

            for (int i = 0; i < tiles.Count(); i++)
            {
                //Console.WriteLine(i);
                if (tiles[i].type == 'u' || tiles[i].type == 'l' || tiles[i].type == 'd' || tiles[i].type == 'r')
                {
                    if (playerRect.Intersects(tiles[i].rect))
                    {
                        tileType.Add(tiles[i].type);
                    }
                }
            }
                

            isIdle = true;
            isFacingBack = false;


            if (canMove)
            {
                flipp = SpriteEffects.None;
                if (!tileType.Contains('u') && (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.UP]) || pad.ThumbSticks.Left.Y > 0))
                {
                    sprit = 0;
                    resetVars();
                    isFacingBack = true;
                    //playerRect.Y -= speed;
                    moveMent.Y = -1;
                    direction = 4;
                }
                if (!tileType.Contains('d') && (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.DOWN]) || pad.ThumbSticks.Left.Y < 0))
                {
                    sprit = 1;
                    resetVars();
                    //playerRect.Y += speed;
                    moveMent.Y = 1;
                    direction = 3;
                }
                if (!tileType.Contains('r') && (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.RIGHT]) || pad.ThumbSticks.Left.X > 0))
                {
                    resetRightVars();
                    anim(rightAnimTimer, 2, 3);
                    rightAnimTimer++;
                    //playerRect.X += speed;
                    moveMent.X = 1;
                    direction = 2;

                }
                else if (!tileType.Contains('l') && (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.LEFT]) || pad.ThumbSticks.Left.X < 0))
                {
                    resetLeftVars();
                    anim(leftAnimTimer, 2, 3);
                    leftAnimTimer++;
                    //playerRect.X -= speed;
                    moveMent.X = -1;
                    direction = 1;
                }
            }

            velocity.X = moveMent.X * playerSpeed;
            velocity.Y = moveMent.Y * playerSpeed;


            dashCooldown++;

            if (dashCooldown > 30)
            {
                canDash = true;
            }

            if (canDash)
            {
                dashCalls(kbr, delta, pad);
            }


            for (int i = particles.Count() - 1; i > -1; i--)
            {
                particles[i].update();
                if (particles[i].remove)
                {
                    particles.RemoveAt(i);
                }
            }
            if (sword != null)
            {
                if (isIdle && !sword.hasBeenCollected)
                {
                    numTimesIdle++;
                    if (numTimesIdle >= 60 * 5)
                    {
                        idleAnimation(idleAnimTimer, 12, 14);
                        idleAnimTimer++;
                    }
                }
            }

            if (lostLife)
                color = Color.LightSalmon;
            else
                color = Color.White;

            //checkElectrify(overMap);

            if (lostLife)
            {
                lifeLostTimer++;
            }
            else
            {
                lifeLostTimer = 0;
            }
            if (lifeLostTimer > 60)
            {
                lostLife = false;
                lifeLostTimer = 0;
                //t.sprit++;
            }

            if (kbr.IsKeyDown(Keys.D2) && oldKB.IsKeyDown(Keys.D2))
            {
                swordEquiped = false;
                flamethrowerEquiped = true;
                sword.hasBeenCollected = false;
            }
            if (kbr.IsKeyDown(Keys.D1) && oldKB.IsKeyDown(Keys.D1))
            {
                swordEquiped = true;
                flamethrowerEquiped = false;
                sword.hasBeenCollected = true;
            }
            if (sword != null & swordEquiped)
            {
                sword.update();
            }
            if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS)
            {
                Collectible.updateCollectibles();
            }
            Collectible.updateCollectibles();
            //Collectible.updateCollectibles();

            playerRect.X += (int)velocity.X;
            playerRect.Y += (int)velocity.Y;

            oldKB = kbr;
            oldPad = pad;
        }

        public void dash(KeyboardState kb, float delta, GamePadState pad)
        {
            //Console.WriteLine(isDashing.ToString());
            if (!cannotMove)
            {
                if (kb.IsKeyDown(InputMaps.keyboardInputs[InputMaps.DASH]) && !oldKB.IsKeyDown(InputMaps.keyboardInputs[InputMaps.DASH]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.DASH]) && !oldPad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.DASH]))
                {
                    isDashing = true;
                    //particles.Add(new Particle(playerRect));
                }
            }
            if (direction != 0 && isDashing)
            {
                particles.Add(new Particle(playerRect));
                if (dashTime > 0)
                {
                    dashTime -= delta;
                    if (direction == 1)
                    {
                        velocity = new Vector2(-1, 0) * dashSpeed;
                    }
                    if (direction == 2)
                    {
                        velocity = new Vector2(1, 0) * dashSpeed;
                    }
                    if (direction == 3)
                    {
                        velocity = new Vector2(0, 1) * dashSpeed;
                    }
                    if (direction == 4)
                    {
                        velocity = new Vector2(0, -1) * dashSpeed;
                    }
                    
                }
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                    isDashing = false;
                    dashTimer--;
                }
                int x = playerRect.X;
                int y = playerRect.Y;
                Rectangle futureR = new Rectangle(x += (int)velocity.X, y += (int)velocity.Y, playerRect.Width, playerRect.Height);
                for (int i = 0; i < TileManager.allTiles.Count(); i++)
                {
                    if (TileManager.allTiles[i] is ImpassableTile)
                    {
                        if (!futureR.Intersects(TileManager.allTiles[i].rect)) continue;
                        bool goingDown = direction == 3 && TileManager.allTiles[i].type == 'd';
                        bool goingUp = direction == 4 && TileManager.allTiles[i].type == 'u';
                        bool goingRight = direction == 2 && TileManager.allTiles[i].type == 'r';
                        bool goingLeft = direction == 1 && TileManager.allTiles[i].type == 'l';
                        if (goingDown || goingUp || goingRight || goingLeft)
                        {
                            velocity = Vector2.Zero;
                            dashTimer--;
                        }
                    }
                }
            }
            

            if (dashTimer != startDashTimer)
            {
                cannotMove = true;
                dashTimer--;
                if (dashTimer <= 0)
                {
                    dashTimer = startDashTimer;
                    cannotMove = false;
                    canDash = false;
                    dashCooldown = 0;
                }
            }
        }
        public void anim(int animTimer, int minSpritValue, int maxSpritValue)
        {

            if (animTimer == 0)
            {
                sprit = minSpritValue;
            }
                
            if (animTimer % 50 == 0)
            {
                if (sprit < maxSpritValue)
                {
                    sprit++;
                }
                else
                {
                    if (sprit >= maxSpritValue)
                    {
                        sprit = minSpritValue;
                    }
                }
            }
            
        }

        public void idleAnimation(int animTimer, int minSprit, int maxSprit)
        {
            if (animTimer == 0)
            {
                idleSprit = 0;
            }
            if (animTimer % idleAnimFrameDelay == 0)
            {
                if (idleSprit < maxSprit)
                {
                    idleSprit++;
                }
                if (idleSprit >= maxSprit)
                {
                    idleSprit = minSprit;
                    idleAnimFrameDelay = 50;
                }
            }
            //Console.WriteLine("Sprit: " + idleSprit);
        }
        public void resetVars()
        {
            leftAnimTimer = 0;
            rightAnimTimer = 0;
            idleAnimTimer = 0;
            idleSprit = 0;
            isIdle = false;
            flip = true;
            idleAnimFrameDelay = startingIdleFrameDelay;
            numTimesIdle = 0;
            isFacingBack = false;
            
        }
        public void resetLeftVars()
        {
            rightAnimTimer = 0;
            idleAnimTimer = 0;
            isIdle = false;
            flip = true;
            flipp = SpriteEffects.FlipHorizontally;
            idleSprit = 0;
            idleAnimFrameDelay = startingIdleFrameDelay;
            numTimesIdle = 0;
            isFacingBack = false;
        }
        public void resetRightVars()
        {
            leftAnimTimer = 0;
            idleAnimTimer = 0;
            isIdle = false;
            flipp = SpriteEffects.None;
            flip = false;
            idleSprit = 0;
            idleAnimFrameDelay = startingIdleFrameDelay;
            numTimesIdle = 0;
            isFacingBack = false;
        }
        public void checkElectrify(Tile[,] ot)
        {
            KeyboardState kbr = Keyboard.GetState();

            for (int r = 0; r < ot.GetLength(0); r++)
            {
                for (int c = 0; c < ot.GetLength(1); c++)
                {
                    Tile t = ot[r, c];
                    if (!t.isPrisonBar) continue;
                    if (!playerRect.Intersects(t.rect)) continue;

                    if (kbr.GetPressedKeys().Length > 0 && !kbr.GetPressedKeys().Contains(Keys.E))
                    {
                        electrify(t);
                        //Console.WriteLine("should be electrified");
                    }
                    //Console.WriteLine(kbr.GetPressedKeys().Length + " " + kbr.GetPressedKeys()[0]);
                    
                }
            }
        }

        public void electrify(Tile t)
        {
            if (t.barIsElectric)
            {
                takeDamage(.2 / 60);
                lostLife = true;
            }

        }

        public void setSword(Rectangle r)
        {
            sword = new Sword(r);
        }

        public void updateHealthBar()
        {
            healthBar.Width = (int)(initialHealthBarWidth * percentage);
            healthBar.X = (playerRect.X + playerRect.Width / 2) - (healthBar.Width / 2) - 15;
            healthBar.Y = playerRect.Y - 25;
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
        public void loadHealthBar()
        {
            healthBarText = GlobalVars.Content.Load<Texture2D>("white");
        }

        public void drawHealthBar()
        {
            GlobalVars.spriteBatch.Draw(healthBarText, healthBar, healthBarColor);
        }

        public void takeDamage(double percentage)
        {
            if (!isDashing)
            {
                this.percentage -= percentage;
                lostLife = true;
            }
        }
        public void dashCalls(KeyboardState kbr, float delta, GamePadState pad)
        {
            if (canMove && velocity != Vector2.Zero)
            {
                dash(kbr, delta, pad);
            }
            if (direction == 4)
            {
                dashAnim = Loader.loadMultipleTextures("DashAnimV");
                dashFlip = SpriteEffects.None;
            }
            else if (direction == 3)
            {
                dashAnim = Loader.loadMultipleTextures("DashAnimV");
                dashFlip = SpriteEffects.FlipVertically;
            }
            else if (direction == 1)
            {
                dashAnim = Loader.loadMultipleTextures("DashAnimP");
                dashFlip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                dashAnim = Loader.loadMultipleTextures("DashAnimP");
                dashFlip = SpriteEffects.None;
            }

            if (dashTime != startDashTime)
            {
                if (dashAnimTimer == 0)
                {
                    dashSprit = 0;
                }
                dashAnimTimer++;
                if (dashAnimTimer % 1 == 0)
                {
                    if (!reverseAnim)
                    {
                        dashSprit++;
                    }
                    else
                    {
                        dashSprit--;
                    }
                }
                if (dashSprit >= dashAnim.Count())
                {
                    reverseAnim = true;
                    dashSprit--;
                }
                if (dashSprit < 0)
                {
                    reverseAnim = true;
                    dashSprit++;
                }
            }
            else
            {
                dashAnimTimer = 0;
                dashSprit = 0;
                reverseAnim = false;
            }

        }

        public void draw()
        {
            Vector2 center = new Vector2(playerRect.Width / 2, playerRect.Height / 2);
            if (isIdle)
                GlobalVars.spriteBatch.Draw(idleAnim[idleSprit], playerRect, null, color, 0, center, flipp, 0);
            else if (dashTime != startDashTime) 
            {
                center = new Vector2(dashAnim[dashSprit].Width / 2, dashAnim[dashSprit].Height / 2 - 55);
                GlobalVars.spriteBatch.Draw(dashAnim[dashSprit], playerRect, null, color, 0, center, dashFlip, 0);
            }
            else
                GlobalVars.spriteBatch.Draw(normalSprites[sprit], playerRect, null, color, 0, center, flipp, 0);
            for (int i = 0; i < particles.Count(); i++)
            {
                particles[i].draw();
            }
        }
    }
 
}
