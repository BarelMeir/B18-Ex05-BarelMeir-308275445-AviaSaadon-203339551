using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex05_Logic
{
    public class MovementValidation
    {
        private Player m_Player;
        private Board m_Board;        

        internal MovementValidation(Player i_Player, Board i_Board)
        {
            m_Player = i_Player;
            m_Board = i_Board;
        }

        internal bool MoveAfterEatLocations(int i_RowFrom, int i_ColFrom)
        {
            if (m_Player.LastMovementLandingRow != i_RowFrom || m_Player.LastMovementLandingCol != i_ColFrom)
            {
                throw new Exception("You have to move with the instrument you ate with.");
            }

            return true;
        }

        internal eOptionalMoves ValidPlayerMove(int io_RowFrom, int io_ColFrom, int io_RowTo, int io_ColTo)
        {
            try
            {
                IsValidMoveAsPlayer(io_RowFrom, io_ColFrom, io_RowTo, io_ColTo); 
                return ConvertToEnumMove(io_RowFrom, io_RowTo, io_ColFrom, io_ColTo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal bool IsValidMoveAsPlayer(int io_RowFrom, int io_ColFrom, int io_RowTo, int io_ColTo)
        {
            bool isValidMoveResult = true;

            eInstrumentType instrumentTypeFrom = m_Board[io_RowFrom, io_ColFrom];
            eOptionalMoves requiredMove = ConvertToEnumMove(io_RowFrom, io_RowTo, io_ColFrom, io_ColTo);

            if (requiredMove == eOptionalMoves.InvalidMove)
            {
                isValidMoveResult = false;
                throw new Exception("Invalid move. Please try again.");
            }
            else if (!ValidMoveByType(io_RowFrom, io_ColFrom, requiredMove))
            {
                isValidMoveResult = false;
                throw new Exception("Invalid move. Please try again.");
            }
            else if (!IsCorrectPlayerIsInCell(instrumentTypeFrom))
            {
                isValidMoveResult = false;
                throw new Exception("Invalid move, it is not your instrument. Please try again.");
            }
            else if (!ValidInstrumentDirection(instrumentTypeFrom, requiredMove))
            {
                isValidMoveResult = false;
                throw new Exception("Invalid move, a soldier can go only forward. Please try again.");
            }
            else if (CheckIfCanEat())
            {
                if (requiredMove != eOptionalMoves.EatDownLeft && requiredMove != eOptionalMoves.EatDownRight &&
                    requiredMove != eOptionalMoves.EatUpLeft && requiredMove != eOptionalMoves.EatUpRight)
                {
                    isValidMoveResult = false;
                    throw new Exception("Invalid move, you should make a capture. Please try again.");
                }
            }

            return isValidMoveResult;
        }

        internal eOptionalMoves ConvertToEnumMove(int i_RowFrom, int i_RowTo, int i_ColFrom, int i_ColTo)
        {
            eOptionalMoves requiredMove = eOptionalMoves.InvalidMove;

            if (i_ColFrom == i_ColTo + 2 && i_RowFrom == i_RowTo + 2)
            {
                requiredMove = eOptionalMoves.EatUpLeft;
            }
            else if (i_ColFrom == i_ColTo + 1 && i_RowFrom == i_RowTo + 1)
            {
                requiredMove = eOptionalMoves.MoveUpLeft;
            }
            else if (i_ColFrom == i_ColTo - 2 && i_RowFrom == i_RowTo + 2)
            {
                requiredMove = eOptionalMoves.EatUpRight;
            }
            else if (i_ColFrom == i_ColTo - 1 && i_RowFrom == i_RowTo + 1)
            {
                requiredMove = eOptionalMoves.MoveUpRight;
            }
            else if (i_ColFrom == i_ColTo + 2 && i_RowFrom == i_RowTo - 2)
            {
                requiredMove = eOptionalMoves.EatDownLeft;
            }
            else if (i_ColFrom == i_ColTo + 1 && i_RowFrom == i_RowTo - 1)
            {
                requiredMove = eOptionalMoves.MoveDownLeft;
            }
            else if (i_ColFrom == i_ColTo - 2 && i_RowFrom == i_RowTo - 2)
            {
                requiredMove = eOptionalMoves.EatDownRight;
            }
            else if (i_ColFrom == i_ColTo - 1 && i_RowFrom == i_RowTo - 1)
            {
                requiredMove = eOptionalMoves.MoveDownRight;
            }

            return requiredMove;
        }

        internal bool IsCorrectPlayerIsInCell(eInstrumentType i_InstrumentTypeInCell)
        {
            bool isCorrectPlayerInCell = false;

            if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                isCorrectPlayerInCell = i_InstrumentTypeInCell == eInstrumentType.PlayerOneSoldier ||
                                        i_InstrumentTypeInCell == eInstrumentType.PlayerOneKing;
            }
            else
            {
                isCorrectPlayerInCell = i_InstrumentTypeInCell == eInstrumentType.PlayerTwoSoldier ||
                                        i_InstrumentTypeInCell == eInstrumentType.PlayerTwoKing;
            }

            return isCorrectPlayerInCell;
        }

        internal bool ValidInstrumentDirection(eInstrumentType i_InstrumentTypeFrom, eOptionalMoves i_RequiredMove)
        {
            bool isValidMove = true;

            if (i_InstrumentTypeFrom == eInstrumentType.PlayerOneSoldier)
            {
                isValidMove = i_RequiredMove == eOptionalMoves.EatUpLeft ||
                              i_RequiredMove == eOptionalMoves.EatUpRight ||
                              i_RequiredMove == eOptionalMoves.MoveUpLeft ||
                              i_RequiredMove == eOptionalMoves.MoveUpRight;
            }
            else if (i_InstrumentTypeFrom == eInstrumentType.PlayerTwoSoldier)
            {
                isValidMove = i_RequiredMove == eOptionalMoves.EatDownLeft ||
                              i_RequiredMove == eOptionalMoves.EatDownRight ||
                              i_RequiredMove == eOptionalMoves.MoveDownLeft ||
                              i_RequiredMove == eOptionalMoves.MoveDownRight;
            }

            return isValidMove;
        }

        internal bool CheckForValidMovesForPlayerOne()
        {
            bool hasValidMove = false;

            for (int i = 0; (i < m_Board.Size) && (!hasValidMove); i++)
            {
                for (int j = 0; (j < m_Board.Size) && (!hasValidMove); j++)
                {
                    hasValidMove = ChekForValidMovesForPlayerOneAtIndexes(i, j);
                }
            }

            return hasValidMove;
        }

        internal bool ChekForValidMovesForPlayerOneAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case eInstrumentType.PlayerOneSoldier:
                    hasValidMove = MoveUpRight(i_Row, i_Col) || MoveUpLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                case eInstrumentType.PlayerOneKing:
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) || MoveUpRight(i_Row, i_Col) ||
                                   MoveUpLeft(i_Row, i_Col) || EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool ChekForValidEatMovesForPlayerOneAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case eInstrumentType.PlayerOneSoldier:
                    hasValidMove = EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                case eInstrumentType.PlayerOneKing:
                    hasValidMove = EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool CheckForValidMovesForPlayerTwo()
        {
            bool hasValidMove = false;

            for (int i = 0; i < m_Board.Size && !hasValidMove; i++)
            {
                for (int j = 0; j < m_Board.Size && !hasValidMove; j++)
                {
                    hasValidMove = CheckForValidMovesForPlayerTwoAtIndexes(i, j);
                }
            }

            return hasValidMove;
        }

        internal bool CheckForValidMovesForPlayerTwoAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case eInstrumentType.PlayerTwoSoldier:
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col);
                    break;
                case eInstrumentType.PlayerTwoKing:
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) ||
                                   MoveUpRight(i_Row, i_Col) || MoveUpLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool ChekForValidEatMovesForPlayerTwoAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case eInstrumentType.PlayerTwoSoldier:
                    hasValidMove = EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col);
                    break;
                case eInstrumentType.PlayerTwoKing:
                    hasValidMove = EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool CheckIfCanEat()
        {
            bool canEat = false;

            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
                    {
                        canEat = canEat || ChekForValidEatMovesForPlayerOneAtIndexes(i, j);
                    }
                    else if (m_Player.PlayerID == Player.ePlayerID.PlayerTwo)
                    {
                        canEat = canEat || ChekForValidEatMovesForPlayerTwoAtIndexes(i, j);
                    }
                }
            }

            return canEat;
        }

        internal bool CheckIfCanEatWithSpecificIndex(int i_Row, int i_Col)
        {
            bool canEat;

            if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                canEat = ChekForValidEatMovesForPlayerOneAtIndexes(i_Row, i_Col);
            }
            else
            {
                canEat = ChekForValidEatMovesForPlayerTwoAtIndexes(i_Row, i_Col);
            }

            return canEat;
        }

        internal bool ValidMoveByType(int i_Row, int i_Col, eOptionalMoves io_MoveType)
        {
            bool validByMoveType;

            switch (io_MoveType)
            {
                case eOptionalMoves.MoveDownRight:
                    validByMoveType = MoveDownRight(i_Row, i_Col);
                    break;
                case eOptionalMoves.MoveDownLeft:
                    validByMoveType = MoveDownLeft(i_Row, i_Col);
                    break;
                case eOptionalMoves.MoveUpRight:
                    validByMoveType = MoveUpRight(i_Row, i_Col);
                    break;
                case eOptionalMoves.MoveUpLeft:
                    validByMoveType = MoveUpLeft(i_Row, i_Col);
                    break;
                case eOptionalMoves.EatDownRight:
                    validByMoveType = EatDownRight(i_Row, i_Col);
                    break;
                case eOptionalMoves.EatDownLeft:
                    validByMoveType = EatDownLeft(i_Row, i_Col);
                    break;
                case eOptionalMoves.EatUpRight:
                    validByMoveType = EatUpRight(i_Row, i_Col);
                    break;
                case eOptionalMoves.EatUpLeft:
                    validByMoveType = EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    validByMoveType = false;
                    break;
            }

            return validByMoveType;
        }

        internal bool MoveDownRight(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row + 1) >= m_Board.Size || (i_Col + 1) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 1, i_Col + 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool MoveDownLeft(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row + 1) >= m_Board.Size || (i_Col - 1) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 1, i_Col - 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool EatDownRight(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row + 1;
            int victimCol = i_Col + 1;

            if ((i_Row + 2) >= m_Board.Size || (i_Col + 2) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 2, i_Col + 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoSoldier;
            }
            else
            {
                // computer or player two
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneSoldier;
            }

            return validMove;
        }

        internal bool EatDownLeft(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row + 1;
            int victimCol = i_Col - 1;

            if ((i_Row + 2) >= m_Board.Size || (i_Col - 2) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 2, i_Col - 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoSoldier;
            }
            else
            {
                // computer or player two
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneSoldier;
            }

            return validMove;
        }

        internal bool MoveUpRight(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row - 1) < 0 || (i_Col + 1) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 1, i_Col + 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool MoveUpLeft(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row - 1) < 0 || (i_Col - 1) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 1, i_Col - 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool EatUpRight(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row - 1;
            int victimCol = i_Col + 1;

            if ((i_Row - 2) < 0 || (i_Col + 2) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 2, i_Col + 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoSoldier;
            }
            else
            {
                // computer or player two
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneSoldier;
            }

            return validMove;
        }

        internal bool EatUpLeft(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row - 1;
            int victimCol = i_Col - 1;

            if ((i_Row - 2) < 0 || (i_Col - 2) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 2, i_Col - 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == Player.ePlayerID.PlayerOne)
            {
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerTwoSoldier;
            }
            else
            {
                // computer or player two
                validMove = m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneKing ||
                            m_Board[victimRow, victimCol] == eInstrumentType.PlayerOneSoldier;
            }

            return validMove;
        }
    }
}
