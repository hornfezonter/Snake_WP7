using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Snake.Sprites
{
    public class Stone:Tile
    {
        public Stone() : base() { }

        public Stone(Texture2D _img, Vector2 _origin, Point _position) : base(_img, _origin, _position) { }

        public Stone(Texture2D _img, Vector2 _origin, Point _position, Point _frameSize, Point _sheetSize) : base(_img, _origin, _position, _frameSize, _sheetSize) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
