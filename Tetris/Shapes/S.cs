using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    class S : Shape
    {
        public S()
        {
            ///                 x   x                   
            ///             x   x
            this.points = new Point[] { new Point(6, 2), new Point(5, 2), new Point(5, 3), new Point(4, 3) };
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
            else
            {
                return Orientation.north;
            }
        }

        public override Point[] getNewRotationPoints()
        {
            if (this.shapeOrientation == Orientation.north)
            {
                ///                       x                          
                ///          x x  ->      x x   
                ///        x x              x   
                return new Tetris.Point[] { this.points[0].movedUpOne().movedLeftOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedRightOne().movedUpOne(), 
                                                        this.points[3].movedRightOne().movedRightOne() };
            }
            else
            {
                ///          x              x x                          
                ///          x x  ->      x x   
                ///            x             
                return new Tetris.Point[] { this.points[0].movedDownOne().movedRightOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedLeftOne().movedDownOne(), 
                                                        this.points[3].movedLeftOne().movedLeftOne()};
            }
        }


        public override Shape getAtTop()
        {
            return new S();
        }

        public override string ToString()
        {
            return "S";
        }
    }
}
