using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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

        //保存的图片
        protected SpriteBatch spriteBatch;
        protected Texture2D img_background;
        protected Texture2D img_food;
        protected Texture2D img_head;
        protected Texture2D img_straightBody;
        protected Texture2D img_cornerBody;
        protected Texture2D img_tail;

        //游戏进行时使用变量
        protected Contain[,] contains;
        protected bool[,] growPoints;
        protected Snake.Sprites.Snake player;
        protected Snake.Sprites.Snake AI;
        protected Vector2 origin;
        protected List<Stone> stones;
        protected List<Food> foods;
        protected bool food_eaten;
        protected Point food_position;

        protected Random rand;

        //游戏可调整参数
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
            food_position = new Point(1, 1);
            food_eaten = true;
            timePerMove = 300;
            timeSinceLastMove = 0;
            rand = new Random();

            sceneHeight = 20;
            sceneWidth = 27;
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
            img_background = Game.Content.Load<Texture2D>(@"images/background");
            Texture2D img_wall = Game.Content.Load<Texture2D>(@"images/objects/wall");

            #region 填充场景

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
                contains[i, sceneHeight - 1] = Contain.stone;
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

            #endregion

            #region 初始化蛇的位置
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
            #endregion

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
                    //((Game1)Game).currentState = Game1.GameState.lose;
                }
                #endregion

                #region generate food

                if (food_eaten)
                {
                    for (; ; )
                    {
                        int roll = rand.Next(0, 400);

                        if (contains[roll % sceneWidth, roll / sceneWidth] == Contain.empty)
                        {
                            contains[roll % sceneWidth, roll / sceneWidth] = Contain.food;
                            food_position.X = roll % sceneWidth;
                            food_position.Y = roll / sceneWidth;
                            foods.Add(new Food(img_food, origin, food_position));
                            food_eaten = false;
                            break;
                        }
                    }
                }
                #endregion

                AIfindWay();
            }

            #region player_control
            if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.HorizontalDrag:
                        if (gesture.Delta.X > 0)
                        {
                            player.turn(Direction.Right);
                        }
                        else if (gesture.Delta.X < 0)
                        {
                            player.turn(Direction.Left);
                        }
                        break;
                    case GestureType.VerticalDrag:
                        if (gesture.Delta.Y > 0)
                        {
                            player.turn(Direction.Down);
                        }
                        else if (gesture.Delta.Y < 0)
                        {
                            player.turn(Direction.Up);
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(img_background, new Vector2(0, 0), Color.White);
            spriteBatch.End();

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

        protected void AIfindWay()
        {
            Point AIHead = AI.HeadPosition;
            Stack<Point> way = new Stack<Point>();

            bool found = false;
            bool[,] map;
            map = new bool[sceneWidth, sceneHeight];
            for (int i=0; i< sceneWidth; i++)
                for (int j = 0; j < sceneHeight; j++)
                {
                    if (contains[i, j] == Contain.food || contains[i, j] == Contain.empty)
                        map[i, j] = true;
                    else
                        map[i, j] = false;
                }

            way.Push(AIHead);
            for (; ; )
            {

                Point top = way.Peek();

                if (contains[top.X, top.Y] == Contain.food)
                {
                    found = true;
                    break;
                }

                if (way.Count == 0)
                {
                    break;
                }

                int x,y;
                if (food_position.X > top.X)
                    x=1;
                else
                    x = -1;

                if (food_position.Y > top.Y)
                    y=1;
                else if (food_position.Y < top.Y)
                    y=-1;
                else 
                    y = 0;

                //尝试各个方向，可行则压栈，不可行则出栈寻找新路
                if (y == 0)
                {
                    y = 1;

                    if (map[top.X + x, top.Y])
                    {
                        way.Push(new Point(top.X + x, top.Y));
                        map[top.X + x, top.Y] = false;
                        continue;
                    }

                    if (map[top.X, top.Y + y])
                    {
                        way.Push(new Point(top.X, top.Y + y));
                        map[top.X, top.Y + y] = false;
                        continue;
                    }

                    x *= -1;
                    y *= -1;

                    if (map[top.X + x, top.Y])
                    {
                        way.Push(new Point(top.X + x, top.Y));
                        map[top.X + x, top.Y] = false;
                        continue;
                    }

                    if (map[top.X, top.Y + y])
                    {
                        way.Push(new Point(top.X, top.Y + y));
                        map[top.X, top.Y + y] = false;
                        continue;
                    }

                    way.Pop();
                }
                else
                {
                    if (map[top.X, top.Y + y])
                    {
                        way.Push(new Point(top.X, top.Y + y));
                        map[top.X, top.Y + y] = false;
                        continue;
                    }
                    if (map[top.X + x, top.Y])
                    {
                        way.Push(new Point(top.X + x, top.Y));
                        map[top.X + x, top.Y] = false;
                        continue;
                    }

                    x *= -1;
                    y *= -1;

                    if (map[top.X, top.Y + y])
                    {
                        way.Push(new Point(top.X, top.Y + y));
                        map[top.X, top.Y + y] = false;
                        continue;
                    }
                    if (map[top.X + x, top.Y])
                    {
                        way.Push(new Point(top.X + x, top.Y));
                        map[top.X + x, top.Y] = false;
                        continue;
                    }

                    way.Pop();
                }
            }//end for

            if (found && !food_eaten)
            {

                Point next = way.ElementAt<Point>(way.Count - 2);

                Debug.WriteLine(next);               
                Debug.WriteLine(AIHead);
                Debug.WriteLine("---");
                int x = next.X - AIHead.X;
                int y = next.Y - AIHead.Y;

                if (x == 0)
                {
                    if (y > 0)
                    {
                        Debug.WriteLine("turn");
                        AI.turn(Direction.Down);
                    }
                    else
                    {
                        Debug.WriteLine("turn");
                        AI.turn(Direction.Up);
                    }
                }
                else if (y == 0)
                {
                    if (x > 0)
                    {
                        Debug.WriteLine("turn");
                        AI.turn(Direction.Right);
                    }
                    else
                    {
                        Debug.WriteLine("turn");
                        AI.turn(Direction.Left);
                    }
                }
            }
            else
            {
                //实际上这里是最初版本的AI移动代码
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
            }
        }

    }
}
