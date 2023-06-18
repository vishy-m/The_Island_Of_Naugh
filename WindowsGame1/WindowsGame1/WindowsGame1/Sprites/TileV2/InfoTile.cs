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
    class InfoTile : Tile2
    {
        public InfoTile(char c, Rectangle r, string textureDir) : base(c, r, textureDir)
        {

        }

        public override void update()
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
            if (GlobalVars.player.playerRect.Intersects(rect) && !HUD.writingText)
            {

                KeyboardState kbr = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (kbr.IsKeyDown(InputMaps.keyboardInputs[InputMaps.INTERACT]) || pad.IsButtonDown(InputMaps.gamePadInputs[InputMaps.INTERACT]))
                {
                    if (GlobalVars.gState == GlobalVars.GAMESTATE.DUNGEON)
                        HUD.drawText(Dungeon.dungeonInfoText, "DungeonSprites/skull");
                }
            }
        }
    }
}
