using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tetris
{

    class ComputerAI
    {
        GameModel game;

        List<Instruction> possibleInstructions;

        Color[,] originalBoard;

        Point[] originalShapeLocation;

        Orientation originalShapeOrientation;

        Shape originalShape;

        InstructionSet desiredInstructions;

        public ComputerAI(GameModel gameModel)
        {
            game = new GameModel(gameModel);
            possibleInstructions = new List<Instruction>();
            originalBoard = DeepCopyBoard(gameModel.boardColors);
            originalShapeLocation = game.currentShape.getPoints();
            originalShapeOrientation = game.currentShape.shapeOrientation;
            originalShape = game.currentShape;
            GetBestMove();
        }

        private void GetBestMove()
        {
            //get all possible instructions
             GetAllPossibleInstructions();

            //get the outcomes for all of the given instructions
            GetAllOutcomes();


            Instruction desiredInstruction = new Instruction(possibleInstructions[0].orientation, possibleInstructions[0].movement, possibleInstructions[0].shape, possibleInstructions[0].outcome);
            foreach (Instruction i in possibleInstructions)
            {
                if (i.CompareTo(desiredInstruction) < 0 )
                {
                    desiredInstruction = new Instruction(i.orientation, i.movement, i.shape, i.outcome);
                }
            }

            game.currentShape = game.currentShape.getAtTop();

            desiredInstructions = new InstructionSet(desiredInstruction, game.currentShape, game.boardColors);

            game.currentShape = game.currentShape.getAtTop();

        }

        private void GetAllOutcomes()
        {
            List<Instruction> instructionsAndOutcomes = new List<Instruction>();
            foreach (Instruction i in possibleInstructions)
            {
                Outcome o = GetOutcomeForInstruction(i);
                instructionsAndOutcomes.Add(new Instruction(i.orientation, i.movement, i.shape, o));
            }

            possibleInstructions = instructionsAndOutcomes;
        }

        private Color[,] DeepCopyBoard(Color [,] oldBoard)
        {
            int MAX_WIDTH = GameModel.BOARD_WIDTH;
            int MAX_HEIGHT = GameModel.BOARD_HEIGHT;
            Color[,] newBoard = new Color[MAX_WIDTH, MAX_HEIGHT];
            for (int i = 0; i < MAX_WIDTH; i++)
            {
                for (int j = 0; j < MAX_HEIGHT; j++)
                {
                    newBoard[i, j] = oldBoard[i, j];
                }
            }
            return newBoard;
        }

        private Outcome GetOutcomeForInstruction(Instruction instruction)
        {
            game.currentShape = game.currentShape.getAtTop();

            //SWITCH PIECE FIRST
            if (game.currentShape.ToString() != instruction.shape.ToString())
            {
                game.switchHold();
            }

            //DO MOVEMENT
            game.currentShape = game.currentShape.getAtTop();
            game.boardColors = DeepCopyBoard(originalBoard);
            Orientation beforeTurn = game.currentShape.shapeOrientation;
            while (game.currentShape.shapeOrientation != instruction.orientation)
            {
                game.currentShape.rotatePiece(ref game.boardColors);
                Orientation afterTurn = game.currentShape.shapeOrientation;
                if (beforeTurn == afterTurn)
                {
                    //we got trouble
                    throw new Exception();
                }
            }
            
            Point[] before = game.currentShape.points;

            //move correct amount
            int moves = 0;
            
            while (moves < Math.Abs(instruction.movement))
            {
                if (instruction.movement < 0)
                {
                    game.currentShape.MoveLeft(ref game.boardColors);
                    moves++;
                }
                else
                {
                    game.currentShape.MoveRight(ref game.boardColors);
                    moves++;
                }
            }

            int linesCleared = 0;
            int height = 0;
            Point[] paintPoints = new Point[] { };

            //move down
            while(game.currentShape.MoveDown(ref game.boardColors))
            {}

            //CALCULATE RESULTS

            //paint shape and get lines that would be cleared
            if (!game.currentShape.MoveDown(ref game.boardColors))
            {
                paintPoints = game.getCurrentShapePoints();
                Color newColor = game.GetShapeColor(game.currentShape);
                for (int i = 0; i < 4; i++)
                {
                    int x = paintPoints[i].x;
                    int y = paintPoints[i].y;
                    game.boardColors[x, y] = newColor;
                }

                //check for cleared lines
                linesCleared = game.CheckForClearLines(false);
            }
            else
            {
                throw new Exception();
            }

            //get landing height;
            int landingHeight = GetLandingHeight();

            //get holes
            int numOfHoles = GetNumberOfHoles();

            int rowTransitions = GetRowTransitions();

            int columnTransitions = GetColumnTransitions();

            height = GetHeightAfterPlacement(linesCleared);

            int wellSums = GetWellSums();

            return new Outcome(linesCleared, numOfHoles, height, landingHeight, rowTransitions,columnTransitions, wellSums);
        }

        private int GetWellSums()
        {
            var well_sums = 0;
            int currentSection = 0;
            int[] topCell = new int[GameModel.BOARD_WIDTH];

            //get top cell in each columns
            for (int i = 0; i < GameModel.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < GameModel.BOARD_HEIGHT; j++)
                {
                    bool isBlack = game.boardColors[i, j] == Color.Black;
                    if (!isBlack)
                    {
                        topCell[i] = j;
                        break;
                    }
                }
            }

            // Check for well cells in the "inner columns" of the board.
            // "Inner columns" are the columns that aren't touching the edge of the board.
            for (int i = 1; i < GameModel.BOARD_WIDTH - 1; i++)
            {
                currentSection = 0;
                for (int j = topCell[i] - 1; j >=0 ; j--)
                {
                    bool currentIsBlack = game.boardColors[i, j] == Color.Black;
                    bool leftIsBlack = game.boardColors[i - 1, j] == Color.Black;
                    bool rightIsBlack = game.boardColors[i + 1, j] == Color.Black;
                    if (!leftIsBlack && currentIsBlack && !rightIsBlack)
                    {
                        currentSection++;
                        well_sums += currentSection;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Check for well cells in the leftmost column of the board.
            int leftCol = 0;
            currentSection = 0;
            for (int j = topCell[leftCol] - 1; j >= 0; j--)
            {
                bool currentIsBlack = game.boardColors[leftCol, j] == Color.Black;
                bool rightIsBlack = game.boardColors[leftCol + 1, j] == Color.Black;
                if (currentIsBlack && !rightIsBlack)
                {
                    currentSection++;
                    well_sums += currentSection;
                }
                else
                {
                    break;
                }
            }

            // Check for well cells in the rightmost column of the board.
            int rightCol = GameModel.BOARD_WIDTH - 1;
            currentSection = 0;
            for (int j = topCell[rightCol] - 1; j >= 0; j--)
            {
                bool currentIsBlack = game.boardColors[rightCol, j] == Color.Black;
                bool leftIsBlack = game.boardColors[rightCol - 1, j] == Color.Black;
                if (!leftIsBlack && currentIsBlack)
                {
                    currentSection++;
                    well_sums += currentSection;
                }
                else
                {
                    break;
                }
            }

            return well_sums;
        }

        private int GetColumnTransitions()
        {
            int trans = 0;
            bool[] hasSeenPiece = new bool[GameModel.BOARD_WIDTH];

            for (int i = 0; i < GameModel.BOARD_HEIGHT - 1; i++)
            {
                for (int j = 0; j < GameModel.BOARD_WIDTH ; j++)
                {
                    bool isBlack = game.boardColors[j, i] == Color.Black;
                    bool nextisBlack = game.boardColors[j, i + 1] == Color.Black;
                    if (isBlack == !nextisBlack)
                    {
                        trans++;
                    }
                }
            }

            return trans;
        }

        private int GetRowTransitions()
        {
            int trans = 0;
            bool[] hasSeenPiece = new bool[GameModel.BOARD_WIDTH];

            for (int i = 0; i < GameModel.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameModel.BOARD_WIDTH - 1; j++)
                {
                    bool isBlack = game.boardColors[j, i] == Color.Black;
                    bool nextisBlack = game.boardColors[j + 1, i] == Color.Black;
                    if (isBlack == !nextisBlack)
                    {
                        trans++;
                    }
                }
            }

            return trans;
        }

        private int GetNumberOfHoles()
        {
            int holes = 0;
            int[] topCell = new int[GameModel.BOARD_WIDTH];

            //get top cell in each columns
            for (int i = 0; i < GameModel.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < GameModel.BOARD_HEIGHT; j++)
                {
                    bool isBlack = game.boardColors[i, j] == Color.Black;
                    if (!isBlack)
                    {
                        topCell[i] = j;
                        break;
                    }
                }
            }

            for (int i = 0; i < GameModel.BOARD_WIDTH; i++)
            {
                if (topCell[i] == 0)
                {
                    continue;
                }
                for (int j = topCell[i] + 1; j <GameModel.BOARD_HEIGHT; j++)
                {
                    bool currentIsBlack = game.boardColors[i, j] == Color.Black;
                    if (currentIsBlack)
                    {
                        holes++;
                    }
                }
            }

            return holes;
        }

        private int GetLandingHeight()
        {
            int height = 0;
            foreach (Point p in game.currentShape.points)
            {
                height += p.y;
            }
            return GameModel.BOARD_HEIGHT - height / 4;
        }

        private int GetHeightAfterPlacement(int linesCleared)
        {
            int height = 0;
            //get top cell in each columns
            for (int i = 0; i < GameModel.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameModel.BOARD_WIDTH; j++)
                {
                    bool isBlack = game.boardColors[j, i] == Color.Black;
                    if (!isBlack)
                    {
                        height = i;
                        break;
                    }
                }
                if (height != 0)
                {
                    break;
                }
            }

            height = GameModel.BOARD_HEIGHT - height - linesCleared;
            
            return height;
        }

        private void GetAllPossibleInstructions()
        {
            List<Instruction> ret = new List<Instruction>();
            List<Shape> shapes = new List<Shape>();
            shapes.Add(game.currentShape);

			// TO DO: Make AI smart enough to look at next and hold pieces to see if
			// those were used what the best outcome would be.
            //if (game.holdShape != null)
            //{
            //    shapes.Add(game.holdShape.getAtTop());
            //}
            //else
            //{
            //    shapes.Add(game.nextShape.getAtTop());
            //}

            foreach(Shape s in shapes)
            {
                game.currentShape = s.getAtTop();
                List<Orientation> orientations = GetAllOrientations();
                foreach (Orientation orientation in orientations)
                {
                    
                    game.currentShape = game.currentShape.getAtTop();
                    while (game.currentShape.shapeOrientation != orientation)
                    {
                        game.currentShape.rotatePiece(ref game.boardColors);
                    }

                    int leftMoves = 0;
                    Point[] before = game.currentShape.points;
                    Point[] after = null;
                    while (!before.Equals(after))
                    {
                        possibleInstructions.Add(new Instruction(orientation, 0 - leftMoves, s));
                        leftMoves++;
                        before = game.currentShape.points;
                        game.currentShape.MoveLeft(ref game.boardColors);
                        after = game.currentShape.points;
                    }

                    game.currentShape = game.currentShape.getAtTop();

                    while (game.currentShape.shapeOrientation != orientation)
                    {
                        game.currentShape.rotatePiece(ref game.boardColors);
                    }
                
                    int rightMoves = 0;

                    before = game.currentShape.points;
                    after = null;

                    while (!before.Equals(after))
                    {
                        //originally position has already been stored
                        if (rightMoves != 0)
                        {
                            possibleInstructions.Add(new Instruction(orientation, rightMoves, s));
                        }
                        rightMoves++;
                        before = game.currentShape.points;
                        game.currentShape.MoveRight(ref game.boardColors);
                        after = game.currentShape.points;
                    }

                }
            }

            game.currentShape = originalShape.getAtTop() ;
        }

        private List<Orientation> GetAllOrientations()
        {
            List<Orientation> orientations = new List<Orientation>();

            //get all possible orientations
            while (true)
            {
                Orientation o = game.currentShape.shapeOrientation;
                if (orientations.Contains(o))
                {
                    break;
                }
                else
                {
                    orientations.Add(o);
                    game.currentShape.rotatePiece(ref game.boardColors);
                }
            }

            return orientations;
        }

        public SingleInstructions getNextInstruction()
        {
            return desiredInstructions.GetInstruction();
        }
    }
}
