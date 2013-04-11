using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    public class Lose:DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D lose;

        public Lose(Game game) : base(game) { }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            lose = Game.Content.Load<Texture2D>(@"images/lose");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            TouchCollection touchs = TouchPanel.GetState();
            if (touchs.Count > 0)
            {
                ((Game1)Game).currentState = Game1.GameState.main_menu;
                ((Game1)Game).Lag = 300;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(lose, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
