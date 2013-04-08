using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Snake.Sprites
{
    public class Snake
    {
        #region 成员变量

        protected SnakeHead head;
        protected List<SnakeBody> body;
        protected SnakeTail tail;
        protected int msPerMove;//贪吃蛇移动一步的时间间隔
        protected int timeSinceLastMove;

        protected Texture2D headImage;
        protected Texture2D straightBodyImage;
        protected Texture2D cornerBodyImage;
        protected Texture2D tailImage;
        protected Vector2 origin;

        public Point NextPosition
        {
            get
            {
                int x, y;
                switch (head.NextDirection)
                {
                    case Direction.Up:
                        x = 0;
                        y = -1;
                        break;
                    case Direction.Down:
                        x = 0;
                        y = 1;
                        break;
                    case Direction.Left:
                        x = -1;
                        y = 0;
                        break;
                    case Direction.Right:
                        x = 1;
                        y = 0;
                        break;
                    default:
                        x = 0;
                        y = 0;
                        break;
                }
                Point result = head.Position;
                result.X += x;
                result.Y += y;
                return result;
            }
        }

        public Point TailPosition
        {
            get
            {
                return tail.Position;
            }
        }

        public Point HeadPosition
        {
            get
            {
                return head.Position;
            }
        }

        #endregion

        #region 构造函数

        public Snake(Vector2 _origin, Point position, Texture2D _head, Texture2D _straightBody, Texture2D _cornerBody, Texture2D _tail)
        {
            origin = _origin;
            headImage = _head;
            straightBodyImage = _straightBody;
            cornerBodyImage = _cornerBody;
            tailImage = _tail;

            Point pos = position;

            head = new SnakeHead(headImage, origin, pos, Direction.Up);
            body = new List<SnakeBody>();
            pos.Y++;
            body.Add(new SnakeBody(straightBodyImage, origin, pos, Direction.Up, Direction.Up));
            pos.Y++;
            tail = new SnakeTail(tailImage, origin, pos, Direction.Up);
        }

        //蛇的最短长度为3
        public Snake(Vector2 _origin, Point position, Texture2D _head, Texture2D _straightBody, Texture2D _cornerBody, Texture2D _tail, Direction _direction, int length)
        {
            origin = _origin;
            headImage = _head;
            straightBodyImage = _straightBody;
            cornerBodyImage = _cornerBody;
            tailImage = _tail;

            Point pos = position;

            if (length < 3)
                length = 3;

            int x, y;
            switch (_direction)
            {
                case Direction.Up:
                    x = 0;
                    y = -1;
                    break;
                case Direction.Down:
                    x = 0;
                    y = 1;
                    break;
                case Direction.Left:
                    x = -1;
                    y = 0;
                    break;
                case Direction.Right:
                    x = 1;
                    y = 0;
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }

            head = new SnakeHead(headImage, origin, pos, _direction);
            body = new List<SnakeBody>();
            for (int i = 0; i < length - 2; i++)
            {
                pos.X += x;
                pos.Y += y;
                body.Add(new SnakeBody(straightBodyImage,origin,pos,_direction,_direction));
            }

            pos.X += x;
            pos.Y += y;
            tail = new SnakeTail();
        }

        #endregion

        public virtual void Update(GameTime gameTime)
        {
            head.Update(gameTime);
            foreach (SnakeBody i in body)
                i.Update(gameTime);
            tail.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            head.Draw(gameTime, spriteBatch);
            tail.Draw(gameTime, spriteBatch);
            foreach (SnakeBody s in body)
                s.Draw(gameTime, spriteBatch);
        }

        public void move(bool grow)
        {
            int x, y;
            switch (head.NextDirection)
            {
                case Direction.Up:
                    x = 0;
                    y = -1;
                    break;
                case Direction.Down:
                    x = 0;
                    y = 1;
                    break;
                case Direction.Left:
                    x = -1;
                    y = 0;
                    break;
                case Direction.Right:
                    x = 1;
                    y = 0;
                    break;
                default:
                    x=0;
                    y=0;
                    break;
            }

            int length = body.Count;

            if (!grow)
            {
                tail.Position = body[length - 1].Position;
                tail.CurrentDirection = body[length - 1].CurrentDirection;

                body.RemoveAt(length - 1);
            }

            Texture2D bodyImage;
            if (head.CurrentDirection == head.NextDirection)
                bodyImage = straightBodyImage;
            else
                bodyImage = cornerBodyImage;

            body.Insert(0,new SnakeBody(bodyImage, origin, head.Position, head.NextDirection, head.CurrentDirection));

            Point p;
            p = head.Position;
            p.X += x;
            p.Y += y;
            head.Position = p;
            head.CurrentDirection = head.NextDirection;
        }

        public void turn(Direction direction)
        {
            head.turn(direction);
        }
    }
}
