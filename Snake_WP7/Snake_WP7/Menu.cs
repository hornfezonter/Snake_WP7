using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Snake.Sprites;

using System.Diagnostics;

namespace Snake
{
    public class Menu:DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Button start;
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
            Texture2D img_start = Game.Content.Load<Texture2D>(@"images/buttons/snake1");
            Texture2D img_start_pushed = Game.Content.Load<Texture2D>(@"images/buttons/snake2");
            background = Game.Content.Load<Texture2D>(@"images/menu_background");
            start = new Button(new Vector2(250, 70), img_start, img_start_pushed);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            start.Update(gameTime);
            if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gestures = TouchPanel.ReadGesture();
                if (gestures.GestureType == GestureType.Tap)
                {
                        if (start.CheckPoint(gestures.Position))
                        {
                            ((Game1)Game).currentState = Game1.GameState.playing;
                            ((Game1)Game).Lag = 500;
                        }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            start.Draw(gameTime,spriteBatch);

            base.Draw(gameTime);
        }
    }
}
