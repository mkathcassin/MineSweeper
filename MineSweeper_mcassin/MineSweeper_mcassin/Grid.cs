using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper_mcassin
{
    internal class Grid
    {
        public int XGridSize;
        public int YGridSize;
        public int NumMines = 10;
        public Cell[,] GridCells;

        public Grid()
        {
            GridCells = new Cell[XGridSize, YGridSize];
            //this could probably be done in a cooler way?
            for(int i = 0; i < XGridSize; i++)
            {
                for(int j = 0; j < YGridSize; j++)
                {
                    GridCells[i, j] = new Cell(i, j);
                }
            }
            randomizeMineLocation();
        }

        private void randomizeMineLocation()
        {
            Random r = new Random();
            // there should be a better way to combine this
            HashSet<int> xPos = new HashSet<int>();
            while(xPos.Count < NumMines) {
                xPos.Add(r.Next(0, XGridSize)); 
            }
            Console.WriteLine(xPos);
            HashSet<int> yPos = new HashSet<int>();
            while (yPos.Count < NumMines)
            {
                yPos.Add(r.Next(0, XGridSize));
            }
            Console.WriteLine(yPos);

        }
    }
}
