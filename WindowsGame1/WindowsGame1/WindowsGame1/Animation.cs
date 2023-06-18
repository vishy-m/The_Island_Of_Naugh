using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsGame1
{
    class Animation
    {
        public string AnimationName { get; private set; }
        public int TextureId { get; private set; }
        public int FrameToDraw { get { return frameList[currentFrame]; } }
        public delegate void AnimationEndCallBack();
        private AnimationEndCallBack callBack;
        private List<int> frameList;
        private float totalElapsed;
        private float timePerFrame;
        private int currentFrame;
        private int frameCount;
        private int framesPerSec;
        private bool isPlaying;
        private bool loopAnimation;

        public void LoadAnimation(string _animationName, int _textureId, List<int> _frameList, int _framesPerSec, bool _loop)
        {
            AnimationName = _animationName;
            TextureId = _textureId;
            frameList = _frameList;
            frameCount = frameList.Count;
            framesPerSec = _framesPerSec;
            timePerFrame = (float)1 / framesPerSec;
            currentFrame = 0;
            totalElapsed = 0;
            loopAnimation = _loop;
        }

        public void play()
        {
            isPlaying = true;
        }
        public void ResetPlay()
        {
            currentFrame = 0;
            totalElapsed = 0;
            isPlaying = true;
        }
        public void Stop()
        {
            isPlaying = false;
        }

        public void AnimationCallBack(AnimationEndCallBack _callBack)
        {
            callBack = _callBack;
        }
        public void Update(GameTime _gameTime)
        {
            if (!isPlaying) return;
            {
                totalElapsed += (float)_gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (totalElapsed <= timePerFrame) return;
            {
                currentFrame++;
                currentFrame = currentFrame % frameCount;
                totalElapsed -= timePerFrame;
            }
            if (!loopAnimation && currentFrame == 0)
            {
                isPlaying = false;
                if (callBack != null)
                    callBack();
            }
        }
    }
}

