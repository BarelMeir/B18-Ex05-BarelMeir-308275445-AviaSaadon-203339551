using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Policy;
using B18_Ex05_Logic;

namespace B18_Ex05_UserInterface
{
    internal class UIForm : Form
    {
        private const int m_LeftOffset = 20;
        private const int m_TopOffset = 50;
        private const int m_ButtonSize = 75;
        private InstrumentButton[,] m_Board;
        private GameHandler m_GameHandler;
        private int m_BoardSize;
        private InstrumentButton m_PreviousPressedButton;
        private InstrumentButton m_CurrentPressedButton;
        private Label labelPlayerOneScore;
        private Label labelPlayerTwoScore;
        private SettingsForm m_SettingsForm = new SettingsForm();
        private Point m_FirstButtonLocation;
        private bool m_EndOfRound;
        private Timer m_AnimationTimer = new Timer();
        private PictureBox m_AnimationPictureBox = new PictureBox();
        private eOptionalMoves m_AnimationMove;
        private Point m_AnimationStopPoint = new Point();

        public UIForm()
        {
            Text = "Damka";
            m_SettingsForm.ShowDialog();
            while (m_SettingsForm.DialogResult != DialogResult.OK)
            {
                MessageBox.Show("Please fill the required settings.", "Damka Settings", MessageBoxButtons.OK);
                m_SettingsForm.ShowDialog();
            }

            m_BoardSize = m_SettingsForm.BoardSize;
            m_GameHandler = new GameHandler(m_BoardSize, m_SettingsForm.PlayerOneName, m_SettingsForm.AgainstComputer, m_SettingsForm.PlayerTwoName);
            m_FirstButtonLocation = new Point(this.Location.X + m_LeftOffset, this.Location.Y + m_TopOffset);
            this.Size = new Size((m_BoardSize * m_ButtonSize) + (3 * m_LeftOffset), (m_BoardSize * m_ButtonSize) + (2 * m_TopOffset));
            initializeComponent();
            m_AnimationTimer.Interval = 10;
            m_AnimationTimer.Tick += new EventHandler(m_AnimationTimer_Tick);
            setBoard();
            setNewRound();
        }

        private void initializeComponent()
        {
            this.labelPlayerOneScore = new Label();
            this.labelPlayerTwoScore = new Label();
            this.SuspendLayout();

            // labelPlayerOneScore
            this.labelPlayerOneScore.AutoSize = true;
            this.labelPlayerOneScore.Location = new Point(this.Width / 5, m_TopOffset / 2);
            this.labelPlayerOneScore.Name = "labelPlayerOneScore";
            this.labelPlayerOneScore.TabIndex = 0;

            // labelPlayerTwoScore
            this.labelPlayerTwoScore.AutoSize = true;
            this.labelPlayerTwoScore.Location = new Point(this.Width * 3 / 5, labelPlayerOneScore.Location.Y);
            this.labelPlayerTwoScore.Name = "labelPlayerTwoScore";
            this.labelPlayerTwoScore.TabIndex = 1;

            // UIForm
            this.Controls.Add(this.labelPlayerOneScore);
            this.Controls.Add(this.labelPlayerTwoScore);
            this.MaximizeBox = false;
            this.Name = "UIForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Controls.Add(m_AnimationPictureBox);
            updateLabels();
        }

        private void animate(InstrumentButton i_OriginButton)
        {
            m_AnimationTimer.Start();
            Image animationImage = i_OriginButton.Image;
            i_OriginButton.Image = null;
            m_AnimationPictureBox.Visible = true;
            m_AnimationPictureBox.BackgroundImage = null;
            calcualteStopLocation(i_OriginButton.Location);
            m_AnimationPictureBox.Location = i_OriginButton.Location;
            m_AnimationPictureBox.Size = new Size(m_ButtonSize, m_ButtonSize);
            m_AnimationPictureBox.Image = animationImage;
        }

        private void calcualteStopLocation(Point i_OriginLocation)
        {
            int offset = (int) Math.Sqrt(m_ButtonSize * m_ButtonSize);
            
            switch (m_AnimationMove)
            {
                case eOptionalMoves.MoveDownRight:
                    m_AnimationStopPoint.X = i_OriginLocation.X + offset;
                    m_AnimationStopPoint.Y = i_OriginLocation.Y + offset;
                    break;
                case eOptionalMoves.MoveDownLeft:
                    m_AnimationStopPoint.X = i_OriginLocation.X - offset;
                    m_AnimationStopPoint.Y = i_OriginLocation.Y + offset;
                    break;
                case eOptionalMoves.MoveUpRight:
                    m_AnimationStopPoint.X = i_OriginLocation.X + offset;
                    m_AnimationStopPoint.Y = i_OriginLocation.Y - offset;
                    break;
                case eOptionalMoves.MoveUpLeft:
                    m_AnimationStopPoint.X = i_OriginLocation.X - offset;
                    m_AnimationStopPoint.Y = i_OriginLocation.Y - offset;
                    break;
                case eOptionalMoves.EatDownRight:
                    m_AnimationStopPoint.X = i_OriginLocation.X + (2 * offset);
                    m_AnimationStopPoint.Y = i_OriginLocation.Y + (2 * offset);
                    break;
                case eOptionalMoves.EatDownLeft:
                    m_AnimationStopPoint.X = i_OriginLocation.X - (2 * offset);
                    m_AnimationStopPoint.Y = i_OriginLocation.Y + (2 * offset);
                    break;
                case eOptionalMoves.EatUpRight:
                    m_AnimationStopPoint.X = i_OriginLocation.X + (2 * offset);
                    m_AnimationStopPoint.Y = i_OriginLocation.Y - (2 * offset);
                    break;
                case eOptionalMoves.EatUpLeft:
                    m_AnimationStopPoint.X = i_OriginLocation.X - (2 * offset);
                    m_AnimationStopPoint.Y = i_OriginLocation.Y - (2 * offset);
                    break;
            }
        }

