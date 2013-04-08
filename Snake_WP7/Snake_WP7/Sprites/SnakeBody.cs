using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Sprites
{
    public class SnakeBody:SnakeSegment
    {
        protected Direction preDirection;

        public SnakeBody() : base(){ }

        public SnakeBody(Texture2D _img, Vector2 _origin, Point _position, Direction _direction, Direction _preDirection) : base(_img, _origin, _position,_direction)
        {
            preDirection = _preDirection;
        }

        public SnakeBody(Texture2D _img, Vector2 _origin, Point _position, Direction _direction, Direction _preDirection, Point _frameSize, Point _sheetSize) : base(_img, _origin, _position, _direction, _frameSize, _sheetSize)
        {
            preDirection = _preDirection;
        }

        public Direction PreDirection
        {
            get
            {
                return preDirection;
            }
            set
            {
                preDirection = value;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (preDirection == direction)
            {
                base.Draw(gameTime, spriteBatch);
            }
            else
            {
                Rectangle destRec = new Rectangle((int)(origin.X + position.X * displayWidth), (int)(origin.Y + position.Y * displayHeight), displayWidth, displayHeight);
                Vector2 rotationOrigin = new Vector2((float)(displayWidth * 0.5), (float)(displayHeight * 0.5));
                float rotation = 0;
                SpriteEffects effect = SpriteEffects.None;

                switch (direction)
                {
                    case Direction.Up:
                        rotation = 0;
                        if (preDirection == Direction.Left)
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }
                        else if (preDirection == Direction.Right)
                        {
                            effect = SpriteEffects.None;
                        }
                        break;
                    case Direction.Right:
                        rotation = (float)(Math.PI * 0.5);
                        if (preDirection == Direction.Down)
                        {
                            effect = SpriteEffects.None;
                        }
                        else if (preDirection == Direction.Up)
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }
                        break;
                    case Direction.Down:
                        rotation = (float)Math.PI;
                        if (preDirection == Direction.Left)
                        {
                            effect = SpriteEffects.None;
                        }
                        else if (preDirection == Direction.Right)
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }
                        break;
                    case Direction.Left:
                        rotation = (float)(Math.PI * 1.5);
                        if (preDirection == Direction.Down)
                        {
                            effect = SpriteEffects.FlipHorizontally;
                        }
                        else if (preDirection == Direction.Up)
                        {
                            effect = SpriteEffects.None;
                        }
                        break;
                    default:
                        rotation = 0;
                        effect = SpriteEffects.None;
                        break;
                }

                spriteBatch.Begin();
                spriteBatch.Draw(img, destRec, null, Color.White, rotation, rotationOrigin, effect, 0.5f);
                spriteBatch.End();
            }
        }
    }
}
