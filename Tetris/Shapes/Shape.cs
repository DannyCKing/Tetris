using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    public enum Orientation{ north, east, south, west};

    abstract class Shape
    {
        public Point[] points
        {
            get;
            protected set;
        }

        /*
        public Shape(Shape s)
        {
            points = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                points[i] = new Point(s.points[i].x, s.points[i].y);
            }
        }
        */
        public Orientation shapeOrientation
        {
            get;
            protected set;
        }

        public bool MoveDown(ref Color[,] board)
        {
            Point[] newPoints = new Tetris.Point[] { this.points[0].movedDownOne(), this.points[1].movedDownOne(), this.points[2].movedDownOne(), this.points[3].movedDownOne() };

            for (int i = 0; i < newPoints.Length; i++)
            {
                if (!this.isPossibleMove(newPoints[i], ref board))
                {
                    return false;
                }
            }

            this.points = newPoints;
            return true;
        }

        public void MoveLeft(ref Color[,] board)
        {
            Point[] newPoints = new Tetris.Point[] { this.points[0].movedLeftOne(), this.points[1].movedLeftOne(), this.points[2].movedLeftOne(), this.points[3].movedLeftOne() };

            for (
                int i = 0; i < newPoints.Length; i++)
            {
                if (!this.isPossibleMove(newPoints[i], ref board))
                {
                    return;
                }
            }

            this.points = newPoints;
            return;
        }

        public void MoveRight(ref Color[,] board)
        {
            Point[] newPoints = new Tetris.Point[] { this.points[0].movedRightOne(), this.points[1].movedRightOne(), this.points[2].movedRightOne(), this.points[3].movedRightOne() };

            for (int i = 0; i < newPoints.Length; i++)
            {
                if (!this.isPossibleMove(newPoints[i], ref board))
                {
                    return;
                }
            }

            this.points = newPoints;
            return;
        }

        protected bool isPossibleMove(Point p, ref Color[,] board)
        {
            bool cond1 = p.x < 0;
            bool cond2 = p.x >= GameModel.BOARD_WIDTH;
            bool cond3 = p.y < 0;
            bool cond4 = p.y >= GameModel.BOARD_HEIGHT;


            if (cond1 || cond2 || cond3 || cond4)
            {
                return false;
            }

            bool cond5 = board[p.x, p.y] != Color.Black;

            if (cond5)
            {
                return false;
            }

            return true;
        }

        public Point[] getPoints()
        {
            return this.points;
        }

        abstract public void rotatePiece(ref Color[,] board);
        abstract public Point[] getNewRotationPoints();
        abstract public Shape getAtTop();
        abstract public Orientation getNewRotationOrientation();

        public static Point [] GetSmallPanelPoints(Shape shape)
        {
            if (shape == null)
            {
                return new Point[] { };
            }
            switch (shape.ToString())
            {
                case "I":
                    return new Point [] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3) };
                case "J":
                    return new Point[] { new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(1, 2) };
                case "L":
                    return new Point[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 2) };
                case "O":
                    return new Point[] { new Point(1, 1), new Point(1, 2), new Point(2, 1), new Point(2, 2) };
                case "S":
                    return new Point[] { new Point(0, 2), new Point(1, 2), new Point(1, 1), new Point(2, 1) };
                case "T":
                    return new Point[] { new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(2, 2) };
                case "Z":
                    return new Point[] { new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 2) };
                default:
                    return new Point[] {};
            }
        }

        public override bool Equals(object obj)
        {
            Shape other = (Shape)obj;

            for (int i = 0; i < 4; i++)
            {
                if (this.points[i] != other.points[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