        private void m_AnimationTimer_Tick(object sender, EventArgs e)
        {
            int animationSpeed = 2;

            switch (m_AnimationMove)
            {
                case eOptionalMoves.EatDownRight:
                case eOptionalMoves.MoveDownRight:
                    m_AnimationPictureBox.Top += animationSpeed;
                    m_AnimationPictureBox.Left += animationSpeed;
                    break;
                case eOptionalMoves.EatDownLeft:
                case eOptionalMoves.MoveDownLeft:
                    m_AnimationPictureBox.Top += animationSpeed;
                    m_AnimationPictureBox.Left -= animationSpeed;
                    break;
                case eOptionalMoves.EatUpRight:
                case eOptionalMoves.MoveUpRight:
                    m_AnimationPictureBox.Top -= animationSpeed;
                    m_AnimationPictureBox.Left += animationSpeed;
                    break;
                case eOptionalMoves.EatUpLeft:
                case eOptionalMoves.MoveUpLeft:
                    m_AnimationPictureBox.Top -= animationSpeed;
                    m_AnimationPictureBox.Left -= animationSpeed;
                    break;
            }

            // stop the timer when arrived to location
            checkForTimerStop();
        }

        private void checkForTimerStop()
        {
            switch (m_AnimationMove)
            {
                case eOptionalMoves.EatDownRight:
                case eOptionalMoves.MoveDownRight:
                    if (m_AnimationPictureBox.Location.X >= m_AnimationStopPoint.X || m_AnimationPictureBox.Location.Y >= m_AnimationStopPoint.Y)
                    {
                        stopAnimation();
                    }

                    break;
                case eOptionalMoves.EatDownLeft:
                case eOptionalMoves.MoveDownLeft:
                    if (m_AnimationPictureBox.Location.X <= m_AnimationStopPoint.X || m_AnimationPictureBox.Location.Y >= m_AnimationStopPoint.Y)
                    {
                        stopAnimation();
                    }

                    break;
                case eOptionalMoves.EatUpRight:
                case eOptionalMoves.MoveUpRight:
                    if (m_AnimationPictureBox.Location.X >= m_AnimationStopPoint.X || m_AnimationPictureBox.Location.Y <= m_AnimationStopPoint.Y)
                    {
                        stopAnimation();
                    }

                    break;
                case eOptionalMoves.EatUpLeft:
                case eOptionalMoves.MoveUpLeft:
                    if (m_AnimationPictureBox.Location.X <= m_AnimationStopPoint.X || m_AnimationPictureBox.Location.Y <= m_AnimationStopPoint.Y)
                    {
                        stopAnimation();
                    }

                    break;
            }
        }

        private void stopAnimation()
        {
            m_AnimationTimer.Stop();
            m_AnimationPictureBox.Visible = false;
            endCurrentTurn();
        }

        private void setBoard()
        {
            m_Board = new InstrumentButton[m_BoardSize, m_BoardSize];
            Point nextButtonLocation = m_FirstButtonLocation;
            bool isEnabled = false;

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    m_Board[i, j] = createButton(nextButtonLocation, isEnabled, i, j);
                    nextButtonLocation.X += m_ButtonSize;
                    isEnabled = !isEnabled;
                }

