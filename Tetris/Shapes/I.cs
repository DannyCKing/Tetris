using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    class I : Shape
    {
        public I()
        {
            ///                 x                   
            ///                 x
            ///                 x                   
            ///                 x
            this.points = new Point[] { new Point(3, 3), new Point(4, 3), new Point(5, 3), new Point(6, 3) };
            this.shapeOrientation = Orientation.east;
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
            else
            {
                return Orientation.north;
            }
        }

        public override Point[] getNewRotationPoints()
        {
            if (this.shapeOrientation == Orientation.north)
            {
                return new Tetris.Point[] { this.points[0].movedLeftOne().movedUpOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedRightOne().movedDownOne(), 
                                                        this.points[3].movedRightOne().movedRightOne().movedDownOne().movedDownOne() };
            }
            else
            {
                return new Tetris.Point[] { this.points[0].movedRightOne().movedDownOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedLeftOne().movedUpOne(), 
                                                        this.points[3].movedLeftOne().movedLeftOne().movedUpOne().movedUpOne() };
            }
        }

        public override Shape getAtTop()
        {
            return new I();
        }

        public override string ToString()
        {
            return "I";
        }
    }
}
