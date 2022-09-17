using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper_mcassin
{
    internal class Cell
    {
        public bool isVisible;
        public bool isMine;
        public bool IsFlagged;
        public int xPos;
        public int yPos;
        public int numMinesTouching;

        public Cell(int x, int y)
        {
            isVisible = false;
            xPos = x;
            yPos = y;
        }

    }
}