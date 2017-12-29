using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{
    public enum SingleInstructions { moveRight, moveLeft, moveDown, boomDown, rotate, switchPiece, none };
    class InstructionSet
    {

        public Queue<SingleInstructions> instructions;

        public InstructionSet(Instruction instruction, Shape s, Color[,] board)
        {
            instructions = new Queue<SingleInstructions>();
            s = s.getAtTop();
            if (instruction.shape.ToString() != s.ToString())
            {
                instructions.Enqueue(SingleInstructions.switchPiece);
                s = instruction.shape.getAtTop() ;
            }

            while (instruction.orientation != s.shapeOrientation)
            {
                instructions.Enqueue(SingleInstructions.rotate);
                s.rotatePiece(ref board);
            }

            int moves = 0;
            
            while (moves < Math.Abs(instruction.movement))
            {
                if (instruction.movement < 0)
                {
                    instructions.Enqueue(SingleInstructions.moveLeft);
                    moves++;
                }
                else
                {
                    instructions.Enqueue(SingleInstructions.moveRight);
                    moves++;
                }
            }
        }

        public SingleInstructions GetInstruction()
        {
            if (instructions.Count > 0)
            {
                return instructions.Dequeue();
            }
            else
            {
                return SingleInstructions.none ;
            }
        }
    }
}
