using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace B18_Ex05_Logic
{
    public class Player
    {
        public enum ePlayerID
        {
            PlayerOne,
            PlayerTwo,
            Computer, 
            None
        }

        private string m_PlayerName;
        private int m_PlayerScore = 0;
        private int m_NumberOfInstrumentsPerRound;
        private int m_LastMovementLandingRow;
        private int m_LastMovementLandingCol;
        private ePlayerID m_PlayerID;
        private Board m_Board;
        private Player m_Rival;
        private eOptionalMoves m_LastMove;
        private ArtificialIntelligence m_ArtificialIntelligence;
        private MovementValidation m_MovementValidation;
        private bool m_ShouldCaptureAgain = false;

        internal Player(string i_Name, ePlayerID i_PlayerID, Board i_Board)
        {
            m_PlayerName = i_Name;
            m_PlayerID = i_PlayerID;
            m_Board = i_Board;
            m_LastMove = eOptionalMoves.InvalidMove;
            m_MovementValidation = new MovementValidation(this, m_Board);

            if (i_PlayerID == ePlayerID.Computer)
            {
                m_ArtificialIntelligence = new ArtificialIntelligence(this, m_Board);
            }
        }

        public int LastMovementLandingRow
        {
            get { return m_LastMovementLandingRow; }
        }

        public int LastMovementLandingCol
        {
            get { return m_LastMovementLandingCol; }
        }

        internal ArtificialIntelligence ArtificialIntelligence
        {
            get { return m_ArtificialIntelligence; }
        }

        public Player Rival
        {
            get { return m_Rival; }
            set { m_Rival = value; }
        }

        public ePlayerID PlayerID
        {
            get { return m_PlayerID; }
        }

        public eOptionalMoves LastMove
        {
            get { return m_LastMove; }
            set { m_LastMove = value; }
        }

        public bool ShouldCaptureAgain
        {
            get { return m_ShouldCaptureAgain; }
        }

        public string Name
        {
            get { return m_PlayerName; }
        }

        public int Score
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }

        internal int NumberOfInstrumentsPerRound
        {
            get { return m_NumberOfInstrumentsPerRound; }
            set { m_NumberOfInstrumentsPerRound = value; }
        }

        internal void ResetNumberOfInstrumentsPerRound(int i_BoardSize)
        {
            if (i_BoardSize == 6)
            {
                m_NumberOfInstrumentsPerRound = 6;
            }
            else if (i_BoardSize == 8)
            {
                m_NumberOfInstrumentsPerRound = 12;
            }
            else
            {
                m_NumberOfInstrumentsPerRound = 20;
            }
        }

        internal void MoveAfterEatValidation(int io_CurrentRow, int io_CurrentCol)
        {
            m_MovementValidation.MoveAfterEatLocations(io_CurrentRow, io_CurrentCol);
        }

        internal void ComputerMove()
        {
            m_ArtificialIntelligence.RegularComputerMove();
        }

        internal void RealPlayerMove(int io_RowFrom, int io_ColFrom, int io_RowTo, int io_ColTo)
        {
            eOptionalMoves validPlayerMove;

            try
            {
                validPlayerMove = m_MovementValidation.ValidPlayerMove(io_RowFrom, io_ColFrom, io_RowTo, io_ColTo);
                m_LastMove = validPlayerMove;
                m_LastMovementLandingRow = io_RowTo;
                m_LastMovementLandingCol = io_ColTo;
                MakeTheMove(io_RowFrom, io_ColFrom, io_RowTo, io_ColTo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal void MakeTheMove(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo)
        {
            int rowVictim = (i_RowFrom + i_RowTo) / 2;
            int colVictim = (i_ColFrom + i_ColTo) / 2;
            m_Board[i_RowTo, i_ColTo] = m_Board[i_RowFrom, i_ColFrom];
            m_Board[i_RowFrom, i_ColFrom] = eInstrumentType.Space;
            soldierToBeKing(i_RowTo, i_ColTo);

            if (i_RowFrom == i_RowTo + 2 || i_RowTo == i_RowFrom + 2)
            {
                // Eat rival instrument
                updateNumberOfInstrumentsPerRound(rowVictim, colVictim);
                m_Board[rowVictim, colVictim] = eInstrumentType.Space;
                if (m_MovementValidation.CheckIfCanEatWithSpecificIndex(i_RowTo, i_ColTo))
                {
                    m_ShouldCaptureAgain = true;

                    // can eat again with same instrument
                    switch (m_PlayerID)
                    {
                        case ePlayerID.Computer:
                            m_ArtificialIntelligence.ContinuesComputerMove(i_RowTo, i_ColTo);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    m_ShouldCaptureAgain = false;
                }
            }
        }

        internal bool CheckForValidMoves()
        {
            bool hasValidMove;

            if (m_PlayerID == ePlayerID.PlayerOne)
            {
                hasValidMove = m_MovementValidation.CheckForValidMovesForPlayerOne();
            }
            else
            {
                hasValidMove = m_MovementValidation.CheckForValidMovesForPlayerTwo();
            }

            return hasValidMove;
        }

        private void updateNumberOfInstrumentsPerRound(int i_RowVictim, int i_ColVictim)
        {
            if (m_PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[i_RowVictim, i_ColVictim] == eInstrumentType.PlayerTwoSoldier)
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 1;
                }
                else
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 4;
                }
            }
            else
            {
                if (m_Board[i_RowVictim, i_ColVictim] == eInstrumentType.PlayerOneSoldier)
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 1;
                }
                else
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 4;
                }
            }
        }

        private void soldierToBeKing(int i_Row, int i_Col) // gets the location after the movement !!! 
        {
            if ((i_Row == 0) && (m_Board[i_Row, i_Col] == eInstrumentType.PlayerOneSoldier))
            {
                m_Board[i_Row, i_Col] = eInstrumentType.PlayerOneKing;
                m_NumberOfInstrumentsPerRound += 3;
            }
            else if ((i_Row == m_Board.Size - 1) && (m_Board[i_Row, i_Col] == eInstrumentType.PlayerTwoSoldier))
            {
                m_Board[i_Row, i_Col] = eInstrumentType.PlayerTwoKing;
                m_NumberOfInstrumentsPerRound += 3;
            }
        }
    }
}
