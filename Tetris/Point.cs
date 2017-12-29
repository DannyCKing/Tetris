using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point movedDownOne()
        {
            return new Point(x, y + 1);
        }

        public Point movedLeftOne()
        {
            return new Point(x - 1, y);
        }

        public Point movedRightOne()
        {
            return new Point(x + 1, y);
        }

        public Point movedUpOne()
        {
            return new Point(x , y-1);
        }
    }
}
