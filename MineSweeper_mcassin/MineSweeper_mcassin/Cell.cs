using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            };
            cell.Click += Clicked;
            cell.PreviewMouseDown += Flagged; //this sometimes causes a flag instead of a click (will fix if I have time)

            Grid.SetColumn(cell, xPos);
            Grid.SetRow(cell, yPos);
            return cell;
        }

        private void Flagged(object sender, RoutedEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if(me.LeftButton == MouseButtonState.Pressed){
                IsFlagged = !IsFlagged;
                cellUI.Background = Brushes.Cyan;
            }

        }

        //tired coding, think of something sleaker
        public void Clicked(object sender, RoutedEventArgs e)
        {
            if (isMine)
            {
                //this should be an event
                return;
            }
            isVisible = true;
            cellUI.IsEnabled = false;
            if(numMinesTouching == 0)
            {
                SurroundingCells(e);
            }
            else
            {
                cellUI.Content = numMinesTouching;
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