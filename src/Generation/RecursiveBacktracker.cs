using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MonkeMaze.Generation
{
    internal struct mazeBlueprint
    {
        internal int endX, endY;
        internal int scale;
        internal bool[,] blueprint;
    }

    internal enum direction
    {
        up,
        down,
        left,
        right,
    }
    internal static class RecursiveBacktracker
    {
        internal static mazeBlueprint Blueprint;

        internal static Dictionary<direction, int[]> Dir2Points = new Dictionary<direction, int[]>
        {
            { direction.up    , new int[] {0  ,  1} },
            { direction.down  , new int[] {0  , -1} },
            { direction.left  , new int[] {-1 ,  0} },
            { direction.right , new int[] {1  ,  0} }
        };

        internal static Dictionary<direction, direction> reverseDir = new Dictionary<direction, direction>
        {
            { direction.up    , direction.down  },
            { direction.down  , direction.up    },
            { direction.left  , direction.right },
            { direction.right , direction.left  }
        };

        internal static direction[] alLDirs = new direction[]
        {
            direction.up, direction.down, direction.left, direction.right
        };

        internal static List<int[]> DeadEnds = new List<int[]>();
        internal static void GenBlueprint(int scale)
        {
            Blueprint = new mazeBlueprint();
            Blueprint.scale = scale;
            Blueprint.blueprint = new bool[scale, scale];
            for(int x = 0; x < scale; ++x)
            {
                for(int y = 0; y < scale; ++y)
                {
                    Blueprint.blueprint[x, y] = false;
                }
            }
            int middle = Mathf.FloorToInt(scale / 2);
            Blueprint.blueprint[middle, middle] = true;
            Blueprint.blueprint[middle, middle + 1] = true;   
            int[] pos = new int[] { middle, middle + 2 };
            Search(pos, direction.up);

            for(int x = 0; x < scale; ++x)
            {
                for(int y = 0; y < scale; ++y)
                {
                    int[] tile = new int[] { x, y };
                    if (!GetPoint(tile)) break;
                    if (tile == new int[] { middle, middle }) break;
                    int count = 0;
                    foreach(direction dir in alLDirs)
                    {
                        int[] check = MoveDir(tile, dir, 1);
                        if (!CheckExists(check)) continue;
                        if (GetPoint(check)) ++count;
                    }
                    if (count == 1) DeadEnds.Add(tile);
                }
            }

            int[] ending = DeadEnds[Rng.RandIndex(DeadEnds.Count)]; 
            Blueprint.endX = ending[0]; Blueprint.endY = ending[1];
        }
        internal static void DebugDraw()
        {
            string draw = "\n";
            for(int x = 0; x < Blueprint.scale; ++x)
            {
                for(int y = 0; y < Blueprint.scale; ++y)
                {
                    draw += Blueprint.blueprint[x, y] ? " " : "#";
                }
                draw += "\n";
            }
            Console.WriteLine(draw);
        }
        internal static bool GetPoint(int[] pos)
        {
            if (!CheckExists(pos)) return false;
            return Blueprint.blueprint[pos[0], pos[1]];
        }
        internal static void SetPoint(int[] pos, bool value)
        {
            if (!CheckExists(pos)) return;
            Blueprint.blueprint[pos[0], pos[1]] = value;
        }

        internal static void Search(int[] pos, direction entrance)
        {
            bool closed = false;
            int iter = 0;
            int[] bridge = MoveDir(pos, reverseDir[entrance], 1);
            SetPoint(pos, true);
            SetPoint(bridge, true);
            while (!closed)
            {
                //Console.WriteLine($"{pos[0]} , {pos[1]}");
                //DebugDraw();
                List<direction> dirs = alLDirs.ToList();
                List<direction> remove = new List<direction>();
                remove.Add(reverseDir[entrance]);
                foreach (direction dir in dirs)
                {
                    int[] check = MoveDir(pos, dir, 2);
                    if (!CheckEmpty(check)) { remove.Add(dir); continue; }
                    if (GetPoint(check)) { remove.Add(dir); }
                }
                remove.ForEach(dir => dirs.Remove(dir));
                //dirs.ForEach(dir => Console.WriteLine(dir));
                if (dirs.Count == 0) closed = true;
                if (closed) return; 

                direction newDir = dirs[Rng.RandIndex(dirs.Count)];
                int[] next = MoveDir(pos, newDir, 2);
                Search(next, newDir);
                ++iter;
            }
        }

        internal static bool CheckExists(int[] index)
        {
            if (Blueprint.scale <= index[0] || index[0] < 0) return false;
            if (Blueprint.scale <= index[1] || index[1] < 0) return false;
            return true;
        }
        internal static bool CheckEmpty(int[] index)
        {
            if (!CheckExists(index)) return false;
            List<direction> dirs = alLDirs.ToList();
            for(int x = -1; x < 2; ++x)
            {
                for(int y = -1; y < 2; ++y)
                {
                    int[] checkIndex = (int[])index.Clone();
                    checkIndex[0] += x;
                    checkIndex[1] += y;
                    if (!CheckExists(checkIndex)) continue;
                    if (GetPoint(checkIndex)) return false;
                }
            }
            return true;
        }

        internal static int[] MoveDir(int[] index, direction dir, int dist)
        {
            int[] newIndex = (int[])index.Clone();
            newIndex[0] += Dir2Points[dir][0] * dist;
            newIndex[1] += Dir2Points[dir][1] * dist;
            return newIndex;
        }
    }
}
