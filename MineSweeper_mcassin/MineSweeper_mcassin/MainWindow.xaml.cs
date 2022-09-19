﻿using System;
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
using System.Windows.Threading;
using static MineSweeper_mcassin.GameStateManager;

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
        private int timerTime = 0;
        private readonly DispatcherTimer timer;
        public MainWindow()
        {
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            timer = TimerSetUp();
            OnGameOver_Win += ResetButtonUpdate_Win;
            OnGameOver_Lose += ResetButtonUpdate_Lose;
            InitializeComponent();
            UpdateGameState();
        }
        private void UpdateGameState()
        {
            timer.Stop();
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            TimerDisplay.Text = "000";
            timerTime = 0;
            NumMinesDisplay.Text = NumMines.ToString();
            RootLayout.Children.Add(mineGrid.mineGridUI);
        }
        private DispatcherTimer TimerSetUp()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            GameStart += timer.Start;
            OnGameOver_Lose += timer.Stop;
            OnGameOver_Win += timer.Stop;
            return timer;
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            timerTime++;
            TimerDisplay.Text = timerTime.ToString("000");
        }
        private void SettingsPanel_Click(object sender, RoutedEventArgs e)
        {
            if (!settingsPopUp.IsOpen) {
                timer.Stop();
            }
            else { timer.Start(); }
            settingsPopUp.IsOpen = !settingsPopUp.IsOpen;
            
        }

        private void ResetGridButton_Click(object sender, RoutedEventArgs e)
        {
            RootLayout.Children.Remove(mineGrid.mineGridUI);
            UpdateGameState();
        }

        private void DifficultyChanged(object sender, RoutedEventArgs e)
        {
            RootLayout.Children.Remove(mineGrid.mineGridUI);
            List<RadioButton> difficultybuttons = new List<RadioButton>() { Easy, Medium, Hard, Custom};
            var difficulty = difficultybuttons.Find(b => b.IsChecked == true);
            switch (difficulty.Name)
            {
                case "Easy":
                    MineGridX = 9;
                    MineGridY = 9;
                    NumMines = 10;
                    break;
                case "Medium":
                    MineGridX = 16;
                    MineGridY = 16;
                    NumMines = 40;
                    break;
                case "Hard":
                    MineGridX = 30;
                    MineGridY = 16;
                    NumMines = 99;
                    break;
                case "Custom":
                    MineGridX = Int32.Parse(CustomHeight.Text);
                    MineGridY = Int32.Parse(CustomWidth.Text);
                    NumMines = Int32.Parse(CustomMines.Text);
                    break;
            }
            UpdateGameState();
            settingsPopUp.IsOpen = false;
        }

        private void ResetButtonUpdate_Win()
        {
            ResetGridButton.Background = Brushes.Green;
        }

        private void ResetButtonUpdate_Lose()
        {
            ResetGridButton.Background = Brushes.Red;
        }
    }

}
