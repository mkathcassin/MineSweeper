using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MineSweeper_mcassin.GameStateManager;

namespace MineSweeper_mcassin
{
    /// <summary>
    /// This generates each cell, stores it's position from generated on the grid, flag state, mine state, and generates UI.
    /// Click event per cell generates checks on win or lose state.
    /// </summary>
    internal class Cell
    {
        //Universal size of UI cell Dimmensions
        public static readonly int CellUIDimms = 30;

        public readonly bool isMine;
        public bool IsFlagged;
        public bool IsVisible = false;
        public readonly int xPos;
        public readonly int yPos;
        public readonly int numMinesTouching;
        public Button cellUI;
        public TextBlock underButton;
        public Rectangle BackGround;
        private MineGrid mGrid;
        public Cell(int x, int y, bool mine, int minesTouching, MineGrid grid)
        {
            isMine = mine;
            numMinesTouching = minesTouching;
            xPos = x;
            yPos = y;
            cellUI = ButtonUISetUp();
            underButton = UnderSetup();
            BackGround = GetBackGround();
            mGrid = grid;
            OnGameOver_Lose += GameOver;
        }

        private Button ButtonUISetUp()
        {
            var cell = new Button
            {
                Height = CellUIDimms,
                Width = CellUIDimms,

            };
            cell.Click += Clicked;
            cell.MouseDown += ResetIconChange;
            cell.MouseUp += ResetIconChange;
            cell.MouseRightButtonDown += Flagged;

            Grid.SetColumn(cell, xPos);
            Grid.SetRow(cell, yPos);

            return cell;
        }

        private TextBlock UnderSetup()
        {

            var block = new TextBlock();
            block.Text = numMinesTouching == 0 || isMine ? "" : numMinesTouching.ToString();
            block.FontSize = 20;
            block.FontWeight = FontWeights.Bold;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.VerticalAlignment = VerticalAlignment.Center;
            block.TextAlignment = TextAlignment.Center;

            Grid.SetColumn(block, xPos);
            Grid.SetRow(block, yPos);

            return block;
        }

        private Rectangle GetBackGround()
        {
            var border = new Rectangle();
            border.Height = CellUIDimms;
            border.Width = CellUIDimms;
            border.Stroke = Application.Current.Resources["MSBrightYellow"] as SolidColorBrush;
            border.StrokeThickness = -1;
            border.Fill = isMine ? Application.Current.Resources["MSRed"] as SolidColorBrush :
                                   Application.Current.Resources["MSYellow"] as SolidColorBrush;

            Grid.SetColumn(border, xPos);
            Grid.SetRow(border, yPos);

            return border;
        }

        private void Flagged(object sender, MouseButtonEventArgs e)
        {
            if (!IsFlagged)
            {
                cellUI.Content = new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\Candycornflagbackup.png", UriKind.RelativeOrAbsolute)) };
                mGrid.numFlaggedMines++;
                if (isMine)
                {
                    mGrid.numCorrectlyFlaggedMines++;
                }
            }
            else
            {
                cellUI.Content = "";
                mGrid.numFlaggedMines--;
                if (isMine)
                {
                    mGrid.numCorrectlyFlaggedMines--;
                }
            }
            IsFlagged = !IsFlagged;
            mGrid.CheckWin();
        }


        public void Clicked(object sender, RoutedEventArgs e)
        {
            if (mGrid.numRevealedCells == 0) { TriggerGameStart(); }
            if (!IsFlagged)
            {
                IsVisible = true;
                mGrid.mineGridUI.Children.Remove(cellUI);
                if (isMine)
                {
                    TriggerGameEnd(false);
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

        private void SurroundingCells(RoutedEventArgs e)
        {
            foreach (var coor in mGrid.SurroundingCoordinates(xPos, yPos))
            {
                var sc = mGrid.GridCells[coor.Item1, coor.Item2];
                if (!sc.IsVisible && !sc.IsFlagged)
                {
                    sc.Clicked(this, e);
                }
            }
        }

        private void GameOver()
        {
            cellUI.IsEnabled = false;
        }

        public void ResetIconChange(object sender, RoutedEventArgs e)
        {
            TriggerCellClicked();
        }
    }
}