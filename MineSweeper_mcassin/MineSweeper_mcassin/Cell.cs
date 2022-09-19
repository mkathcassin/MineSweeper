using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MineSweeper_mcassin
{
    internal class Cell
    {
        public static readonly int CellUIDimms = 20;

        public readonly bool isMine;
        public bool IsFlagged;
        public bool IsVisible = false;
        public readonly int xPos;
        public readonly int yPos;
        public readonly int numMinesTouching;
        public Button cellUI;
        public TextBlock underButton;
        public Rectangle underText;
        private MineGrid mGrid;
        public Cell(int x, int y, bool mine, int minesTouching, MineGrid grid)
        {
            isMine = mine;
            numMinesTouching = minesTouching;
            xPos = x;
            yPos = y;
            cellUI = ButtonUISetUp();
            underButton = UnderSetup();
            underText = GetBackGround();
            mGrid = grid;
        }
      
        private Button ButtonUISetUp()
        {
            var cell = new Button
            {
                Height = CellUIDimms,
                Width = CellUIDimms,
            };
            cell.Click += Clicked;
            cell.MouseRightButtonDown += Flagged;
            Grid.SetColumn(cell, xPos);
            Grid.SetRow(cell, yPos);

            return cell;
        }

        private TextBlock UnderSetup()
        {

            var block = new TextBlock();
            block.Text = numMinesTouching == 0 || isMine ? "" : numMinesTouching.ToString();
            block.FontWeight = FontWeights.Bold;
            switch (numMinesTouching)
            {
                case 1:
                    block.Foreground = Brushes.Blue;
                    break;
                case 2:
                    block.Foreground = Brushes.Green;
                    break;
                case 3:
                    block.Foreground = Brushes.DarkRed; 
                    break;
                case 4:
                    block.Foreground = Brushes.DarkBlue; 
                    break;
                case 5:
                    block.Foreground = Brushes.DarkBlue; //change the rest later
                    break;
                case 6:
                    block.Foreground = Brushes.DarkBlue; 
                    break;
                case 7:
                    block.Foreground = Brushes.DarkBlue; 
                    break;
                case 8:
                    block.Foreground = Brushes.DarkBlue; 
                    break;
            }
            block.TextAlignment = TextAlignment.Center;
            
            Grid.SetColumn(block, xPos);
            Grid.SetRow(block, yPos);
            
            return block;
        }

        private Rectangle GetBackGround()
        {
            var border = new Rectangle();
            border.Stroke = Brushes.DarkGray;
            border.StrokeThickness = .2;
            border.Fill = isMine ? Brushes.Red : Brushes.LightGray;
            Grid.SetColumn(border, xPos);
            Grid.SetRow(border, yPos);
            return border;
        }

        private void Flagged(object sender, MouseButtonEventArgs e)
        {
            if (!IsFlagged)
            {
                cellUI.Background = Brushes.Cyan;
                mGrid.numFlaggedMines++;
                mGrid.numCorrectlyFlaggedMines = isMine ? mGrid.numCorrectlyFlaggedMines++ : mGrid.numCorrectlyFlaggedMines;
            }
            else
            {
                cellUI.Background = Brushes.LightGray;
                mGrid.numFlaggedMines--;
                mGrid.numCorrectlyFlaggedMines = isMine ? mGrid.numCorrectlyFlaggedMines-- : mGrid.numCorrectlyFlaggedMines;
            }
            IsFlagged = !IsFlagged;
            mGrid.CheckWin();
        }


        public void Clicked(object sender, RoutedEventArgs e)
        {
            if (mGrid.numRevealedCells == 0) { GameStateManager.TriggerGameStart(); }
            if (!IsFlagged)
            {
                IsVisible = true;
                mGrid.mineGridUI.Children.Remove(cellUI);
                if (isMine)
                {
                    GameStateManager.TriggerGameEnd(false);
                    return;
                }

                mGrid.numRevealedCells++;
                if (numMinesTouching == 0)
                {
                    SurroundingCells(e);
                }
                mGrid.CheckWin();
            }          
        }

        //Should we maybe just save this on initalization? maybe faster?
        private void SurroundingCells(RoutedEventArgs e){
            foreach(var coor in mGrid.SurroundingCoordinates(xPos,yPos))
            {
                var sc = mGrid.GridCells[coor.Item1, coor.Item2];
                if (!sc.IsVisible && !sc.IsFlagged) {
                    sc.Clicked(this, e); 
                }
            }
        }
    }
}