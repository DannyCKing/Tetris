using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        GamePlay gamePlay;

        public Form1()
        {
            InitializeComponent();
            
            //Select Human Player by default
            HumanPlayer.Select();

            //default to level 0 
            Level.SelectedIndex = 0;

            //default to 0 lines
            Lines.SelectedIndex = 0;
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            BeginGame();
        }

        private void BeginGame()
        {
            PlayerType player;
            int level;
            int lines;
            
            if(HumanPlayer.Checked)
            {
                player = PlayerType.Human;
            }
            else
            {
                player = PlayerType.Computer;
            }

            level = Level.SelectedIndex +1;
            lines = Lines.SelectedIndex;

            PlayerType playerType = HumanPlayer.Checked ? PlayerType.Human : PlayerType.Computer;

            gamePlay = new GamePlay(playerType, level, lines);
            gamePlay.Show();
            
        }
    }
}
