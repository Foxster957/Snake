using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Program
    {
        static char[,] gameBoard = new char[11, 21] {
                {'+', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '+'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|'},
                {'+', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '+'}};

        public static List<SnakePart> snakeParts = new List<SnakePart>(); // Every part of the snake to be rendered
        public static List<FoodPiece> foodPieces = new List<FoodPiece>(); // Only contains one piece of food, list is only for convenience
        static bool alive;
        static ConsoleKey pressedKey;
        static char[,] frame;

        static void Main()
        {
            FoodPiece test = new FoodPiece(6, 3);
            foodPieces.Add(test);
            alive = true;
            SnakePart head = new SnakePart(0)
            {
                xPos = 10,
                yPos = 5,
                direction = 2
            };
            snakeParts.Add(head);
            RenderFrame();
            while (alive)
            {
                DateTime timeoutvalue = DateTime.Now.AddMilliseconds(500);
                while (DateTime.Now < timeoutvalue) // Detects keypresses for 500 ms
                {
                    if (Console.KeyAvailable)
                    {
                        pressedKey = Console.ReadKey(true).Key;
                        break;
                    }
                }

                head.oldDirection = head.direction;
                switch (pressedKey) // Sets a new direction for the head if an arrow key with an available direction was pressed
                {
                    case ConsoleKey.DownArrow:
                        if (head.direction == 0 || head.direction == 2)
                        {
                            break;
                        }
                        else
                        {
                            head.direction = 0;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (head.direction == 1 || head.direction == 3)
                        {
                            break;
                        }
                        else
                        {
                            head.direction = 1;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (head.direction == 2 || head.direction == 0)
                        {
                            break;
                        }
                        else
                        {
                            head.direction = 2;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (head.direction == 3 || head.direction == 1)
                        {
                            break;
                        }
                        else
                        {
                            head.direction = 3;
                        }
                        break;
                }
                
                foreach (SnakePart part in snakeParts)
                {
                    if (part.placement == 0)
                    {
                        Move(part, part.direction);
                    }
                    else
                    {
                        part.oldDirection = part.direction;
                        part.direction = GetPreviousPart(part).oldDirection;
                        Move(part, part.direction);
                    }
                }
                RenderFrame();
            }
        }

        static SnakePart GetPreviousPart(SnakePart part)
        {
            SnakePart previousPart = snakeParts[part.placement - 1];
            return previousPart;
        }

        static void Move(SnakePart part, int direction)
        {
            switch (direction)
            {
                case 0:
                    part.yPos++;
                    break;
                case 1:
                    part.xPos--;
                    break;
                case 2:
                    part.yPos--;
                    break;
                case 3:
                    part.xPos++;
                    break;
            }
        }

        static bool a = true;

        static int RandomX()
        {
            Random rnd = new Random();
            int result = rnd.Next(1, 20);
            return result;
        }

        static int RandomY()
        {
            Random rnd = new Random();
            int result = rnd.Next(1, 10);
            return result;
        }

        static void FoundFood()
        {
            foodPieces.RemoveAt(0);
            int xChange = 0;
            int yChange = 0;
            switch (snakeParts[^1].direction)
            {
                case 0:
                    xChange = 0;
                    yChange = -1;
                    break;
                case 1:
                    yChange = 0;
                    xChange = 1;
                    break;
                case 2:
                    xChange = 0;
                    yChange = 1;
                    break;
                case 3:
                    yChange = 0;
                    xChange = -1;
                    break;
            }
            snakeParts.Add(new SnakePart(snakeParts.Count)
            {
                xPos = snakeParts[^1].xPos + xChange,
                yPos = snakeParts[^1].yPos + yChange
            });
            RenderFrame();

            int xPos = 0;
            int yPos = 0;
            bool positionNotFound = true;
            while (positionNotFound)
            {
                xPos = RandomX();
                yPos = RandomY();
                if (frame[yPos, xPos] != 'o' && frame[yPos, xPos] != '@')
                {
                    positionNotFound = false;
                }
            }
            foodPieces.Add(new FoodPiece(xPos, yPos));
        }

        static void RenderFrame()
        {
            frame = (char[,]) gameBoard.Clone();
            bool foundFood = false;

            foreach (FoodPiece piece in foodPieces) // Places all food pieces in the frame
            {
                frame[piece.yPos, piece.xPos] = piece.symbol;
            }

            foreach (SnakePart part in snakeParts) // Places all snake parts in the frame and detects food
            {
                if (frame[part.yPos, part.xPos] == '¤' || frame[part.yPos, part.xPos] == '#')
                {
                    foundFood = true;
                }
                else if (frame[part.yPos, part.xPos] == '-' || frame[part.yPos, part.xPos] == '|')
                {
                    alive = false;
                }
                else if (frame[part.yPos, part.xPos] == 'o')
                {
                    alive = false;
                }
                frame[part.yPos, part.xPos] = part.symbol;
            }
            if (foundFood)
            {
                FoundFood();
                return;
            }

            int rowLength = frame.GetLength(0);
            int colLength = frame.GetLength(1);
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < rowLength; i++) // Prints the frame
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(frame[i, j]);
                }
                Console.Write(Environment.NewLine);
            }
            if (!alive)
            {
                Console.SetCursorPosition(6, 5);
                Console.Write("GAME OVER");
                Console.SetCursorPosition(0, 11);
            }
        }
    }

    class SnakePart
    {
        public int xPos;
        public int yPos;
        public int direction; // 0: down, 1: left, 2: up, 3: right
        public int oldDirection;
        public int placement;
        public char symbol;

        public SnakePart(int placement)
        {
            this.placement = placement;
            if (placement == 0)
            {
                symbol = '@';
            }
            else
            {
                symbol = 'o';
            }
        }
    }

    class FoodPiece
    {
        public int xPos;
        public int yPos;
        public char symbol;

        public FoodPiece(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            Random rnd = new Random();
            if (rnd.Next(0, 2) == 1)
            {
                symbol = '¤';
            }
            else
            {
                symbol = '#';
            }
        }
    }
}
