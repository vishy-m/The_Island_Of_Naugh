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
    public class HUD
    {
        public static Rectangle textRect;
        public static SpriteFont font;
        public static SpriteBatch batch;
        public static void setSpriteBatch(SpriteBatch b) { batch = b; }

        public static Texture2D textBackgroundText;


        public static int textTimer = 0;
        public static Boolean writingText;
        public static String currentStr;
        public static int textIndex = 0;
        public static List<Vector2> textPoss = new List<Vector2>();
        public static Vector2 textPos;
        public static List<string> lines = new List<string>();
        public static int lineIndex = 0;
        public static bool drawPressE = false;
        public static bool waitForEPress = false;
        public static int startingWordSpeed = 10;
        public static int wordSpeed = startingWordSpeed;

        public static Texture2D texture = null;
        public static Rectangle tRect = new Rectangle(0, 0, 0, 0);

        public static int delayTimer = 0;

        public static int displacementX = 50;
        public static int displacementY = 50;

        public static void drawText(String s)
        {
            if (!writingText && delayTimer == 0 && GlobalVars.retTimer == 0)
            {
                writingText = true;
                currentStr = s;
                lines.Add("");
                textPos = new Vector2(textRect.X + 50, textRect.Y + 50);
                textPoss.Add(textPos);
                delayTimer++;
            }
        }

        public static void drawText(String s, string dir)
        {
            if (!writingText && delayTimer == 0 && GlobalVars.retTimer == 0)
            {
                try
                {
                    texture = GlobalVars.Content.Load<Texture2D>(dir);
                }
                catch (Exception e)
                {
                    texture = GlobalVars.Content.Load<Texture2D>("Blue Husk");
                }
                writingText = true;
                currentStr = s;
                lines.Add("");
                textPos = new Vector2(textRect.X + 50, textRect.Y + 50);
                textPoss.Add(textPos);
                delayTimer++;
                displacementX = 15;
            }
        }

        public static void update()
        {
            if (writingText)
            {
                makeText();
                GlobalVars.player.canMove = false;
            }
            else
            {
                GlobalVars.player.canMove = true;
            }
            if (delayTimer >= 2)
            {
                delayTimer++;
                if (delayTimer > 60)
                {
                    delayTimer = 0;
                }
            }
        }

        public static void makeText()
        {
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            int x = GlobalVars.camera.transformX;
            int y = GlobalVars.camera.transformY + (Game1.screenH - 200);
            int w = Game1.screenW;
            int h = (GlobalVars.camera.transformY + Game1.screenH) - y;
            textRect = new Rectangle(x, y, w, h);
            if (texture != null)
            {
                tRect.X = textRect.X + displacementX;
                tRect.Y = textRect.Y + displacementY / 2;
                tRect.Width = textRect.Height - displacementY;
                tRect.Height = textRect.Height - displacementY;
            }
            for (int i = 0; i < textPoss.Count(); i++)
            {
                int d = tRect.Width;
                if (texture != null)
                {
                    d = (tRect.X - textRect.X) + tRect.Width;
                }
                Vector2 textp = new Vector2(textRect.X + displacementX + d, textRect.Y + displacementY + (30 * i));
                textPoss[i] = textp;
            }

            KeyboardState kbr = Keyboard.GetState();
            if (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.SPEED_DIALOGUE]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.SPEED_DIALOGUE]))
            {
                wordSpeed = 1;
            }
            else
            {
                wordSpeed = startingWordSpeed;
            }
            textTimer++;
            string emptyText = lines[lineIndex];
            //Console.WriteLine(emptyText + " " + (int)font.MeasureString(emptyText).X + " " + lineIndex + " " + textIndex);
            //Console.WriteLine(font.MeasureString(emptyText).X + " " + textRect.Width);
            if (textIndex != currentStr.Length)
            { 
                if (textTimer % wordSpeed == 0 && !waitForEPress)
                {
                    drawPressE = false;
                    if (textIndex != currentStr.Length && currentStr[textIndex] != '#')
                    {
                        emptyText += currentStr[textIndex];
                    }

                    lines[lineIndex] = emptyText;

                    checkAddLine(emptyText);                    

                    if (textIndex < currentStr.Length)
                    {
                        textIndex++;
                    }
                }

            }
            else
            {
                drawPressE = true;
                if (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.NEXT_DIALOGUE]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.NEXT_DIALOGUE]))
                {
                    textIndex = 0;
                    textTimer = 0;
                    writingText = false;
                    delayTimer++;
                    lines = new List<string>();
                    drawPressE = false;
                    lineIndex = 0;
                    texture = null;
                    tRect = new Rectangle(0, 0, 0, 0);
                    displacementX = 50;
                }
            }

            if (waitForEPress)
            {
                drawPressE = true;
                if (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.NEXT_DIALOGUE]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.NEXT_DIALOGUE]))
                {
                    lines = new List<string>();
                    lineIndex = 0;
                    textTimer = 0;
                    lines.Add("");
                    textPoss = new List<Vector2>();
                    textPos = new Vector2(textRect.X + 50, textRect.Y + 50);
                    textPoss.Add(textPos);
                    waitForEPress = false;
                }
            }
        }

        public static void checkAddLine(String s)
        {
            int d = tRect.Width + 50;
            if (texture != null)
            {
                d = (tRect.X - textRect.X) + tRect.Width;
            }
            if (currentStr[textIndex] == '#')
            {
                waitForEPress = true;
            }
            if ((int)font.MeasureString(s).X >= textRect.Width - 75 - d)
            {
                textPos.Y += 30;
                textPoss.Add(textPos);
                lines.Add("");
                lineIndex++;
            }

        }
        
        public static void drawHUD()
        {

            if (writingText)
            {
                GlobalVars.spriteBatch.Draw(HUD.textBackgroundText, HUD.textRect, Color.White);
                for (int i = 0; i < HUD.lines.Count(); i++)
                {
                    GlobalVars.spriteBatch.DrawString(HUD.font, HUD.lines[i], HUD.textPoss[i], Color.White);
                }
                if (texture != null)
                {
                    GlobalVars.spriteBatch.Draw(texture, tRect, Color.White);
                }
                if (!drawPressE)
                    drawSpeedUp();
            }
            if (drawPressE)
            {
                drawNextPress();
            }
        }

        public static void initializeTextVars(SpriteFont f, Rectangle textRect, Texture2D t)
        {
            font = f;
            HUD.textRect = textRect;
            textBackgroundText = t;
        }

        public static void drawSpeedUp()
        {
            string interactPrompt = "Press " + InputMaps.getInputInString(InputMaps.SPEED_DIALOGUE, Keyboard.GetState()) + " To Speed Up";
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                interactPrompt = "Press " + InputMaps.getInputInString(InputMaps.SPEED_DIALOGUE, GamePad.GetState(PlayerIndex.One)) + " To Speed Up";
            }
            int stringLenX = (int)HUD.font.MeasureString(interactPrompt).X;
            int stringLenY = (int)HUD.font.MeasureString(interactPrompt).Y;
            GlobalVars.spriteBatch.DrawString(font, interactPrompt, new Vector2((textRect.X + textRect.Width) - stringLenX - 50, (textRect.Y + textRect.Height) - stringLenY - 25), Color.White);
        }

        public static void drawNextPress()
        {
            string interactPrompt = "Press " + InputMaps.getInputInString(InputMaps.INTERACT, Keyboard.GetState());
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                interactPrompt = "Press " + InputMaps.getInputInString(InputMaps.INTERACT, GamePad.GetState(PlayerIndex.One));
            }
            int stringLenX = (int)HUD.font.MeasureString(interactPrompt).X;
            int stringLenY = (int)HUD.font.MeasureString(interactPrompt).Y;
            GlobalVars.spriteBatch.DrawString(font, interactPrompt, new Vector2((textRect.X + textRect.Width) - stringLenX - 50, (textRect.Y + textRect.Height) - stringLenY - 25), Color.White);
        }

        public static void reset()
        {

            textTimer = 0;
            textIndex = 0;
            textPoss = new List<Vector2>();
            lines = new List<string>();
            lineIndex = 0;
            drawPressE = false;
            waitForEPress = false;
            startingWordSpeed = 10;
            wordSpeed = startingWordSpeed;

            Texture2D texture = null;
            Rectangle tRect = new Rectangle(0, 0, 0, 0);

            delayTimer = 0;

            displacementX = 50;
            displacementY = 50;
        }
    }
}
