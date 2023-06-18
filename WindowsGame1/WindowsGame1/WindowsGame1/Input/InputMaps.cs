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
    public static class InputMaps
    {
        public const int UP = 0;
        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int DOWN = 3;
        public const int INTERACT = 4;
        public const int ATTACK = 5;
        public const int SPEED_DIALOGUE = 6;
        public const int NEXT_DIALOGUE = 7;
        public const int DASH = 8;

        public static Dictionary<int, Buttons> gamePadInputs = new Dictionary<int, Buttons>()
        {
            {INTERACT, Buttons.X},
            {ATTACK, Buttons.RightTrigger},
            {SPEED_DIALOGUE, Buttons.DPadUp},
            {NEXT_DIALOGUE, Buttons.DPadLeft},
            {DASH, Buttons.A}
        };

        public static Dictionary<int, Keys> keyboardInputs = new Dictionary<int, Keys>()
        {
            {UP, Keys.W},
            {LEFT, Keys.A},
            {RIGHT, Keys.D},
            {DOWN, Keys.S},
            {INTERACT, Keys.E},
            {ATTACK, Keys.Space},
            {SPEED_DIALOGUE, Keys.Space},
            {NEXT_DIALOGUE, Keys.E},
            {DASH, Keys.Q}
        };

        public static string getInputInString(int inputKey, GamePadState pad)
        {
            if (gamePadInputs.ContainsKey(inputKey))
            {
                return gamePadInputs[inputKey].ToString();
            }
            return "NO KEY";
        }
        public static string getInputInString(int inputKey, KeyboardState kb)
        {
            if (keyboardInputs.ContainsKey(inputKey))
            {
                return keyboardInputs[inputKey].ToString();
            }
            return "NO KEY";
        }

    }
}
