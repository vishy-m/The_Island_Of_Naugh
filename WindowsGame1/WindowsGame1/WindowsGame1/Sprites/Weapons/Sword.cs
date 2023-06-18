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
    public class Sword : Weapon
    {
        public static Texture2D slash;

        public List<Texture2D> swordOpeningAnimFloor1;

        public int animTimer;
        public int timer;
        public bool animStarted;
        public int openAnimFrameRate;
        public int startingOpenAnimFrameRate;
        public int slashTimer;

        public bool isSlashing;

        public SpriteEffects flip;

        public Sword(Rectangle r) : base(r)
        {
            damage = .5f;
            animTimer = 0;
            timer = 0;
            startingOpenAnimFrameRate = 10;
            openAnimFrameRate = 5;
            slashTimer = 0;
            isSlashing = false;
            flip = SpriteEffects.None;
        }
        public override void loadContent()
        {
            
            swordOpeningAnimFloor1 = Loader.loadMultipleTextures("Sprites/Floor1/sword");
        }
        public override void update()
        {
            KeyboardState kbr = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            if (!hasBeenCollected)
            {
                if (GlobalVars.player.sword == null && GlobalVars.gState > GlobalVars.GAMESTATE.DUNGEON && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
                {
                    GlobalVars.player.setSword(new Rectangle(0, 0, 100, 100));
                    hasBeenCollected = true;
                }
            }


            checkRotation();
            if (hasBeenCollected)
            {
                isInteractible = false;
                timer++;

                if (timer > 60)
                {
                    openingAndIdleAnim(22, 27);
                    animTimer++;
                }
                if (!isSlashing && !GlobalVars.player.isFacingBack)
                {
                    if (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.ATTACK]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.ATTACK]))
                    { 
                        isSlashing = true;
                    }
                }

                if (isSlashing)
                {
                    if (GlobalVars.gState == GlobalVars.GAMESTATE.BOSS)
                    {
                        if (rect.Intersects(BossRoom.boss.rect))
                        {
                            BossRoom.boss.takeDamage(damage / 60);
                        }
                        for (int i = 0; i < BossRoom.boss.enemies.Count(); i++)
                        {
                            if (rect.Intersects(BossRoom.boss.enemies[i].rect))
                            {
                                BossRoom.boss.enemies[i].takeDamage(damage);
                            }
                        }
                    }
                    openAnimFrameRate = 1;
                    if (slashTimer < 10)
                    {
                        EnemyManager.checkEnemyLifeLost(rect);
                        animTimer = 0;
                        slashTimer++;
                        sprit = 27;
                    }

                    if (sprit == 25)
                    {
                        isSlashing = false;
                        slashTimer = 0;
                    }
                }

                if (GlobalVars.player.flip)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }

        }

        public override void draw()
        {
            Vector2 swordCenter = new Vector2(rect.Width / 2, rect.Height / 2);
            if (!hasBeenCollected)
                GlobalVars.spriteBatch.Draw(swordOpeningAnimFloor1[sprit], rect, Color.White);
            else
                GlobalVars.spriteBatch.Draw(swordOpeningAnimFloor1[sprit], rect, null, Color.White, 0, swordCenter, flip, 0);
        }

        public void openingAndIdleAnim(int minSprit, int maxSprit)
        {
            if (animTimer == 0)
            {
                sprit = 0;
            }
            if (animTimer % openAnimFrameRate == 0)
            {
                if (sprit < maxSprit)
                {
                    sprit++;
                }
                if (sprit >= maxSprit)
                {
                    sprit = minSprit;
                    openAnimFrameRate = startingOpenAnimFrameRate;
                }
            }
        }

    }
}
