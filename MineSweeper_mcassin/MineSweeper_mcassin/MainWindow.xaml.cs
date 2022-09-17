using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            var grid = new Grid(100,100);
            foreach(var cell in grid.GridCells)
            {
                var cellUI = new Button();
                cellUI.Height = 20;
                cellUI.Width = 20;
                MineGridArea.Children.Add(cellUI);
                Debug.WriteLine(cell.isMine);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void CellTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GenGrid();
        }
    }

}
