using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MineSweeper_mcassin
{
    internal class Grid
    {
        public int NumMines = 10;
        public Cell[,] GridCells;
        private int xGridSize;
        private int yGridSize;

        public Grid(int x, int y)
        {
            xGridSize = x;
            yGridSize = y;
            GridCells = new Cell[xGridSize, yGridSize];
            var mineCoors = randomizeMineLocation();
            //Could probably be done in a cooler way?
            for(int i = 0; i < xGridSize; i++)
            {
                for(int j = 0; j < yGridSize; j++)
                {
                    GridCells[i, j] = new Cell(i, j);
                    GridCells[i, j].isMine = mineCoors.Contains((i,j));
                    GridCells[i, j].numMinesTouching = NumMinesTouching(i,j);
                }
            }
        }

        private List<(int,int)> randomizeMineLocation()
        {
            var r = new Random();
            var mineCoordinates = new HashSet<(int,int)>(); //using hashset for unique coordinates

            while (mineCoordinates.Count < NumMines) {
                mineCoordinates.Add((r.Next(0, xGridSize), r.Next(0, yGridSize)));
            }

            return mineCoordinates.ToList();
        }

        private int NumMinesTouching(int CoorX, int CoorY)
        {
            int numTouching = 0;
            //try surrounding cells
            //be prepped for outside array
            // (-1,1)  (0,1)  (1,1)
            // (-1,0)  (0,0)  (1,0)
            // (-1,-1) (0,-1) (1,-1)

            //make list of coordinate, remove any outside of (0,maxX) and (0,maxY)

            //easy but ugly, fix this, or this should be a static global function for in game query as well, also edge cells will be out of index
            var coorsToCheck = new List<(int, int)>() { (-1, 1), (0, 1), (1, 1), (-1, 0), (1, 0), (-1, -1), (0, -1), (1, -1)};
            for (int i = -1; i < length; i++)
            {

            }
            foreach (var coor in coorsToCheck) {

                numTouching = GridCells[coor.Item1, coor.Item2].isMine ? numTouching++ : numTouching;
            }

            return numTouching;
        }

        private (int,int) SurroundCoordinates(int x, int y)
        {
            return (0, 0);
        }
    }
}
