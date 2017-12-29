namespace Tetris
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PlayButton = new System.Windows.Forms.Button();
            this.QuitButton = new System.Windows.Forms.Button();
            this.HumanPlayer = new System.Windows.Forms.RadioButton();
            this.ComputerPlayer = new System.Windows.Forms.RadioButton();
            this.Level = new System.Windows.Forms.ComboBox();
            this.Lines = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(146, 212);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(75, 23);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(46, 212);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 1;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // HumanPlayer
            // 
            this.HumanPlayer.AutoSize = true;
            this.HumanPlayer.Location = new System.Drawing.Point(146, 22);
            this.HumanPlayer.Name = "HumanPlayer";
            this.HumanPlayer.Size = new System.Drawing.Size(59, 17);
            this.HumanPlayer.TabIndex = 2;
            this.HumanPlayer.TabStop = true;
            this.HumanPlayer.Text = "Human";
            this.HumanPlayer.UseVisualStyleBackColor = true;
            // 
            // ComputerPlayer
            // 
            this.ComputerPlayer.AutoSize = true;
            this.ComputerPlayer.Location = new System.Drawing.Point(146, 45);
            this.ComputerPlayer.Name = "ComputerPlayer";
            this.ComputerPlayer.Size = new System.Drawing.Size(70, 17);
            this.ComputerPlayer.TabIndex = 3;
            this.ComputerPlayer.TabStop = true;
            this.ComputerPlayer.Text = "Computer";
            this.ComputerPlayer.UseVisualStyleBackColor = true;
            // 
            // Level
            // 
            this.Level.FormattingEnabled = true;
            this.Level.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.Level.Location = new System.Drawing.Point(146, 85);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(121, 21);
            this.Level.TabIndex = 4;
            // 
            // Lines
            // 
            this.Lines.FormattingEnabled = true;
            this.Lines.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.Lines.Location = new System.Drawing.Point(146, 134);
            this.Lines.Name = "Lines";
            this.Lines.Size = new System.Drawing.Size(121, 21);
            this.Lines.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Player";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Level";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Lines";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lines);
            this.Controls.Add(this.Level);
            this.Controls.Add(this.ComputerPlayer);
            this.Controls.Add(this.HumanPlayer);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.PlayButton);
            this.Name = "Form1";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.RadioButton HumanPlayer;
        private System.Windows.Forms.RadioButton ComputerPlayer;
        private System.Windows.Forms.ComboBox Level;
        private System.Windows.Forms.ComboBox Lines;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

