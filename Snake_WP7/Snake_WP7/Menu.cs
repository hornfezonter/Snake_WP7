using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Sprites;

using System.Diagnostics;

namespace Snake
{
    public class Menu:DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Button start;
        private Button exit;
        private Texture2D background;

        public Menu(Game game) : base(game) { }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //设置按钮的位置和图片
            Texture2D img_start = Game.Content.Load<Texture2D>(@"images/buttons/start1");
            Texture2D img_start_pushed = Game.Content.Load<Texture2D>(@"images/buttons/start2");
            start = new Button(new Vector2(100, 100), img_start, img_start_pushed);
            Texture2D img_exit = Game.Content.Load<Texture2D>(@"images/buttons/exit1");
            Texture2D img_exit_pushed = Game.Content.Load<Texture2D>(@"images/buttons/exit2");
            exit = new Button(new Vector2(100, 500), img_exit, img_exit_pushed);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            Vector2 mousePoint = new Vector2(state.X, state.Y);

            start.Update(gameTime);
            exit.Update(gameTime);

            if (state.LeftButton == ButtonState.Pressed)
            {
                if (start.CheckPoint(mousePoint))
                {
                    ((Game1)Game).currentState = Game1.GameState.playing;
                }

                if (exit.CheckPoint(mousePoint))
                    ((Game1)Game).Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            start.Draw(gameTime,spriteBatch);
            exit.Draw(gameTime,spriteBatch);

            base.Draw(gameTime);
        }
    }
}
