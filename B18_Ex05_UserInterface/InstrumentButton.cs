using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using B18_Ex05_Logic;

namespace B18_Ex05_UserInterface
{
    internal class InstrumentButton : Button
    {
        private int m_RowIndex;
        private int m_ColIndex;

        internal int RowIndex
        {
            get { return m_RowIndex; }
            set { m_RowIndex = value; }
        }

        internal int ColIndex
        {
            get { return m_ColIndex; }
            set { m_ColIndex = value; }
        }
    }
}
