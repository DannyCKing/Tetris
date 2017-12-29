using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{

    class GameModel
    {
        public bool inUse = false;
        public const int BOARD_WIDTH = 10;
        public const int BOARD_HEIGHT = 22;

        public int shapeCount = 0;

        public Color[,] boardColors = new Color[BOARD_WIDTH, BOARD_HEIGHT];

        public Shape currentShape
        {
            get;
            set;
        }

        public Shape nextShape
        {
            get;
            private set;
        }

        public Shape holdShape
        {
            get;
            private set;
        }

        Random rand;

        public int level = 1;

        public int score = 0;

        int linesCleared = 0;

        public bool gameOver
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines">the number of lines to start off that will be somewhat filled</param>
        public GameModel(int lines)
        {
            rand = new Random(2345235);
            currentShape = GetRandomShape();
            nextShape = GetRandomShape();
            holdShape = null;

            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    boardColors[i, j] = Color.Black;
                }
            }
        }

        public GameModel(GameModel game)
        {
            rand = game.rand;
            currentShape = game.currentShape;
            nextShape = game.nextShape.getAtTop();
            holdShape = game.holdShape;

            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    boardColors[i, j] = game.boardColors[i, j];
                }
            }
        }

        public Color GetColorAt(int x, int y)
        {
            return boardColors[x, y];
        }

        public Point[] getCurrentShapePoints()
        {
            return this.currentShape.getPoints();
        }

        public void progressGame()
        {
            this.moveDown();
        }

        public void moveLeft()
        {
            lock (currentShape)
            {
                this.currentShape.MoveLeft(ref this.boardColors);
            }
        }

        public void moveRight()
        {
            lock (currentShape)
            {
                this.currentShape.MoveRight(ref this.boardColors);
            }
        }

        public void moveDown()
        {
            lock(currentShape)
            {
                if (!this.currentShape.MoveDown(ref this.boardColors))
                {

                    Point[] paintPoints = getCurrentShapePoints();
                    Color newColor = this.GetShapeColor(currentShape);
                    for (int i = 0; i < 4; i++)
                    {
                        int x = paintPoints[i].x;
                        int y = paintPoints[i].y;
                        this.boardColors[x, y] = newColor;
                    }

                    //check for cleared lines
                    CheckForClearLines(true);

                    //check for game over
                    if (isGameOver())
                    {
                        gameOver = true;
                        return;
                    }
                     
                    this.currentShape = this.nextShape;
                    this.nextShape = GetRandomShape();
                }
            }
           
        }

        public int CheckForClearLines(bool clearLines)
        {
            int newLinesCleared = 0;
            linesCleared = 0;
            for (int i = BOARD_HEIGHT - 1; i > 0; i--)
            {
                bool lineIsFull = true;

                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    if (boardColors[j, i] == Color.Black)
                    {
                        lineIsFull = false;
                        break;
                    }
                }

                if (lineIsFull)
                {
                    removeLine(i, clearLines);
                    if (clearLines)
                    {
                        i++;
                    }
                    newLinesCleared++;
                }
            }


            AddToScore(newLinesCleared);

            linesCleared += newLinesCleared;
            
            level = GetLevelFromScore();

            return newLinesCleared;
        }

        private void AddToScore(int linesCleared)
        {
            score += linesCleared;
            if (linesCleared > 0)
            {
                //score += 100 * Convert.ToInt32(Math.Pow(2, linesCleared));
            }
        }

        private int GetLevelFromScore()
        {
            int level = (int)score / 1000;
            level = Math.Max(level, 1);
            level = Math.Min(level, 9);
            return level;
        }

        private void removeLine(int row, bool clearLine)
        {
            if (clearLine)
            {
                for (int i = row; i > 0; i--)
                {
                    for (int j = 0; j < BOARD_WIDTH; j++)
                    {
                        boardColors[j, i] = boardColors[j, i - 1];
                    }
                }
            }
        }

        public bool isGameOver()
        {
            //if any of the top rows are non black game is over
            const int hiddenPieces = 4;
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < hiddenPieces; j++)
                {
                    if (boardColors[i, j] != Color.Black)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void dropDown()
        {
            throw new NotImplementedException();
        }

        public void rotate()
        {
            lock (currentShape)
            {
                this.currentShape.rotatePiece(ref this.boardColors);
            }
        }



        private Shape GetRandomShape()
        {
            rand = new Random();
            shapeCount++;
            int x = rand.Next(7);
            //x = 0;
            switch (x)
            {
                case 0:
                    return new I ();
                case 1:
                    return new J();
                case 2:
                    return new L();
                case 3:
                    return new O();
                case 4:
                    return new S();
                case 5:
                    return new T();
                case 6:
                    return new Z();
                default:
                    return new T();
            }
        }


        public Color GetShapeColor(Shape shape)
        {
            if (shape == null)
            {
                return Color.White;
            }
            switch (shape.ToString())
            {
                case "I":
                    return Color.Cyan;
                case "J":
                    return Color.Blue;
                case "L":
                    return Color.Orange;
                case "O":
                    return Color.Yellow;
                case "S":
                    return Color.LimeGreen;
                case "T":
                    return Color.Purple;
                case "Z":
                    return Color.Red;
                default:
                    return Color.Pink;
            }
        }

        public void switchHold()
        {
            if (holdShape == null)
            {
                holdShape = currentShape;
                currentShape = nextShape.getAtTop();
                nextShape = GetRandomShape();
            }
            else
            {
                Shape temp = currentShape;
                currentShape = holdShape.getAtTop();
                holdShape = temp;
            }
        }


    }
}
