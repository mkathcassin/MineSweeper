using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MineSweeper_mcassin
{
    /// <summary>
    /// This generates the board per new game within the app, randomly generates and stores mine location, tracks and checks for win state
    /// </summary>
    internal class MineGrid
    {
        public Cell[,] GridCells;
        public Grid mineGridUI;

        private readonly int xGridSize;
        private readonly int yGridSize; 
        private readonly (int, int)[] mineCoors;
        private readonly int numMines;

        public int numRevealedCells = 0;
        public int numCorrectlyFlaggedMines = 0;
        public int numFlaggedMines = 0;
        
        public MineGrid(int x, int y, int numMines)
        {
            xGridSize = x;
            yGridSize = y;
            GridCells = new Cell[xGridSize, yGridSize];
            this.numMines = numMines;
            mineGridUI = UISetUp();
            mineCoors = randomizeMineLocation(numMines);
            
            for(int i = 0; i < xGridSize; i++)
            {
                for(int j = 0; j < yGridSize; j++)
                {
                    GridCells[i, j] = new Cell(i, j, mineCoors.Contains((i, j)), NumMinesTouching(i, j), this);
                    mineGridUI.Children.Add(GridCells[i, j].BackGround);
                    mineGridUI.Children.Add(GridCells[i, j].underButton);
                    mineGridUI.Children.Add(GridCells[i, j].cellUI);
                }
            }
            GameStateManager.OnGameOver_Lose += RevealAllMines;
        }

        private Grid UISetUp()
        {
            var grid = new Grid();
            grid.Width = xGridSize * Cell.CellUIDimms;
            grid.Height = yGridSize * Cell.CellUIDimms;
            grid.Margin = new Thickness(30);
            Grid.SetColumn(grid,1);
            Grid.SetRow(grid,3);
            //TODO: better way to do this?
            for (int i = 0; i < xGridSize; i++)
            {
                var columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(Cell.CellUIDimms, GridUnitType.Pixel);
                grid.ColumnDefinitions.Add(columnDef);
            }
            for (int i = 0; i < yGridSize; i++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(Cell.CellUIDimms, GridUnitType.Pixel);
                grid.RowDefinitions.Add(rowDef);
            }

            return grid;

        }
        private (int,int)[] randomizeMineLocation(int numMines)
        {
            var r = new Random();
            var mineCoordinates = new HashSet<(int,int)>(); //using hashset for unique coordinates

            while (mineCoordinates.Count < numMines) {
                mineCoordinates.Add((r.Next(0, xGridSize-1), r.Next(0, yGridSize-1)));
            }

            return mineCoordinates.ToArray();
        }

        private int NumMinesTouching(int coorX, int coorY)
        {
            int numTouching = 0;

            foreach (var coor in SurroundingCoordinates(coorX,coorY)) {
                if (mineCoors.Contains(coor))
                {
                    numTouching++;
                }
            }
            return numTouching;
        }

        public (int,int)[] SurroundingCoordinates(int xCoor,int yCoor)
        {
            var coordinates = new List<(int,int)>();

            for (int i = -1; i <= 1; i++)
            {
                var coorX = xCoor + i;
                for(int j = -1; j <= 1; j++)
                {
                    var coorY = yCoor + j;
                    // Checking to make sure the coordinates are within a realistic range (maybe could do this better?)
                    if (coorX >= 0 && coorX < xGridSize && coorY >= 0 && coorY < yGridSize)
                    {
                        coordinates.Add((coorX, coorY));
                    }
                }
            }
            return coordinates.ToArray();
        }

        public void RevealAllMines()
        {
            foreach(var coor in mineCoors)
            {
                mineGridUI.Children.Remove(GridCells[coor.Item1, coor.Item2].cellUI);
            }
        }

        public void CheckWin()
        {
            if(numMines == numCorrectlyFlaggedMines || numRevealedCells == GridCells.Length - numMines)
            {
                GameStateManager.TriggerGameEnd(true);
                Debug.WriteLine("you win");
            }

        }
    }
}
