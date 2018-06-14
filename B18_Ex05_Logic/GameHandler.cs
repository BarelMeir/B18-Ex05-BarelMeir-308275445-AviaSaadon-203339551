using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace B18_Ex05_Logic
{
    public class GameHandler
    {
        private Board m_Board;
        private Player m_PlayerOne; // can be  person only
        private Player m_PlayerTwo; // can be computer or a person
        private Player.ePlayerID m_CurrentTurnPlayerID;
        private Player.ePlayerID m_PreviousTurnPlayerID;
        private Player.ePlayerID m_CurrentRoundWinner;
        private bool m_EndOfRound;

        public GameHandler(int io_BoardSize, string io_PlayerOneName, bool io_isAgainstComputer, string io_PlayerTwoName)
        {
            m_Board = new Board(io_BoardSize);
            m_PlayerOne = new Player(io_PlayerOneName, Player.ePlayerID.PlayerOne, m_Board);

            if (io_isAgainstComputer)
            {
                m_PlayerTwo = new Player("Computer", Player.ePlayerID.Computer, m_Board);
            }
            else
            {
                m_PlayerTwo = new Player(io_PlayerTwoName, Player.ePlayerID.PlayerTwo, m_Board);
            }

            m_PlayerOne.Rival = m_PlayerTwo;
            m_PlayerTwo.Rival = m_PlayerOne;
            SetNewRound();
        }

        public Player PlayerOne
        {
            get { return m_PlayerOne; }
        }

        public Player PlayerTwo
        {
            get { return m_PlayerTwo; }
        }

        public bool EndOfRound
        {
            get { return m_EndOfRound; }
        }

        public Player.ePlayerID CurrentTurnPlayerID
        {
            get { return m_CurrentTurnPlayerID; }
        }

        public Player.ePlayerID CurrentRoundWinner
        {
            get { return m_CurrentRoundWinner; }
        }

        public eInstrumentType this[int i, int j]
        {
            get { return m_Board[i, j]; }
        }

        public void TryToMakeMove(int io_RowFrom, int io_ColFrom, int io_RowTo, int io_ColTo)
        {
            if (m_CurrentTurnPlayerID == Player.ePlayerID.PlayerOne)
            {
                try
                {
                    if (m_PreviousTurnPlayerID == Player.ePlayerID.PlayerOne)
                    {
                        m_PlayerOne.MoveAfterEatValidation(io_RowFrom, io_ColFrom);
                    }

                    m_PlayerOne.RealPlayerMove(io_RowFrom, io_ColFrom, io_RowTo, io_ColTo);
                    switchTurn(m_PlayerOne);
                    checkEndOfRound();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                try
                {
                    if (m_PreviousTurnPlayerID == Player.ePlayerID.PlayerTwo)
                    {
                        m_PlayerTwo.MoveAfterEatValidation(io_RowFrom, io_ColFrom);
                    }

                    m_PlayerTwo.RealPlayerMove(io_RowFrom, io_ColFrom, io_RowTo, io_ColTo);
                    switchTurn(m_PlayerTwo);
                    checkEndOfRound();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void switchTurn(Player i_CurrentPlayerTurn)
        {
            if (i_CurrentPlayerTurn == PlayerOne)
            {
                m_PreviousTurnPlayerID = Player.ePlayerID.PlayerOne;
                m_CurrentTurnPlayerID = Player.ePlayerID.PlayerTwo;
                if (i_CurrentPlayerTurn.ShouldCaptureAgain)
                {
                    m_CurrentTurnPlayerID = Player.ePlayerID.PlayerOne;
                }
            }
            else
            {
                m_PreviousTurnPlayerID = Player.ePlayerID.PlayerTwo;
                m_CurrentTurnPlayerID = Player.ePlayerID.PlayerOne;
                if (i_CurrentPlayerTurn.ShouldCaptureAgain)
                {
                    m_CurrentTurnPlayerID = Player.ePlayerID.PlayerTwo;
                }
            }
        }

        public void SetNewRound()
        {
            m_EndOfRound = false;
            m_CurrentTurnPlayerID = Player.ePlayerID.PlayerOne;
            m_PreviousTurnPlayerID = Player.ePlayerID.None;
            m_Board.SetPlayersOnBoard();
            m_PlayerOne.ResetNumberOfInstrumentsPerRound(m_Board.Size);
            m_PlayerTwo.ResetNumberOfInstrumentsPerRound(m_Board.Size);
            m_PlayerOne.LastMove = eOptionalMoves.InvalidMove;
            m_PlayerTwo.LastMove = eOptionalMoves.InvalidMove;
        }

        public void ComputerMove()
        {
            m_PlayerTwo.ComputerMove();
            switchTurn(m_PlayerTwo);
            checkEndOfRound();
        }

        public void UpdateScores()
        {
            if (m_CurrentRoundWinner == Player.ePlayerID.None)
            {
                return;
            }
            else if (m_CurrentRoundWinner == Player.ePlayerID.PlayerOne)
            {
                m_PlayerOne.Score += m_PlayerOne.NumberOfInstrumentsPerRound - m_PlayerTwo.NumberOfInstrumentsPerRound;
            }
            else
            {
                m_PlayerTwo.Score += m_PlayerTwo.NumberOfInstrumentsPerRound - m_PlayerOne.NumberOfInstrumentsPerRound;
            }
        }

        private void checkEndOfRound()
        {
            bool endRound = false;

            if (!m_PlayerOne.CheckForValidMoves() && !m_PlayerTwo.CheckForValidMoves())
            {
                endRound = true;
                m_CurrentRoundWinner = Player.ePlayerID.None;
            }
            else if (m_PlayerOne.NumberOfInstrumentsPerRound <= 0 || !m_PlayerOne.CheckForValidMoves())
            {
                endRound = true;
                m_CurrentRoundWinner = Player.ePlayerID.PlayerTwo;
            }
            else if (m_PlayerTwo.NumberOfInstrumentsPerRound <= 0 || !m_PlayerTwo.CheckForValidMoves())
            {
                endRound = true;
                m_CurrentRoundWinner = Player.ePlayerID.PlayerOne;
            }

            m_EndOfRound = endRound;
        }

        public eOptionalMoves LastMove()
        {
            eOptionalMoves lastMove;

            switch (m_PreviousTurnPlayerID)
            {
                case Player.ePlayerID.PlayerOne:
                    lastMove = m_PlayerOne.LastMove;
                    break;
                default: 
                    lastMove = m_PlayerTwo.LastMove;
                    break;
            }

            return lastMove;
        }

        public int ComputerCurrentMoveRowOrigin()
        {
            return m_PlayerTwo.ArtificialIntelligence.CurrentMoveRowOrigin;
        }

        public int ComputerCurrentMoveColOrigin()
        {
            return m_PlayerTwo.ArtificialIntelligence.CurrentMoveColOrigin;
        }
    }
}
