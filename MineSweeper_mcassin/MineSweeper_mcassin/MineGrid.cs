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
    internal class MineGrid
    {
        public int NumMines = 10;
        public Cell[,] GridCells;
        private int xGridSize;
        private int yGridSize;

        public MineGrid(int x, int y)
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

        private int NumMinesTouching(int coorX, int coorY)
        {
            int numTouching = 0;

            foreach (var coor in SurroundCoordinates(coorX,coorY)) {

                numTouching = GridCells[coor.Item1, coor.Item2].isMine ? numTouching++ : numTouching;
            }

            return numTouching;
        }

        private (int,int)[] SurroundCoordinates(int x, int y)
        {
            var coordinates = new List<(int,int)>();
            //try surrounding cells
            //be prepped for outside array
            // (-1,1)  (0,1)  (1,1)
            // (-1,0)  (0,0)  (1,0)
            // (-1,-1) (0,-1) (1,-1)

            
            //TODO: this is gross and I should be able to shorten this
            for (int i = -1; i == 1; i++)
            {
                var coorX = x + i;
                for(int j = -1; j == 1; j++)
                {
                    var coorY = y + j;
                    if ( Enumerable.Range(0,xGridSize).Contains(coorX) && Enumerable.Range(0, yGridSize).Contains(coorY))
                    {
                        coordinates.Add((coorX, coorY));
                    }
                }
            }
            return coordinates.ToArray();
        }
    }
}
