using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    class O : Shape
    {
        public O()
        {            
            ///                 x   x
            ///                 x   x
            this.points = new Point[] { new Point(5, 2), new Point(4, 2), new Point(5, 3), new Point(4, 3) };
            this.shapeOrientation = Orientation.north;
        }

        public override void rotatePiece(ref Color[,] board)
        {
            return;
        }

        public override Orientation getNewRotationOrientation()
        {
            return Orientation.north;
        }

        public override Point[] getNewRotationPoints()
        {
            return this.points;
        }

        public override Shape getAtTop()
        {
            return new O();
        }

        public override string ToString()
        {
            return "O";
        }
    }
}
