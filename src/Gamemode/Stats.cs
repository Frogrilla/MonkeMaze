using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace MonkeMaze.Gamemode
{
    internal static class Stats
    {
        internal static Dictionary<string, int> Scores = new Dictionary<string, int>();
        internal static void LoadStats()
        {
            string DataPath = Path.Combine(Path.GetDirectoryName(typeof(Stats).Assembly.Location), "SaveData");
            if (!File.Exists(DataPath)) return;
            Scores = JsonConvert.DeserializeObject<Dictionary<string,int>>(Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(DataPath))));
            Scores = Scores.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        internal static void SaveStats()
        {
            string DataPath = Path.Combine(Path.GetDirectoryName(typeof(Stats).Assembly.Location), "SaveData");
            string data = JsonConvert.SerializeObject(Scores);
            File.WriteAllText(DataPath, Convert.ToBase64String(Encoding.UTF8.GetBytes(data)));
        }

        internal static void AddScore(string name, int ammount)
        {
            if (Scores.ContainsKey(name)) Scores[name] += ammount;
            else Scores.Add(name, ammount);
            Scores = Scores.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            SaveStats();
        }
    }
}
