using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Tile : Sprite
    {
        public static bool removeGates = false;
        public int sprit;
        //public Rectangle loc;
        public char type;
        public Boolean isPrisonBar;
        public Boolean isTransparent;
        int prisonTimer = 0;
        public Boolean barIsElectric;
        public Boolean isImpassable;
        public Boolean isInfoBoard;
        public Boolean isGate;
        public string infoText;
        Random rnd = new Random();
        public Tile(char type, int x, int y) : base(new Rectangle(x, y, 100, 100))
        {
            
            isPrisonBar = false;
            isTransparent = false;
            prisonTimer = 0;
            barIsElectric = false;
            isImpassable = false;
            isInfoBoard = false;
            isGate = false;
            infoText = "";
            this.type = type;
            prisonTimer = 0;
            //loc = new Rectangle(x, y, 100, 100);
            switch (type)
            {
                case '_':
                    sprit = 1;
                    break;
                case 'a':
                    sprit = 2;
                    break;
                case 'u':
                    sprit = 0;
                    break;
                case 'l':
                    sprit = 0;
                    break;
                case 'r':
                    sprit = 0;
                    break;
                case 'd':
                    sprit = 0;
                    break;
                case 'p':
                    sprit = 0;
                    isPrisonBar = true;
                    break;
                case '.':
                    sprit = 0;
                    isTransparent = true;
                    break;
                case 'b':
                    sprit = 0;
                    isTransparent = true;
                    DungeonKey.makeBounds(rect);
                    break;
                case 'I':
                    sprit = 0;
                    isImpassable = true;
                    break;
                case 't':
                    sprit = 3;
                    isInfoBoard = true;
                    break;
                case 'g':
                    sprit = 4;
                    isGate = true;
                    isImpassable = true;
                    break;
                case 'G':
                    sprit = 5;
                    isGate = true;
                    isImpassable = true;
                    break;
                case 's':
                    sprit = 0;
                    GlobalVars.player.setSword(new Rectangle(x, y, 100, 100));
                    isTransparent = true;
                    break;
                case 'E':
                    sprit = 0;
                    isTransparent = true;
                    //EnemyManager.makeEnemyBounds(rect, type, );
                    break;
                case 'x':
                    sprit = 0;
                    isTransparent = true;
                    GlobalVars.player.playerRect.X = x;
                    GlobalVars.player.playerRect.Y = y;
                    break;
                case 'n':
                    isTransparent = true;
                    sprit = 0;
                    break;

            }
            
        }
        public override void update()
        {

            if (isPrisonBar)
            {
                prisonTimer++;

                if (sprit == 0)
                {
                    barIsElectric = false;
                    if (prisonTimer > 60)
                    {
                        sprit++;
                        prisonTimer = 0;
                        
                    }
                }
                if (prisonTimer > 15 && sprit != 0)
                {
                    sprit++;
                    barIsElectric = true;
                    if (sprit > 3)
                    {
                        sprit = 0;
                    }
                    prisonTimer = 0;
                }
            }
            if (isImpassable)
            {
                int playerRectXCenter = GlobalVars.player.playerRect.X + (GlobalVars.player.playerRect.Width / 2);
                int playerRectYCenter = GlobalVars.player.playerRect.Y + (GlobalVars.player.playerRect.Height / 2);
                //Console.WriteLine("X Center: " + playerRectXCenter + " Y Center: " + playerRectYCenter);
                //Console.WriteLine("Loc X: " + loc.X + " Loc Y: " + loc.Y + "\n\n");

                if (playerRectXCenter < rect.X && playerRectYCenter > rect.Y && playerRectYCenter < rect.Y + rect.Height - 10)
                {
                    type = 'r';
                }
                else if (playerRectXCenter > rect.X + rect.Width && playerRectYCenter > rect.Y && playerRectYCenter < rect.Y + rect.Height - 10)
                {
                    type = 'l';
                }
                else if (playerRectYCenter > rect.Y + rect.Height && playerRectXCenter > rect.X && playerRectXCenter < rect.X + rect.Width - 10)
                {
                    type = 'u';
                }
                else if (playerRectYCenter < rect.Y && playerRectXCenter > rect.X && playerRectXCenter < rect.X + rect.Width - 10)
                {
                    type = 'd';
                }

            }
            if (isInfoBoard)
            {
                if (!HUD.writingText && !DungeonKey.correctKeyCollected)
                {
                    isInteractible = true;
                    interactionKey = "E";
                }
                else
                {
                    isInteractible = false;
                    interactionKey = "";
                }
                if (GlobalVars.player.playerRect.Intersects(rect) && !HUD.writingText && !DungeonKey.doesKeyIntersectPlayerOrRect(rect) && !DungeonKey.correctKeyCollected)
                {
                    
                    KeyboardState kbr = Keyboard.GetState();
                    if (kbr.IsKeyDown(Keys.E))
                    {
                        if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON)
                            HUD.drawText(Game1.dungeonInfoText);
                    }
                }
            }
            if (isGate)
            {
                //Console.WriteLine(DungeonKey.correctKeyCollected);
                //Console.WriteLine(isImpassable + " " + sprit + " " + isTransparent);
                if (!DungeonKey.correctKeyCollected)
                {
                    isImpassable = true;
                }
                else 
                {
                    if (rect.Intersects(GlobalVars.player.playerRect))
                    {
                        removeGates = true;
                    }
                    
                }

                if (removeGates)
                {
                    isImpassable = false;
                    type = '.';
                    sprit = 1;
                }
                else
                {
                    isImpassable = true;
                    //sprit = 1;
                }
            } 
        }
        public override void loadContent()
        {
            throw new NotImplementedException();
        }
        public override void draw()
        {
            throw new NotImplementedException();
        }

        public static Tile[,] tileLoad(String directory)
        {
            Tile[,] ret = null;
            using(StreamReader fileReader = new StreamReader(directory))
            {
                int l = 0;
                int w = 0;
                for (int i = 0; !fileReader.EndOfStream; i++)
                {
                    String line = fileReader.ReadLine();
                    if (i == 0)
                    {
                        string[] dimensions = line.Split(' ');
                        l = Convert.ToInt32(dimensions[0]);
                        w = Convert.ToInt32(dimensions[1]);
                        ret = new Tile[l, w];
                        continue;
                    }
                    char[] temp = line.ToCharArray();
                    if (i - 1 < l)
                    {
                        int y = (i - 1) * 100;
                        for (int j = 0; j < temp.Length; j++)
                        {
                            int x = j * 100;
                            ret[i - 1, j] = new Tile(temp[j], x, y);
                            //Console.WriteLine("x: " + x + " y: " + y);
                        }
                            
                    }
                } 
            }
            
            return ret;
        }
        

    }
}