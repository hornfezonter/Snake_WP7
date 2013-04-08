using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Snake.Sprites
{
    public abstract class Tile
    {
        #region 成员变量

        protected Texture2D img;
        protected Point position;
        protected Vector2 origin;
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        protected int timeSinceLastFrame;
        protected int timePerFrame;
        protected int displayWidth;
        protected int displayHeight;

        public Point Position
        {
            set
            {
                position = value;
            }
            get
            {
                return position;
            }
        }

        #endregion

        public Tile() 
        {
            position = new Point(0,0);
            origin = new Vector2(0,0);
            frameSize = new Point(1,1);
            currentFrame = new Point(0,0);
            sheetSize = new Point(1,1);
            timePerFrame = 16;
            timeSinceLastFrame = 16;
        }

        public Tile(Texture2D _img, Vector2 _origin, Point _position)
            : this()
        {
            img = _img;
            origin = _origin;
            position = _position;
            displayHeight = _img.Height;
            displayWidth = _img.Width;
        }

        public Tile(Texture2D _img, Vector2 _origin, Point _position, Point _frameSize, Point _sheetSize)
            : this(_img, _origin, _position)
        {
            frameSize = _frameSize;
            sheetSize = _sheetSize;
        }

        public void changeImage(Texture2D _img, Point _frameSize, Point _sheetSize)
        {
            img = _img;
            frameSize = _frameSize;
            sheetSize = _sheetSize;
            currentFrame.X = 0;
            currentFrame.Y = 0;
        }

        public virtual  void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > timePerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(img, new Rectangle((int)(origin.X + position.X * displayWidth), (int)(origin.Y + position.Y * displayHeight), displayWidth, displayHeight),null, Color.White, 0, new Vector2((float)(displayWidth*0.5), (float)(displayHeight*0.5)), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
