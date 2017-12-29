using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    class T : Shape
    {
        
        public T()
        {
            ///             x   x   x                   
            ///                 x
            this.points = new Point[] { new Point(4, 2), new Point(5, 2), new Point(5, 3), new Point(6, 2) };
            this.shapeOrientation = Orientation.north;
        }
        
        public override void rotatePiece(ref Color[,] board)
        {
            Point[] newPoints = getNewRotationPoints();

            for (int i = 0; i < newPoints.Length; i++)
            {
                if (!this.isPossibleMove(newPoints[i], ref board))
                {
                    return;
                }
            }

            this.points = newPoints;
            this.shapeOrientation = getNewRotationOrientation();
            return;
        }

        public override Orientation getNewRotationOrientation()
        {
            if (this.shapeOrientation == Orientation.north)
            {
                return Orientation.east;
            }
            else if (this.shapeOrientation == Orientation.east)
            {
                return Orientation.south;
            }
            else if (this.shapeOrientation == Orientation.south)
            {
                return Orientation.west;
            }
            else
            {
                return Orientation.north;
            }
        }

        public override Point[] getNewRotationPoints()
        {
            if (this.shapeOrientation == Orientation.north)
            {
                return new Tetris.Point[] { this.points[0].movedUpOne().movedRightOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedUpOne().movedLeftOne(), 
                                                        this.points[3].movedDownOne().movedLeftOne()};
            }
            else if (this.shapeOrientation == Orientation.east)
            {
                return new Tetris.Point[] { this.points[0].movedDownOne().movedRightOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedUpOne().movedRightOne(), 
                                                        this.points[3].movedUpOne().movedLeftOne() };
            }
            else if (this.shapeOrientation == Orientation.south)
            {
                return new Tetris.Point[] { this.points[0].movedDownOne().movedLeftOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedDownOne().movedRightOne(), 
                                                        this.points[3].movedUpOne().movedRightOne() };
            }
            else
            {
                return new Tetris.Point[] { this.points[0].movedUpOne().movedLeftOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedDownOne().movedLeftOne(), 
                                                        this.points[3].movedDownOne().movedRightOne() };
            }
        }

        public override Shape getAtTop()
        {
            return new T();
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
