using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex05_Logic
{
    public class Board
    {
        private int m_Size;
        private eInstrumentType[,] m_BoardMatrix;

        internal Board(int i_BoardSize)
        {
            m_Size = i_BoardSize;
            m_BoardMatrix = new eInstrumentType[m_Size, m_Size];
        }

        internal int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        internal eInstrumentType this[int i_Row, int i_Col]
        {
            get { return m_BoardMatrix[i_Row, i_Col]; }
            set { m_BoardMatrix[i_Row, i_Col] = value; }
        }

        internal void SetPlayersOnBoard()
        {
            // upper part (O) = Player Two
            for (int i = 0; i < ((m_Size / 2) - 1); i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                    else if (isEven(i) && !isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerTwoSoldier;
                    }
                    else if (!isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerTwoSoldier;
                    }
                    else
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                }
            }

            // two rows of spaces
            for (int i = (m_Size / 2) - 1; i < (m_Size / 2) + 1; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_BoardMatrix[i, j] = eInstrumentType.Space;
                }
            }

            // buttom part (X) = Player One
            for (int i = (m_Size / 2) + 1; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                    else if (isEven(i) && !isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerOneSoldier;
                    }
                    else if (!isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerOneSoldier;
                    }
                    else
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                }
            }
        }

        private static bool isEven(int i_number)
        {
            bool isEvenNumber = i_number % 2 == 0;

            return isEvenNumber;
        }
    }
}
