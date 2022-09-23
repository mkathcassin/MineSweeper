using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static MineSweeper_mcassin.GameStateManager;

namespace MineSweeper_mcassin
{
    /// <summary>
    /// Main window for all interaction within the MineSweeper App
    /// </summary>
    public partial class MainWindow : Window
    {
        public int MineGridX = 30; //TODO: get set reasonable range
        public int MineGridY = 16; //TODO: get set reasonable range
        public int NumMines = 99; //TODO: get set less than numOfCells-1

        private MineGrid mineGrid;
        private int timerTime = 0;
        private string difficulty = "Hard";
        private readonly DispatcherTimer timer;
        private bool gameOver;
        public MainWindow()
        {
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            timer = TimerSetUp();
            OnGameOver_Win += ResetButtonUpdate_Win;
            OnGameOver_Lose += ResetButtonUpdate_Lose;
            CellClicked += CellDownClick;
            InitializeComponent();
            UpdateGameState();
        }
        private void UpdateGameState()
        {
            timer.Stop();
            mineGrid = new MineGrid(MineGridX, MineGridY, NumMines);
            
            TimerDisplay.Text = "000";
            timerTime = 0;
            
            NumMinesDisplay.Text = NumMines.ToString("000");
            
            RootLayout.Children.Add(mineGrid.mineGridUI);
            
            ResetGridButton.Content= new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\PumpkinNormal.png", UriKind.RelativeOrAbsolute)) };
            
            gameOver = false;
        }
        private DispatcherTimer TimerSetUp()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            GameStart += timer.Start;
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
            List<RadioButton> difficultybuttons = new List<RadioButton>() { EasyDiff, MediumDiff, HardDiff, CustomDiff};
            var newDifficulty = difficultybuttons.Find(b => b.IsChecked == true);
            switch (newDifficulty.Content)
            {
                case "Easy":
                    MineGridX = 9;
                    MineGridY = 9;
                    NumMines = 10;
                    difficulty = "Easy";
                    break;
                case "Medium":
                    MineGridX = 16;
                    MineGridY = 16;
                    NumMines = 40;
                    difficulty = "Medium";
                    break;
                case "Hard":
                    MineGridX = 30;
                    MineGridY = 16;
                    NumMines = 99;
                    difficulty = "Hard";
                    break;
                case "Custom":
                    MineGridX = Int32.Parse(CustomHeight.Text);
                    MineGridY = Int32.Parse(CustomWidth.Text);
                    NumMines = Int32.Parse(CustomMines.Text);
                    difficulty = "Custom";
                    break;
            }
            UpdateGameState();
            settingsPopUp.IsOpen = false;
        }

        private void ResetButtonUpdate_Win()
        {
            if (gameOver) return;
            ResetGridButton.Content = new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\PumpkinWin.png", UriKind.RelativeOrAbsolute))};
            
            timer.Stop();
           
            if(CompareLeaderBoard(timerTime, difficulty))
            {

                WinnerPopUp.IsOpen = true;
               
            }

            gameOver = true;
        }

        private void ResetButtonUpdate_Lose()
        {
            if (gameOver) return;
            ResetGridButton.Content = new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\PumpkinGameOver.png", UriKind.RelativeOrAbsolute)) }; ;
            timer.Stop();
            gameOver = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LeaderBoardDifficultyChange(object sender, RoutedEventArgs e)
        {
            var difficulty = (RadioButton)sender;
            Top10.ItemsSource = ReadJson(difficulty.Name);
        }

        private void LeaderBoard_Click(object sender, RoutedEventArgs e)
        {
            LeaderBoardPopUp.IsOpen = !LeaderBoardPopUp.IsOpen;
            var rbs = LBDifficultyOptions.Children;
            foreach (var item in rbs)
            {
                var button = (RadioButton)item;
                if (button.IsChecked == true)
                {
                    Top10.ItemsSource = ReadJson(button.Name);
                    return;
                }
            }
        }

        private void AcceptName_Click(object sender, RoutedEventArgs e)
        {
            WriteJson(new Winner()
            {
                UserName = UserNameInput.Text,
                Time = timerTime}, difficulty);
            WinnerPopUp.IsOpen = false;

        }

        private void WinnerPopUpClose(object sender, RoutedEventArgs e)
        {
            WinnerPopUp.IsOpen = false;
        }

        private void CellDownClick()
        {
            List<Image> PumkpinImages = new List<Image>() { new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\PumpkinNormal.png", UriKind.RelativeOrAbsolute)) },
                                                            new Image() { Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\..\..\..\Images\PumpkinMouseDown.png", UriKind.RelativeOrAbsolute)) }};

            var index = PumkpinImages.IndexOf(ResetGridButton.Content as Image);
            ResetGridButton.Content = index == 1? PumkpinImages[0] : PumkpinImages[1];

        }
    }

}
