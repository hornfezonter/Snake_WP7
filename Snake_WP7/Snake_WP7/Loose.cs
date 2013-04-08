using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    public class Loose:DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D loose;

        public Loose(Game game) : base(game) { }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            loose = Game.Content.Load<Texture2D>(@"images/loose");

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(loose, new Vector2(100, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
