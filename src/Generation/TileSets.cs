using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;

namespace MonkeMaze.Generation
{
    internal static class TileSets
    {
        internal static GameObject[] Tiles;
        internal static GameObject[] SecretTiles;
        internal static GameObject[] RelicTiles;
        internal static GameObject Compass;

        internal static GameObject GetTile(tiles tile)
        {
            int i = (int)tile;
            if(i < Tiles.Length && i >= 0) return Tiles[i];
            if (tile == tiles.relic)
            {
                if (PhotonNetwork.InRoom)
                {
                    // No peaking ;)
                }

                return RelicTiles[Rng.RandIndex(RelicTiles.Length)];
            }

            return Tiles[0];
        }
    }
}
