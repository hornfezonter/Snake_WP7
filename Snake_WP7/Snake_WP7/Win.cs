using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

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

        public override void Update(GameTime gameTime)
        {
            TouchCollection touchs = TouchPanel.GetState();
            if (touchs.Count > 0)
            {
                ((Game1)Game).currentState = Game1.GameState.main_menu;
                ((Game1)Game).Lag = 600;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(win, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

    }
}
