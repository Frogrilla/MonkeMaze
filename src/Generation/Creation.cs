using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace MonkeMaze.Generation
{
    internal enum tiles
    {
        floor,
        wall,
        spawn,
        relic,
        none
    }
    internal static class Creation
    {
        internal static Vector3 position = new Vector3(-50, 70, -70);
        internal static float size = 3f;
        internal static int scale = 32;
        internal static tiles[,] map;
        internal static int[] start = new int[2];
        internal static bool Created = false;
        internal static GameObject MonkeMaze;

        internal static void GenerateAll()
        {
            if (!Created) { CreateMap(); SpawnMaze(); Created = true; }
            else { MonkeMaze.SetActive(true); }
        }
        
        internal static void Remove()
        {
            if (!Created) return;
            GameObject.Destroy(MonkeMaze);
            Created = false;
        }
        internal static void CreateMap()
        {
            map = new tiles[scale+2, scale+2];
            for(int i = 0; i < scale+2; ++i)
            {
                for(int j = 0; j < scale+2; ++j)
                {
                    map[i, j] = tiles.wall;
                }
            }

            RecursiveBacktracker.GenBlueprint(scale);
            for (int x = 0; x < scale; x++)
            {
                for (int y = 0; y < scale; y++)
                {
                    bool isFloor = RecursiveBacktracker.Blueprint.blueprint[x, y];
                    tiles tile = isFloor ? tiles.floor : tiles.wall;
                    map[x + 1, y + 1] = tile;
                }
            }

            int middle = Mathf.FloorToInt(scale / 2);
            map[middle+1, middle+1] = tiles.spawn;
            start = new int[] { middle+1, middle+1 };

            map[RecursiveBacktracker.Blueprint.endX+1, RecursiveBacktracker.Blueprint.endY+1] = tiles.relic;
        }

        internal static void SpawnMaze()
        {
            MonkeMaze = new GameObject();
            MonkeMaze.transform.position = position;
            MonkeMaze.AddComponent<Gamemode.Manager>();

            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Plane);
            roof.transform.parent = MonkeMaze.transform;
            roof.transform.localPosition = new Vector3(scale, 4, scale);
            roof.transform.localScale = Vector3.one * scale;
            roof.transform.eulerAngles = new Vector3(0, 0, 180);
            roof.GetComponent<MeshRenderer>().enabled = false;
        }

        internal static string DrawMaze()
        {
            string draw = "\n";
            for (int i = 0; i < scale + 2; ++i)
            {
                for (int j = 0; j < scale + 2; ++j)
                {
                    Vector3 pos = new Vector3(i, 0, j);
                    pos *= size;

                    GameObject tile = GameObject.Instantiate(TileSets.GetTile(map[i, j]));
                    tile.transform.parent = MonkeMaze.transform;
                    tile.transform.localPosition = pos;

                    draw += tile2str[map[i, j]];
                }
                draw += "\n";
            }
            return draw;
        }

        internal static Vector3 GetTilePos(int x, int y, bool local)
        {
            return (new Vector3(x, .5f, y) * size) + (local ? Vector3.zero : position);
        }

        internal static Dictionary<tiles, char> tile2str = new Dictionary<tiles, char>()
        {
            { tiles.floor , ' ' },
            { tiles.wall  , '#' },
            { tiles.spawn , '@' },
            { tiles.relic , 'X' }
        };
    }
}
