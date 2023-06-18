using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Camera
    {
        public Matrix transform;
        public int transformX;
        public int transformY;
        public Camera()
        {
            transformX = 0;
            transformY = 0;
        }
        public void follow(Rectangle target)
        {
            int targetXCenter = -target.X - (target.Width / 2);
            int targetYCenter = -target.Y - (target.Height / 2);

            Matrix pos = Matrix.CreateTranslation(targetXCenter, targetYCenter, 0);


            Matrix offset = Matrix.CreateTranslation(Game1.screenW / 2, Game1.screenH / 2, 0);

            transform = pos * offset;

            transformX = -(int)transform.Translation.X;
            transformY = -(int)transform.Translation.Y;

        }
    }
}
