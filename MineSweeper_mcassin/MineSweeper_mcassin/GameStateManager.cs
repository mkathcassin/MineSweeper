using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MineSweeper_mcassin
{
    internal class WinnerJSON
    {
        public string UserNames;
        public string Difficulty;
        public int Time;
        public int Ranking;
    }
    internal class GameStateManager
    {
        //win -- all flagged or all visible
        //lose
        //Top 10 and all those things
        public readonly DispatcherTimer timer;
        public  int timerTime;
        private int numCorrectFlaggedMines; //if this matches numMines = win
        private int numVisibleCells; // if this == total number - number of mines = win
        public GameStateManager()
        {
            timer = new DispatcherTimer();
            timerTime = 0;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timerTime++;
        }

        public void GameOver(bool winOrLose)
        {
            timer.Stop();
            //check against list of winners
            Debug.WriteLine("Game Over");
        }
    }
}
