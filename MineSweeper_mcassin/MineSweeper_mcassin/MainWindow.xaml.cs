using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MineSweeper_mcassin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GenGrid();
        }
        private void GenGrid()
        {
            var gridData = new MineGrid(20,20);
            
            //This should also be in the MineGrid Class
            var mineGrid = new UniformGrid();
            mineGrid.Height = 20 * 20; //replace with Ymax * 20
            mineGrid.Width = 20 * 20; //replace with Xmax * 20
            foreach(var cell in gridData.GridCells)
            {
                //this should probably live in the Cell Class once we figure this out
                var cellUI = new Button();
                cellUI.Height = 20;
                cellUI.Width = 20;
                mineGrid.Children.Add(cellUI);
                Grid.SetColumn(cellUI, cell.xPos);
                Grid.SetRow(cellUI, cell.yPos);
                cellUI.Click += CellClicked;
            }
            NumMines.Text = gridData.NumMines.ToString();
            RootLayout.Children.Add(mineGrid);
        }

        private void SettingsPanel_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CellClicked(object sender, RoutedEventArgs e)
        {
            var cell = (Button)sender;
            cell.Background = Brushes.Teal;
        }
    }

}
