using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Instruction : IComparable<Instruction>
    {
        public Shape shape;
        public Orientation orientation;
        public int movement = 0;
        public int xLeftMostLocation = 0;
        public Outcome outcome;

        public Instruction(Orientation orientation, int movement, Shape shape)
        {
            this.orientation = orientation;
            this.movement = movement;
            this.shape = shape;
        }

        public Instruction(Orientation orientation, int movement,  Shape shape, Outcome outcome)
        {
            this.orientation = orientation;
            this.movement = movement;
            this.shape = shape;
            this.outcome = outcome;
        }

        public int CompareTo(Instruction other)
        {
            return this.outcome.CompareTo(other.outcome);
        }


    }
}
