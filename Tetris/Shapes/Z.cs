﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris;
using System.Drawing;

namespace Tetris
{
    class Z : Shape
    {
        public Z()
        {
            ///             x   x                      
            ///                 x   x
            this.points = new Point[] { new Point(4, 2), new Point(5, 2), new Point(5, 3), new Point(6, 3) };
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
                ///                                 x                      
                ///        x    x       ->      x   x   
                ///             x   x           x   
                return new Tetris.Point[] { this.points[0].movedRightOne().movedUpOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedLeftOne().movedUpOne(), 
                                                        this.points[3].movedLeftOne().movedLeftOne() };
            }
            else
            {
                ///             x                                        
                ///        x    x       ->      x   x   
                ///        x                        x   x   
                return new Tetris.Point[] { this.points[0].movedDownOne().movedLeftOne(), 
                                                        this.points[1], 
                                                        this.points[2].movedRightOne().movedDownOne(), 
                                                        this.points[3].movedRightOne().movedRightOne() };
            }
        }

        public override Shape getAtTop()
        {
            return new Z();
        }

        public override string ToString()
        {
            return "Z";
        }
    }
}
