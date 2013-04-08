using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Snake.Sprites
{
    public class Button
    {
        protected Texture2D img;
        protected Texture2D img_pushed;
        protected bool pushed;
        protected Vector2 position;
        protected int width;
        protected int height;

        public Button() 
        {
            pushed = false;
        }

        public Button(Vector2 _position, Texture2D _img, Texture2D _img_pushed):base()
        {
            position = _position;
            img = _img;
            img_pushed = _img_pushed;
            width = _img.Width;
            height = _img.Height;
        }

        public bool resize(int _width, int _height)
        {
            if (_width > 0 && _height > 0)
            {
                width = _width;
                height = _height;
                return true;
            }
            else
                return false;           
        }

        public void setPosition(Vector2 _pos)
        {
            position = _pos;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle rec = new Rectangle((int)position.X, (int)position.Y, width, height);
            spriteBatch.Begin();
            if (pushed)
            {
                spriteBatch.Draw(img_pushed, rec, Color.White);
            }
            else
            {
                spriteBatch.Draw(img, rec, Color.White);
            }
            spriteBatch.End();
        }

        public bool CheckPoint(Vector2 point)
        {
            if (point.X > position.X && point.X < point.X + width && point.Y > position.Y && point.Y < point.Y + height)
            {
                pushed = true;
                return true;
            }
            else
            {
                pushed = false;
                return false;
            }
        }  
    }
}
