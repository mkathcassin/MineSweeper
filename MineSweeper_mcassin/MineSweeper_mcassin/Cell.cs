using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace MineSweeper_mcassin
{
    internal class Cell
    {
        public static readonly int CellUIDimms = 20;

        public bool isVisible;
        public readonly bool isMine;
        public bool IsFlagged;
        public readonly int xPos;
        public readonly int yPos;
        public readonly int numMinesTouching;
        private MineGrid mGrid;
        public Button cellUI;

        public Cell(int x, int y, bool mine, int minesTouching, MineGrid grid)
        {
            isVisible = false;
            isMine = mine;
            numMinesTouching = minesTouching;
            xPos = x;
            yPos = y;
            cellUI = UISetUp();
            mGrid = grid;
        }

        private Button UISetUp()
        {
            var cell = new Button
            {
                Height = CellUIDimms,
                Width = CellUIDimms,
                Background = isMine ? Brushes.Red : Brushes.Transparent, //debug
                Content = numMinesTouching == 0 || isMine ? "" : numMinesTouching, //debug
            };
            cell.Click += Clicked;
            cell.PreviewMouseLeftButtonDown += Flagged; //this doesn't work

            Grid.SetColumn(cell, xPos);
            Grid.SetRow(cell, yPos);
            return cell;
        }

        private void Flagged(object sender, RoutedEventArgs e)
        {
            if(e.OriginalSource is Button)
            {
                IsFlagged = !IsFlagged;
                var clickedCell = (Button)sender;
                clickedCell.Background = Brushes.Red;
            }
        }

        //tired coding, think of something sleaker
        public void Clicked(object sender, RoutedEventArgs e)
        {
            if (isMine)
            {
                GameStateManager.GameOver();
                return;
            }
            isVisible = true;
            cellUI.Background = Brushes.Khaki; //DEBUG
            if(numMinesTouching == 0)
            {
                SurroundingCells(e);
            }
            
        }

        private void SurroundingCells(RoutedEventArgs e){
            foreach(var coor in mGrid.SurroundingCoordinates(xPos,yPos))
            {
                var sc = mGrid.GridCells[coor.Item1, coor.Item2];
                if (!sc.isVisible && !sc.IsFlagged) {
                    sc.Clicked(this, e); 
                }
            }
        }

    }
}