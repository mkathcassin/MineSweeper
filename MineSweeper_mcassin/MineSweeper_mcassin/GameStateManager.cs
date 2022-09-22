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
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MineSweeper_mcassin
{   
    internal class Winner
    {
        [JsonPropertyName("Difficulty")]
        public string? Difficulty { get; set; }
        
        [JsonPropertyName("Time")]
        public int Time { get; set; }
        
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }
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

        private static List<Winner>? ReadJson()
        {
            string json = File.ReadAllText("MineSweeperTop10.json");
            var board = new List<Winner>();
            if (json.Length > 10)
            {
                board = JsonSerializer.Deserialize<List<Winner>>(json);
            }
            
            return board;
        }

        //could probably write a custom comparer for this?
        public static bool CompareLeaderBoard(int time, string difficulty)
        {
            if (ReadJson().FindAll(w => w.Difficulty == difficulty).Count < 10) return true;

            //creates ordered subBoard
            var localLeaderBoard = ReadJson().FindAll(w => w.Difficulty == difficulty).OrderBy(w => w.Time);
            
            foreach(var winner in localLeaderBoard)
            {
                if (winner.Time > time) return true;
            }

            return false;   
        }

        public static void WriteJson(Winner newWinner)
        {
            var newLeaderBoard = ReadJson();
            var localLeaderBoard = newLeaderBoard.FindAll(w => w.Difficulty == newWinner.Difficulty);
            if ( localLeaderBoard.Count >= 10)
            {
                newLeaderBoard.Remove(newLeaderBoard.FindAll(w => w.Difficulty == newWinner.Difficulty).Last());
            }
            newLeaderBoard.Add(newWinner);
            newLeaderBoard.OrderBy(w => w.Time).ThenBy(w => w.Difficulty);
            var json = JsonSerializer.Serialize(newLeaderBoard);
            File.WriteAllText("MineSweeperTop10.json", json);
        }
    }
}
