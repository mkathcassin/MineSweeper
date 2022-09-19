using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public delegate void GameEvent();
        public static event GameEvent GameStart, OnGameOver_Win, OnGameOver_Lose;

        public static void TriggerGameEnd(bool winOrLose)
        {
            if (winOrLose)
            {
                OnGameOver_Win?.Invoke();
            }
            else
            {
                OnGameOver_Lose?.Invoke();
            }
        }

        public static void TriggerGameStart()
        {
            GameStart?.Invoke();
        }
       
    }
}
