using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace B18_Ex05_UserInterface
{
    internal class SettingsForm : Form
    {
        private Label labelBoardSize;
        private Label labelPlayers;
        private Label labelPlayerOne;
        private Label labelPlayerTwo;
        private Button buttonDone;
        private RadioButton radioButtonSize6;
        private RadioButton radioButtonSize8;
        private RadioButton radioButtonSize10;
        private TextBox textBoxPlayerOneName;
        private TextBox textBoxPlayerTwoName;
        private CheckBox checkBoxPlayerTwo;
        private string m_PlayerOneName;
        private string m_PlayerTwoName;
        private int m_BoardSize;
        private bool m_AgainstComputer = true;

        public SettingsForm()
        {
            initializeComponent();
        }

        private void initializeComponent()
        {
            this.labelBoardSize = new Label();
            this.buttonDone = new Button();
            this.radioButtonSize6 = new RadioButton();
            this.labelPlayers = new Label();
            this.labelPlayerOne = new Label();
            this.labelPlayerTwo = new Label();
            this.textBoxPlayerOneName = new TextBox();
            this.textBoxPlayerTwoName = new TextBox();
            this.checkBoxPlayerTwo = new CheckBox();
            this.radioButtonSize8 = new RadioButton();
            this.radioButtonSize10 = new RadioButton();
            this.SuspendLayout();

            // labelBoardSize
            this.labelBoardSize.AutoSize = true;
            this.labelBoardSize.Location = new System.Drawing.Point(25, 19);
            this.labelBoardSize.Name = "labelBoardSize";
            this.labelBoardSize.Size = new System.Drawing.Size(123, 25);
            this.labelBoardSize.TabIndex = 0;
            this.labelBoardSize.Text = "Board Size:";
            
            // buttonDone 
            this.buttonDone.Location = new System.Drawing.Point(249, 256);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);

            // radioButtonSize6 
            this.radioButtonSize6.AutoSize = true;
            this.radioButtonSize6.Location = new System.Drawing.Point(44, 60);
            this.radioButtonSize6.Name = "radioButtonSize6";
            this.radioButtonSize6.Size = new System.Drawing.Size(78, 29);
            this.radioButtonSize6.TabIndex = 2;
            this.radioButtonSize6.TabStop = true;
            this.radioButtonSize6.Text = "6x6";
            this.radioButtonSize6.UseVisualStyleBackColor = true;
            
            // labelPlayers 
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.Location = new System.Drawing.Point(30, 110);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(90, 25);
            this.labelPlayers.TabIndex = 3;
            this.labelPlayers.Text = "Players:";
            
            // labelPlayerOne 
            this.labelPlayerOne.AutoSize = true;
            this.labelPlayerOne.Location = new System.Drawing.Point(78, 153);
            this.labelPlayerOne.Name = "labelPlayerOne";
            this.labelPlayerOne.Size = new System.Drawing.Size(97, 25);
            this.labelPlayerOne.TabIndex = 4;
            this.labelPlayerOne.Text = "Player 1:";
            
            // labelPlayerTwo 
            this.labelPlayerTwo.AutoSize = true;
            this.labelPlayerTwo.Location = new System.Drawing.Point(78, 194);
            this.labelPlayerTwo.Name = "labelPlayerTwo";
            this.labelPlayerTwo.Size = new System.Drawing.Size(97, 25);
            this.labelPlayerTwo.TabIndex = 5;
            this.labelPlayerTwo.Text = "Player 2:";
            
            // textBoxPlayerOneName 
            this.textBoxPlayerOneName.Location = new System.Drawing.Point(214, 147);
            this.textBoxPlayerOneName.Name = "textBoxPlayerOneName";
            this.textBoxPlayerOneName.Size = new System.Drawing.Size(100, 31);
            this.textBoxPlayerOneName.TabIndex = 6;
            
            // textBoxPlayerTwoName 
            this.textBoxPlayerTwoName.Enabled = false;
            this.textBoxPlayerTwoName.Location = new System.Drawing.Point(214, 194);
            this.textBoxPlayerTwoName.Name = "textBoxPlayerTwoName";
            this.textBoxPlayerTwoName.Size = new System.Drawing.Size(100, 31);
            this.textBoxPlayerTwoName.TabIndex = 7;
            this.textBoxPlayerTwoName.Text = "[Computer]";
            
            // checkBoxPlayerTwo 
            this.checkBoxPlayerTwo.AutoSize = true;
            this.checkBoxPlayerTwo.Location = new System.Drawing.Point(44, 192);
            this.checkBoxPlayerTwo.Name = "checkBoxPlayerTwo";
            this.checkBoxPlayerTwo.Size = new System.Drawing.Size(28, 27);
            this.checkBoxPlayerTwo.TabIndex = 8;
            this.checkBoxPlayerTwo.UseVisualStyleBackColor = true;
            this.checkBoxPlayerTwo.CheckedChanged += new System.EventHandler(this.checkBoxPlayerTwo_CheckedChanged);
            
            // radioButtonSize8 
            this.radioButtonSize8.AutoSize = true;
            this.radioButtonSize8.Location = new System.Drawing.Point(147, 60);
            this.radioButtonSize8.Name = "radioButtonSize8";
            this.radioButtonSize8.Size = new System.Drawing.Size(78, 29);
            this.radioButtonSize8.TabIndex = 9;
            this.radioButtonSize8.TabStop = true;
            this.radioButtonSize8.Text = "8x8";
            this.radioButtonSize8.UseVisualStyleBackColor = true;
            
            // radioButtonSize10 
            this.radioButtonSize10.AutoSize = true;
            this.radioButtonSize10.Location = new System.Drawing.Point(249, 60);
            this.radioButtonSize10.Name = "radioButtonSize10";
            this.radioButtonSize10.Size = new System.Drawing.Size(102, 29);
            this.radioButtonSize10.TabIndex = 10;
            this.radioButtonSize10.TabStop = true;
            this.radioButtonSize10.Text = "10x10";
            this.radioButtonSize10.UseVisualStyleBackColor = true;
            
            // SettingsForm 
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Controls.Add(this.radioButtonSize10);
            this.Controls.Add(this.radioButtonSize8);
            this.Controls.Add(this.checkBoxPlayerTwo);
            this.Controls.Add(this.textBoxPlayerTwoName);
            this.Controls.Add(this.textBoxPlayerOneName);
            this.Controls.Add(this.labelPlayerTwo);
            this.Controls.Add(this.labelPlayerOne);
            this.Controls.Add(this.labelPlayers);
            this.Controls.Add(this.radioButtonSize6);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.labelBoardSize);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Damka Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void checkBoxPlayerTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (textBoxPlayerTwoName.Enabled)
            {
                textBoxPlayerTwoName.Text = "[Computer]";
                textBoxPlayerTwoName.Enabled = false;
                m_AgainstComputer = true;
            }
            else
            {
                textBoxPlayerTwoName.Text = string.Empty;
                textBoxPlayerTwoName.Enabled = true;
                m_AgainstComputer = false;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            try
            {
                validateClose();
                m_PlayerOneName = textBoxPlayerOneName.Text;
                m_PlayerTwoName = textBoxPlayerTwoName.Text;
                m_AgainstComputer = !checkBoxPlayerTwo.Checked;
                if (radioButtonSize6.Checked)
                {
                    m_BoardSize = 6;
                }
                else if (radioButtonSize8.Checked)
                {
                    m_BoardSize = 8;
                }
                else
                {
                    m_BoardSize = 10;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Damka Settings", MessageBoxButtons.OK);
            }
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public string PlayerOneName
        {
            get { return m_PlayerOneName; }
        }

        public string PlayerTwoName
        {
            get { return m_PlayerTwoName; }
        }

        public bool AgainstComputer
        {
            get { return m_AgainstComputer; }
        }

        private void validateClose()
        {
            if (textBoxPlayerOneName.Text == string.Empty)
            {
                throw new Exception("Please enter player one name.");
            }

            if (checkBoxPlayerTwo.Checked && textBoxPlayerTwoName.Text == string.Empty)
            {
                throw new Exception("Please enter player two name.");
            }

            if (!radioButtonSize6.Checked && !radioButtonSize8.Checked && !radioButtonSize10.Checked)
            {
                throw new Exception("Please select the board size.");
            }
        }
    }
}
