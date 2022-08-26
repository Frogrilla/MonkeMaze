using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface;
using ComputerInterface.ViewLib;
using UnityEngine;
using MonkeMaze.Gamemode;

namespace MonkeMaze.CI
{
    internal class MonkeMazeView : ComputerView
    {
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            DrawScreen();
        }
        private void DrawScreen()
        {
            string statText = "You have no available stats :(";
            if(Stats.Scores.Count > 0)
            {
                statText = "";
                foreach(string key in Stats.Scores.Keys)
                {
                    statText += $"{key}: {Stats.Scores[key]}\n";
                }
            }

            SetText(str =>
            {
                str.BeginCenter();
                str.MakeBar('-', SCREEN_WIDTH, 0, "FFFFFF10").AppendLine();
                str.AppendClr("Monke Maze Stats", "FF0066").AppendLine();
                str.Append("By <color=#38FF8D>Frogrilla</color>").AppendLine();
                str.MakeBar('-', SCREEN_WIDTH, 0, "FFFFFF10").AppendLine();
                str.EndAlign();
                str.Append(statText).AppendLine();
            });
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            base.OnKeyPressed(key);
            switch (key)
            {
                case EKeyboardKey.Back:
                    ReturnView();
                    break;
                case EKeyboardKey.Option1:
                    DrawScreen();
                    break;
            }
        }
    }
}
