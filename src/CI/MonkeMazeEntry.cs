using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface.Interfaces;

namespace MonkeMaze.CI
{
    internal class MonkeMazeEntry : IComputerModEntry
    {
        public string EntryName => "Monke Maze";
        public Type EntryViewType => typeof(MonkeMazeView);
    }
}
