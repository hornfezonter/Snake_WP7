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
    public enum Contain
    {
        food,
        snake,
        stone,
        empty
    }

    public class Scene:DrawableGameComponent
    {
        #region 成员变量

        protected int finish;//0为进行中，1为失败，2为胜利

        protected SpriteBatch spriteBatch;
        protected Texture2D img_background;
        protected Texture2D img_food;
        protected Texture2D img_head;
        protected Texture2D img_straightBody;
        protected Texture2D img_cornerBody;
        protected Texture2D img_tail;

        protected Contain[,] contains;
        protected bool[,] growPoints;
        protected Snake.Sprites.Snake player;
        protected Snake.Sprites.Snake AI;
        protected Vector2 origin;
        protected List<Stone> stones;
        protected List<Food> foods;
        protected bool food_eaten;

        protected Random rand;

        protected int timePerMove;
        protected int timeSinceLastMove;
        protected int sceneWidth;
        protected int sceneHeight;

        #endregion

        public Scene(Game game)
            : base(game)
        {
            origin = new Vector2(12, 12);
            stones = new List<Stone>();
            foods = new List<Food>();
            timePerMove = 300;
            timeSinceLastMove = 0;
            rand = new Random();

            sceneHeight = 25;
            sceneWidth = 20;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            img_head = Game.Content.Load<Texture2D>(@"images/head");
            img_straightBody = Game.Content.Load<Texture2D>(@"images/straightBody");
            img_cornerBody = Game.Content.Load<Texture2D>(@"images/cornerBody");
            img_tail = Game.Content.Load<Texture2D>(@"images/tail");
            img_food = Game.Content.Load<Texture2D>(@"images/objects/meat");
            Texture2D img_wall = Game.Content.Load<Texture2D>(@"images/objects/wall");

            contains = new Contain[sceneWidth, sceneHeight];
            growPoints = new bool[sceneWidth, sceneHeight];
            food_eaten = true;
            for (int i = 0; i < sceneWidth; i++)
                for (int j = 0; j < sceneHeight; j++)
                {
                    contains[i, j] = Contain.empty;
                    growPoints[i, j] = false;
                }

            for (int i = 0; i < sceneWidth; i++)
            {
                Stone stone = new Stone(img_wall, origin, new Point(i, 0));
                stones.Add(stone);
                contains[i, 0] = Contain.stone;
            }
            for (int i = 0; i < sceneWidth; i++)
            {
                Stone stone = new Stone(img_wall, origin, new Point(i, sceneHeight-1));
                stones.Add(stone);
                contains[i, sceneWidth - 1] = Contain.stone;
            }
            for (int i = 1; i < sceneHeight - 1; i++)
            {
                Stone stone = new Stone(img_wall, origin, new Point(0, i));
                stones.Add(stone);
                contains[0,i] = Contain.stone;
            }
            for (int i = 1; i < sceneHeight; i++)
            {
                Stone stone = new Stone(img_wall, origin, new Point(sceneWidth - 1, i));
                stones.Add(stone);
                contains[sceneWidth - 1, i] = Contain.stone;
            }

            player = new Snake.Sprites.Snake(origin, new Point(3, 3), img_head, img_straightBody, img_cornerBody, img_tail);
            contains[3, 3] = Contain.snake;
            contains[3, 4] = Contain.snake;
            contains[3, 5] = Contain.snake;

            img_head = Game.Content.Load<Texture2D>(@"images/AIhead");
            img_straightBody = Game.Content.Load<Texture2D>(@"images/AIstraightBody");
            img_cornerBody = Game.Content.Load<Texture2D>(@"images/AIcornerBody");
            img_tail = Game.Content.Load<Texture2D>(@"images/AItail");

            AI = new Snake.Sprites.Snake(origin, new Point(10, 10), img_head, img_straightBody, img_cornerBody, img_tail);
            contains[10, 10] = Contain.snake;
            contains[10, 11] = Contain.snake;
            contains[10, 12] = Contain.snake;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            foreach (Stone i in stones)
                i.Update(gameTime);
            foreach (Food f in foods)
                f.Update(gameTime);

            timeSinceLastMove += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastMove >= timePerMove)
            {
                timeSinceLastMove = 0;

                #region AI_move

                bool AIgrow = false;

                Point AItailPosition = AI.TailPosition;
                if (growPoints[AItailPosition.X, AItailPosition.Y])
                {
                    AIgrow = true;
                    growPoints[AItailPosition.X, AItailPosition.Y] = false;
                }

                Point AInextPosition = AI.NextPosition;
                if (contains[AInextPosition.X, AInextPosition.Y] == Contain.empty)
                {
                    AI.move(AIgrow);
                    contains[AInextPosition.X, AInextPosition.Y] = Contain.snake;
                    if (!AIgrow)
                    {
                        contains[AItailPosition.X, AItailPosition.Y] = Contain.empty;
                    }
                }
                else if (contains[AInextPosition.X, AInextPosition.Y] == Contain.food)
                {
                    AI.move(AIgrow);
                    contains[AInextPosition.X, AInextPosition.Y] = Contain.snake;
                    if (!AIgrow)
                    {
                        contains[AItailPosition.X, AItailPosition.Y] = Contain.empty;
                    }
                    growPoints[AInextPosition.X, AInextPosition.Y] = true;
                    foods.RemoveAt(0);
                    food_eaten = true;
                }
                else
                {
                    ((Game1)Game).currentState = Game1.GameState.win;
                }

                #endregion

                #region player_move

                bool grow = false;

                Point tailPosition = player.TailPosition;
                if (growPoints[tailPosition.X, tailPosition.Y])
                {
                    grow = true;
                    growPoints[tailPosition.X, tailPosition.Y] = false;
                }

                Point nextPosition = player.NextPosition;
                if (contains[nextPosition.X, nextPosition.Y] == Contain.empty)
                {
                    player.move(grow);
                    contains[nextPosition.X, nextPosition.Y] = Contain.snake;
                    if (!grow)
                    {
                        contains[tailPosition.X, tailPosition.Y] = Contain.empty;
                    }
                }
                else if (contains[nextPosition.X, nextPosition.Y] == Contain.food)
                {
                    player.move(grow);
                    contains[nextPosition.X, nextPosition.Y] = Contain.snake;
                    if (!grow)
                    {
                        contains[tailPosition.X, tailPosition.Y] = Contain.empty;
                    }
                    growPoints[nextPosition.X, nextPosition.Y] = true;
                    foods.RemoveAt(0);
                    food_eaten = true;
                }
                else
                {
                    //((Game1)Game).currentState = Game1.GameState.loose;
                }
                #endregion

            }

            #region generate food

            if (food_eaten)
            {
                for (; ; )
                {
                    int roll = rand.Next(0, 400);

                    if (contains[roll%sceneWidth, roll/sceneWidth] == Contain.empty)
                    {
                        contains[roll%sceneWidth, roll/sceneWidth] = Contain.food;
                        foods.Add(new Food(img_food, origin, new Point(roll%sceneWidth, roll/sceneWidth)));
                        food_eaten = false;
                        break;
                    }                 
                }
            }
            #endregion

            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.Up))
                player.turn(Direction.Up);
            else if (key.IsKeyDown(Keys.S) || key.IsKeyDown(Keys.Down))
                player.turn(Direction.Down);
            else if (key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.Left))
                player.turn(Direction.Left);
            else if (key.IsKeyDown(Keys.D) || key.IsKeyDown(Keys.Right))
                player.turn(Direction.Right);

            Point AIHead = AI.HeadPosition;
            if (contains[AIHead.X - 1, AIHead.Y] == Contain.empty || contains[AIHead.X - 1, AIHead.Y] == Contain.food)
            {
                AI.turn(Direction.Left);
            }
            else if (contains[AIHead.X + 1, AIHead.Y] == Contain.empty || contains[AIHead.X + 1, AIHead.Y] == Contain.food)
            {
                AI.turn(Direction.Right);
            }
            else if (contains[AIHead.X, AIHead.Y - 1] == Contain.empty || contains[AIHead.X - 1, AIHead.Y - 1] == Contain.food)
            {
                AI.turn(Direction.Up);
            }
            else if (contains[AIHead.X, AIHead.Y + 1] == Contain.empty || contains[AIHead.X - 1, AIHead.Y + 1] == Contain.food)
            {
                AI.turn(Direction.Down);
            }
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            player.Draw(gameTime, spriteBatch);
            AI.Draw(gameTime, spriteBatch);
            foreach (Stone s in stones)
            {
                s.Draw(gameTime, spriteBatch);
            }

            foreach (Food f in foods)
            {
                f.Draw(gameTime, spriteBatch);
            }
            
            base.Draw(gameTime);
        }

    }
}