                isEnabled = !isEnabled;
                nextButtonLocation.X = m_FirstButtonLocation.X;
                nextButtonLocation.Y += m_ButtonSize;
            }
        }

        private InstrumentButton createButton(Point i_Location, bool i_isEnabled, int i_RowIndex, int i_ColIndex)
        {
            InstrumentButton button = new InstrumentButton();

            button.RowIndex = i_RowIndex;
            button.ColIndex = i_ColIndex;
            button.Height = m_ButtonSize;
            button.Width = m_ButtonSize;
            button.Location = new Point(i_Location.X, i_Location.Y);
            button.Enabled = i_isEnabled;
            if (!i_isEnabled)
            {
                button.BackColor = Color.Gray;
            }

            this.Controls.Add(button);
            button.Click += new System.EventHandler(this.button_Click);

            return button;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (m_PreviousPressedButton == null)
            {
                (sender as InstrumentButton).BackColor = Color.CornflowerBlue;
                m_PreviousPressedButton = sender as InstrumentButton;
            }
            else if ((sender as InstrumentButton).Equals(m_PreviousPressedButton))
            {
                m_PreviousPressedButton = null;
                m_CurrentPressedButton = null;
                (sender as InstrumentButton).BackColor = Color.Empty;
            }
            else
            {
                m_CurrentPressedButton = sender as InstrumentButton;
                tryToMakeMove();
            }
        }

        private void tryToMakeMove()
        {
            int colMoveFrom = m_PreviousPressedButton.ColIndex;
            int rowMoveFrom = m_PreviousPressedButton.RowIndex;
            int colMoveTo = m_CurrentPressedButton.ColIndex;
            int rowMoveTo = m_CurrentPressedButton.RowIndex;
            try
            {
                m_GameHandler.TryToMakeMove(rowMoveFrom, colMoveFrom, rowMoveTo, colMoveTo);
                m_AnimationMove = m_GameHandler.LastMove();
                animate(m_Board[rowMoveFrom, colMoveFrom]);
                m_CurrentPressedButton = null;
                m_PreviousPressedButton = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Damka", MessageBoxButtons.OK);
                m_CurrentPressedButton = null;
                m_PreviousPressedButton = null;
                updateDisplay();
            }
        }

        private void tryComputerMove()
        {
            if (m_GameHandler.CurrentTurnPlayerID == Player.ePlayerID.PlayerTwo && m_GameHandler.PlayerTwo.PlayerID == Player.ePlayerID.Computer)
            {
                if (!m_GameHandler.EndOfRound)
                {
                    m_GameHandler.ComputerMove();
                    m_AnimationMove = m_GameHandler.LastMove();
                    animate(m_Board[m_GameHandler.ComputerCurrentMoveRowOrigin(), m_GameHandler.ComputerCurrentMoveColOrigin()]);
                }
            }
        }

        private void endCurrentRound()
        {
            Player.ePlayerID winner = m_GameHandler.CurrentRoundWinner;
            DialogResult continueGame;
            string winnerName;

            m_EndOfRound = true;
            m_GameHandler.UpdateScores();
            switch (winner)
            {
                case Player.ePlayerID.PlayerOne:
                    winnerName = string.Format(
                        @"{0} Won!
Another round?",
m_GameHandler.PlayerOne.Name);
                    break;
                case Player.ePlayerID.PlayerTwo:
                    winnerName = string.Format(
                        @"{0} Won!
Another round?",
m_GameHandler.PlayerTwo.Name); 
                    break;
                default:
                    winnerName = string.Format(@"Tie!
Another round?");
                    break;
            }

            continueGame = MessageBox.Show(winnerName, "Damka", MessageBoxButtons.YesNo);
            switch (continueGame)
            {
                case DialogResult.Yes:
                    setNewRound();
                    break;
                case DialogResult.No:
                    this.Close();
                    break;
            }
        }

        private void setNewRound()
        {
            m_EndOfRound = false;
            updateLabels();
            m_GameHandler.SetNewRound();
            updateDisplay();
        }

        private void updateDisplay()
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    switch (m_GameHandler[i, j])
                    {
                        case eInstrumentType.PlayerOneSoldier:
                            m_Board[i, j].Image = (Image)(new Bitmap(B18_Ex05_UserInterface.Properties.Resources.PlayerOneSoldier, new Size(m_ButtonSize, m_ButtonSize)));
                            break;
                        case eInstrumentType.PlayerOneKing:
                            m_Board[i, j].Image = (Image)(new Bitmap(B18_Ex05_UserInterface.Properties.Resources.PlayerOneKing, new Size(m_ButtonSize, m_ButtonSize)));
                            break;
                        case eInstrumentType.PlayerTwoSoldier:
                            m_Board[i, j].Image = (Image)(new Bitmap(B18_Ex05_UserInterface.Properties.Resources.PlayerTwoSoldier, new Size(m_ButtonSize, m_ButtonSize)));
                            break;
                        case eInstrumentType.PlayerTwoKing:
                            m_Board[i, j].Image = (Image)(new Bitmap(B18_Ex05_UserInterface.Properties.Resources.PlayerTwoKing, new Size(m_ButtonSize, m_ButtonSize)));
                            break;
                        default:
                            m_Board[i, j].Image = null;
                            break;
                    }

                    if (m_Board[i, j].Enabled)
                    {
                        m_Board[i, j].BackColor = Color.WhiteSmoke;
                    }
                    else
                    {
                        m_Board[i, j].BackColor = Color.Gray;
                    }
                }
            }
        }

        private void endCurrentTurn()
        {
            updateDisplay();
            tryComputerMove();
            if (m_GameHandler.EndOfRound && !m_EndOfRound)
            {
                endCurrentRound();
            }
        } 

        private void updateLabels()
        {
            labelPlayerOneScore.Text =
                string.Format("{0}: {1}", m_GameHandler.PlayerOne.Name, m_GameHandler.PlayerOne.Score);
            labelPlayerTwoScore.Text =
                string.Format("{0}: {1}", m_GameHandler.PlayerTwo.Name, m_GameHandler.PlayerTwo.Score);
        }
    }
}