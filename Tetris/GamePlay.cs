using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Tetris
{
    public enum PlayerType { Computer, Human };
      
    public partial class GamePlay : Form
    {
        ComputerAI computerAI;
        private const int BOARD_WIDTH = 10;

        private const int ACTUAL_BOARD_HEIGHT = 22;
        private const int BOARD_HEIGHT = 18;

        private const int SQ_WIDTH = 30;

        PlayerType playerType;
        int level = 0;
        int lines;
        GameModel gameModel;
        Control[,] squares;

        Control[,] holdTiles;
        Control[,] nextTiles;
        Shape nextShape;
        Shape holdShape;
        int shapeCount = 0;

        //Time objects
        private DateTime lastTime;
        private System.Timers.Timer progressTimer;
        private int timerInterval = 1000;

        //Key area
        TextBox inputKeys = new TextBox();

        int tickCount = 0;


        public GamePlay(PlayerType player, int level, int lines)
        {

            InitializeComponent();

            //show entire board
            gameBoard.Width = SQ_WIDTH * BOARD_WIDTH;
            gameBoard.Height = SQ_WIDTH * BOARD_HEIGHT;

            //optimize
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                          , true);


            this.playerType = player;
            this.level = level;
            this.lines = lines;

            this.holdShape = null;
            this.nextShape = null;

            this.gameModel = new GameModel(lines);

            inputKeys.Size = new Size(1, 1);
            this.Controls.Add(inputKeys);
            inputKeys.LostFocus += new EventHandler(inputKeys_LostFocus);

            inputKeys.Select();
            inputKeys.Focus();

            inputKeys.PreviewKeyDown += new PreviewKeyDownEventHandler(GamePlay_PreviewKeyDown);
            inputKeys.KeyDown += new KeyEventHandler(GamePlay_KeyDown);

            SetText(scoreLabel, gameModel.score.ToString());
            SetText(levelLabel, gameModel.level.ToString());

            SetUpGameBoard();

            holdTiles = new Control[4, 4];
            nextTiles = new Control[4, 4];
            SetUpSidePanel(HoldPanel);
            SetUpSidePanel(NextPanel);
        }

        private void OnGamePlayLoaded(object sender, EventArgs e)
        {
            //InitializeComponent();
            BeginGame();

            inputKeys.Focus();
        }



        // PreviewKeyDown is where you preview the key. 
        // Do not put any logic here, instead use the 
        // KeyDown event after setting IsInputKey to true. 
        private void inputKeys_LostFocus(object sender, EventArgs e)
        {
            inputKeys.Focus();
        }

        // PreviewKeyDown is where you preview the key. 
        // Do not put any logic here, instead use the 
        // KeyDown event after setting IsInputKey to true. 
        private void GamePlay_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }

        void GamePlay_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    gameModel.moveDown();
                    break;
                case Keys.Left:
                    gameModel.moveLeft();
                    break;
                case Keys.Right:
                    gameModel.moveRight();
                    break;
                case Keys.Space:
                    break;
                case Keys.Up:
                    gameModel.rotate();
                    break;
                case Keys.H:
                    gameModel.switchHold();
                    break;
                case Keys.S:
                    CreateAI();
                    break;
                default:
                    return;
            }

            SetPanelPiece(NextPanel, gameModel.nextShape);
            SetPanelPiece(HoldPanel, gameModel.holdShape);

            DrawBoard();
        }

        private void BeginGame()
        {
            progressTimer = new System.Timers.Timer(timerInterval);
            progressTimer.Elapsed += (s, e) => {
                if (gameModel.gameOver == true )
                {
                    progressTimer.Stop();
                    return;
                }
                    tickCount++;
                    gameModel.progressGame();
                    DrawBoard();

                    //update score and level
                    SetText(scoreLabel, gameModel.score.ToString());
                    SetText(levelLabel, gameModel.level.ToString());
                    level = gameModel.level;

                    //udpate side panel
                    SetPanelPiece(NextPanel, gameModel.nextShape);
                    SetPanelPiece(HoldPanel, gameModel.holdShape);
                progressTimer.Enabled = true; // enables firing Elapsed event
                timerInterval = 1000 - (level * 95);
                this.progressTimer.Interval = timerInterval;
            };

            //create AI timer
            if (playerType == PlayerType.Computer)
            {
                CreateAI();
            }
            if (playerType == PlayerType.Human)
            {
                progressTimer.Start();
            }

        }

        private void CreateAI()
        {
            Thread AI_Thread = new Thread(new ThreadStart(RunAI));
            this.shapeCount = gameModel.shapeCount;
            computerAI = new ComputerAI(gameModel);
            AI_Thread.Start();
        }

        private void RunAI()
        {
            lock (gameModel)
            {
                while (!gameModel.gameOver)
                {
                    Thread.Sleep(5);
                    if (gameModel.shapeCount != this.shapeCount)
                    {
                        computerAI = new ComputerAI(gameModel);

                        this.shapeCount = gameModel.shapeCount;
                    }
                    tickCount++;
                    SingleInstructions instruction;
                    instruction = computerAI.getNextInstruction();

                    //instruction = GetRandomInstruction();

                    switch (instruction)
                    {
                        case SingleInstructions.rotate:
                            gameModel.rotate();
                            break;
                        case SingleInstructions.moveLeft:
                            gameModel.moveLeft();
                            break;
                        case SingleInstructions.moveRight:
                            gameModel.moveRight();
                            break;
                        case SingleInstructions.switchPiece:
                            gameModel.switchHold();
                            break;
                        default:
                            gameModel.moveDown();
                            break;
                    }

                    DrawBoard();


                    //update score and level
                    SetText(scoreLabel, gameModel.score.ToString());
                    SetText(levelLabel, gameModel.level.ToString());
                    level = gameModel.level;

                    //udpate side panel
                    SetPanelPiece(NextPanel, gameModel.nextShape);
                    SetPanelPiece(HoldPanel, gameModel.holdShape);
                }
            }
        }

        private SingleInstructions GetRandomInstruction()
        {
            SingleInstructions instruction;
            switch (new Random().Next(10))
            {
                case 0:
                    instruction = SingleInstructions.rotate;
                    break;
                case 1:
                    instruction = SingleInstructions.moveLeft;
                    break;
                case 2:
                    instruction = SingleInstructions.moveRight;
                    break;
                default:
                    instruction = SingleInstructions.moveDown;
                    break;
            }
            return instruction;
        }

        private void SetPanelPiece(Panel panel, Shape shape)
        {
            Control[,] tiles;
            if (panel == HoldPanel)
            {
                if (holdShape != null && holdShape.GetType() == shape.GetType())
                {
                    return;
                }
                holdShape = shape;
                tiles = holdTiles;
            }
            else
            {
                if (nextShape != null && nextShape.GetType() == shape.GetType())
                {
                    return;
                }
                nextShape = shape;
                tiles = nextTiles;
            }

            Point [] points = Shape.GetSmallPanelPoints(shape);
            DrawPointsColor(tiles, points, gameModel.GetShapeColor(shape));
        }

        private void DrawPointsColor(Control[,] tiles, Point[] points, Color color)
        {
            //draw set pieces
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tiles[i, j].BackColor = Color.Black;
                    tiles[i, j].Invalidate();
                }
            }

            for(int n =0 ; n < points.Length ; n ++)
            {
                tiles[points[n].x, points[n].y].BackColor = color;
                tiles[points[n].x, points[n].y].Invalidate();
            }
        }

        delegate void SetTextCallback(Control control, string text);

        private void SetText(Control control, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                try
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { control, text });
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                control.Text = text;
            }
        }

        void TimerTick(object sender, EventArgs e)
        {

        }

        private void SetUpGameBoard()
        {
            squares = new Control[BOARD_WIDTH, BOARD_HEIGHT];
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    squares[i, j] = new Control("", i * SQ_WIDTH, j * SQ_WIDTH, SQ_WIDTH, SQ_WIDTH);
                    gameBoard.Controls.Add(squares[i, j]);
                    squares[i, j].BackColor = Color.Black;
                    squares[i, j].Invalidate();
                }
            }
            gameBoard.Invalidate();
        }

        private void SetUpSidePanel(Panel control)
        {
            Control[,] tiles;
            if (control == HoldPanel)
            {
                tiles = holdTiles;
            }
            else
            {
                tiles = nextTiles;
            }

            int sq_width = control.Width / 4;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tiles[i, j] = new Control("", i * sq_width, j * sq_width, sq_width, sq_width);
                    control.Controls.Add(tiles[i, j]);
                    tiles[i, j].BackColor = Color.Black;
                    tiles[i, j].Invalidate();
                }
            }
            control.Invalidate();
        }

        private void DrawBoard()
        {
            if (BOARD_HEIGHT == ACTUAL_BOARD_HEIGHT)
            {
                DrawEntireBoard();
                return;
            }
               
            //draw set pieces
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    if( squares[i, j].BackColor == gameModel.GetColorAt(i, j+4))
                    {
                        continue;
                    }
                    else
                    {
                        squares[i, j].BackColor = gameModel.GetColorAt(i, j+4);
                        squares[i, j].Invalidate();
                    }
                }
            }

            //draw moving pieces
            Point [] shapePoints = gameModel.getCurrentShapePoints();
            Color shapeColor = gameModel.GetShapeColor(gameModel.currentShape);
            for (int i = 0; i < 4; i++)
            {
                int x = shapePoints[i].x;
                int y = shapePoints[i].y - 4;
                if (x < 0 || y < 0 || x >= BOARD_WIDTH || y >= BOARD_HEIGHT)
                {
                    continue;
                }
                squares[x, y].BackColor = shapeColor;
                squares[x, y].Invalidate();
            }

        }

        private void DrawEntireBoard()
        {
            //draw set pieces
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    if (squares[i, j].BackColor == gameModel.GetColorAt(i, j ))
                    {
                        continue;
                    }
                    else
                    {
                        squares[i, j].BackColor = gameModel.GetColorAt(i, j );
                        squares[i, j].Invalidate();
                    }
                }
            }

            //draw moving pieces
            Point[] shapePoints = gameModel.getCurrentShapePoints();
            Color shapeColor = gameModel.GetShapeColor(gameModel.currentShape);
            for (int i = 0; i < 4; i++)
            {
                int x = shapePoints[i].x;
                int y = shapePoints[i].y;
                if (x < 0 || y < 0 || x >= BOARD_WIDTH || y >= BOARD_HEIGHT)
                {
                    continue;
                }
                squares[x, y].BackColor = shapeColor;
                squares[x, y].Invalidate();
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }






    }
}
