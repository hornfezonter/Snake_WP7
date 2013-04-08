using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Snake.Sprites
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public class SnakeSegment:Tile
    {
        protected Direction direction;

        public SnakeSegment() : base()
        {
            direction = Direction.Up;
        }

        public SnakeSegment(Texture2D _img, Vector2 _origin, Point _position, Direction _direction) : base(_img, _origin, _position) {
            direction = _direction;
        }

        public SnakeSegment(Texture2D _img, Vector2 _origin, Point _position, Direction _direction, Point _frameSize, Point _sheetSize) : base(_img, _origin, _position, _frameSize, _sheetSize) {
            direction = _direction;
        }

        public Direction CurrentDirection
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle destRec = new Rectangle((int)(origin.X+position.X*displayWidth), (int)(origin.Y+position.Y*displayHeight), displayWidth,displayHeight);
            Vector2 rotationOrigin = new Vector2((float)(displayWidth * 0.5), (float)(displayHeight * 0.5));
            float rotation;

            switch (direction)
            {
                case Direction.Up:
                    rotation = 0;
                    break;
                case Direction.Right:
                    rotation = (float)(Math.PI * 0.5);
                    break;
                case Direction.Down:
                    rotation = (float)Math.PI;
                    break;
                case Direction.Left:
                    rotation = (float)(Math.PI * 1.5);
                    break;
                default:
                    rotation = 0;
                    break;
            }

            spriteBatch.Begin();
            spriteBatch.Draw(img, destRec, null, Color.White, rotation, rotationOrigin, SpriteEffects.None, 0);
            //spriteBatch.Draw(img, new Vector2(origin.X + position.X * displayWidth, origin.Y + position.Y * displayHeight), null, Color.White, rotation, rotationOrigin, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
