using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace MineSweeper_mcassin
{
    internal class Grid
    {
        public int XGridSize =100; //remove values later
        public int YGridSize = 100;
        public int NumMines = 10;
        public Cell[,] GridCells;

        public Grid()
        {
            GridCells = new Cell[XGridSize, YGridSize];
            var mineCoors = randomizeMineLocation().ToList();
            //Could probably be done in a cooler way?
            for(int i = 0; i < XGridSize; i++)
            {
                for(int j = 0; j < YGridSize; j++)
                {
                    GridCells[i, j] = new Cell(i, j);
                    GridCells[i, j].isMine = mineCoors.Contains((i,j));
                    GridCells[i, j]. = NumMinesTouching();
                }
            }
        }

        private List<(int,int)> randomizeMineLocation()
        {
            Random r = new Random();

            var mineCoordinates = new HashSet<(int,int)>(); //using hashset for unique coordinates
            while (mineCoordinates.Count < NumMines) {
                mineCoordinates.Add((r.Next(0, XGridSize), r.Next(0, YGridSize)));
            }

            return mineCoordinates.ToList();
        }

        private int NumMinesTouching((int,int) cellCoordinate)
        {
            return 0;
        }
    }
}
