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
{   internal class DifficultyTop10
    {
        [JsonPropertyName("Difficulty")]
        public string? Difficulty { get; set; }

        [JsonPropertyName("Winners")]
        public List<Winner>? Winners { get; set; }
    }
    internal class Winner
    {
        [JsonPropertyName("Time")]
        public int Time { get; set; }
        
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Ranking")]
        public int Ranking { get; set; }
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

        public static List<Winner> ReadJson(string difficulty)
        {
            string json = File.ReadAllText("MineSweeperTop10.json");
            var leaderBoard = JsonSerializer.Deserialize<Dictionary<string, List<Winner>>>(json);
            var local10 = new List<Winner>();

            if (!leaderBoard.TryGetValue(difficulty, out local10) || local10.Count < 10)
            {
                local10 = FixJsonFile(local10);
            }

            return local10;
        }

        //could probably write a custom comparer for this?
        public static bool CompareLeaderBoard(int time, string difficulty)
        {

            foreach (var winner in ReadJson(difficulty))
            {
                if (winner.Time > time) return true;
            }

            return false;
        }

        public static void WriteJson(Winner newWinner, string difficulty)
        {
            string json = File.ReadAllText("MineSweeperTop10.json");
            var fullLeaderBoard = JsonSerializer.Deserialize<Dictionary<string, List<Winner>>>(json);
            var localTop10 = ReadJson(difficulty);

            localTop10.Add(newWinner);
            localTop10 = localTop10.OrderBy(w => w.Time).ToList();
            localTop10.RemoveAll(w => localTop10.IndexOf(w) > 9);

            //Adding Ranking
            for (int i = 0; i < 10; i++)
            {
                localTop10[i].Ranking = i;
            }

            if (!fullLeaderBoard.ContainsKey(difficulty))
            {
                fullLeaderBoard.Add(difficulty, new List<Winner>());
            }

            fullLeaderBoard[difficulty] = localTop10;

            json = JsonSerializer.Serialize(fullLeaderBoard);
            File.WriteAllText("MineSweeperTop10.json", json);
        }

        //method to clean up if anything happens to the json file
        private static List<Winner> FixJsonFile(List<Winner>? brokenJson)
        {
            brokenJson = brokenJson != null ? brokenJson : new List<Winner>();
            
            for (int i = brokenJson.Count; i < 10; i++)
            {
                brokenJson.Add(new Winner() { Time = 999, UserName = "---", Ranking = i+1 });
            }

            return brokenJson;
        }
    }
}
