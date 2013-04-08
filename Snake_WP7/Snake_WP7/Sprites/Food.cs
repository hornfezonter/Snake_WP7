using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Sprites
{
    public class Food:Tile
    {
        public Food() : base() { }

        public Food(Texture2D _img, Vector2 _origin, Point _position) : base(_img, _origin, _position) { }

        public Food(Texture2D _img, Vector2 _origin, Point _position, Point _frameSize, Point _sheetSize) : base(_img, _origin, _position, _frameSize, _sheetSize) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
