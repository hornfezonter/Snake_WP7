using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class Win:DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        private Texture2D win;

        public Win(Game game) : base(game) { }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            win = Game.Content.Load<Texture2D>(@"images/win");

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(win, new Vector2(100, 100), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

    }
}
