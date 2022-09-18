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
        public int MineGridX = 30; //TODO: get set reasonable range
        public int MineGridY = 16; //TODO: get set reasonable range
        public int NumMines = 99; //TODO: get set less than numOfCells-1

        private MineGrid mineGrid;
        public MainWindow()
        {
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            InitializeComponent();
            UpdateGameState();
        }
        private void UpdateGameState()
        {
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            NumMinesDisplay.Text = NumMines.ToString();
            RootLayout.Children.Add(mineGrid.mineGridUI);
        }

        private void SettingsPanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetGridButton_Click(object sender, RoutedEventArgs e)
        {
            RootLayout.Children.Remove(mineGrid.mineGridUI);
            UpdateGameState();
        }

    }

}
