using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Sprites
{
    public class SnakeTail:SnakeSegment
    {
        public SnakeTail() : base(){}

        public SnakeTail(Texture2D _img, Vector2 _origin, Point _position, Direction _direction) : base(_img, _origin, _position, _direction) {}

        public SnakeTail(Texture2D _img, Vector2 _origin, Point _position, Direction _direction, Point _frameSize, Point _sheetSize) : base(_img, _origin, _position, _direction, _frameSize, _sheetSize) {}


    }
}
