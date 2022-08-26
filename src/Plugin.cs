using BepInEx;
using Bepinject;
using System;
using System.Reflection;
using HarmonyLib;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilla;
using Photon.Pun;
using Zenject;
using MonkeMaze.Generation;
using UnityEngine.InputSystem;
using GorillaLocomotion;
using ComputerInterface;
using ExitGames.Client.Photon;

namespace MonkeMaze
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode("monkemaze", "MONKE MAZE", Utilla.Models.BaseGamemode.Casual)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Events.RoomJoined += Joined;
            Events.RoomLeft += Left;
            StartCoroutine(LoadAssets());

            Gamemode.Stats.LoadStats();
            Install();
        }

        private void Install()
        {
            try { Zenjector.Install<CI.MainInstaller>().OnProject(); }
            catch { Console.WriteLine("MonkeMaze stats view not available"); }
        }

        [ModdedGamemodeJoin]
        private void MazeJoin(string gamemode)
        {
            Tunnel.Entrance.Open = true;
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                var table = PhotonNetwork.CurrentRoom.CustomProperties;
                if (table.ContainsKey("CreationTime")) table["CreationTime"] = DateTimeOffset.Now.ToUnixTimeSeconds();
                else table.Add("CreationTime", DateTimeOffset.Now.ToUnixTimeSeconds());
                PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            }
        }
        private void Joined(object sender, EventArgs args)
        {
            Tunnel.Entrance.Open = false;
            Creation.Remove();
        }
        
        private void Left(object sender, EventArgs args)
        {
            Tunnel.Entrance.Open = true;
            Creation.Remove();
        }

        private IEnumerator LoadAssets()
        {
            yield return new WaitForSeconds(3);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MonkeMaze.Resources.maze_assets");
            var Bundle = AssetBundle.LoadFromStream(stream);

            GameObject tunnels = Bundle.LoadAsset<GameObject>("Entrances");
            GameObject[] entrances = new GameObject[]
            {
                tunnels.transform.Find("MazeEntrance (forest)").gameObject,
                tunnels.transform.Find("MazeEntrance (cave)").gameObject,
                tunnels.transform.Find("MazeEntrance (canyon)").gameObject,
                tunnels.transform.Find("MazeEntrance (mountain)").gameObject
            };

            foreach (GameObject entrance in entrances) GameObject.Instantiate(entrance);

            GameObject Tiles = Bundle.LoadAsset<GameObject>("Tiles");
            TileSets.Tiles = new GameObject[] {
                Tiles.transform.Find("EmptyTile").gameObject,
                Tiles.transform.Find("WallTile").gameObject,
                Tiles.transform.Find("EntranceTile").gameObject
            };

            GameObject SecretTiles = Bundle.LoadAsset<GameObject>("SecretTiles");
            TileSets.SecretTiles = new GameObject[] {
                SecretTiles.transform.Find("TheHole").gameObject
            };

            GameObject RelicTiles = Bundle.LoadAsset<GameObject>("RelicTiles");
            TileSets.RelicTiles = new GameObject[] {
                RelicTiles.transform.Find("Monke1").gameObject,
                RelicTiles.transform.Find("Monke2").gameObject,
                RelicTiles.transform.Find("Monke3").gameObject,
                RelicTiles.transform.Find("Monke4").gameObject,
                RelicTiles.transform.Find("Stick").gameObject,
                RelicTiles.transform.Find("Snail").gameObject,
                RelicTiles.transform.Find("Banana").gameObject,
                RelicTiles.transform.Find("Bubble").gameObject,
                RelicTiles.transform.Find("Arrow").gameObject,
                RelicTiles.transform.Find("DefaultCube").gameObject
            };

            TileSets.Compass = Bundle.LoadAsset<GameObject>("Compass");

            Bundle.Unload(false);
        }
    }
}
