using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR;
using MonkeMaze.Generation;
using MonkeTunes.Music;

namespace MonkeMaze.Gamemode
{
    internal class Manager : MonoBehaviour
    {
        internal GameObject compass;
        internal Vector3 lastComputerPos = Vector3.zero;
        internal Vector3 lastComputerRot = Vector3.zero;
        private void OnEnable()
        {
            compass = Instantiate(TileSets.Compass, GorillaLocomotion.Player.Instance.rightHandTransform);
            compass.AddComponent<Compass>();
            compass.transform.localEulerAngles = new Vector3(0, 0, -90);
            compass.transform.localScale = Vector3.one * .05f;
            compass.transform.localPosition = new Vector3(.05f,0,0) + GorillaLocomotion.Player.Instance.rightHandOffset;

            try { MonkeTunesEnter(); }
            catch { Console.WriteLine("No MonkeTunes"); }
        }

        private void MonkeTunesEnter()
        {
            lastComputerPos = MusicPlayer.instance.transform.position;
            lastComputerRot = MusicPlayer.instance.transform.eulerAngles;
            MusicPlayer.instance.transform.SetParent(null, true);
            MusicPlayer.instance.transform.position = Creation.GetTilePos(Creation.start[0], Creation.start[1], false) + new Vector3(-1.2f, -1.175f, 0);
            MusicPlayer.instance.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        private void MonkeTunesLeave()
        {
            MusicPlayer.instance.transform.position = lastComputerPos;
            MusicPlayer.instance.transform.eulerAngles = lastComputerRot;
        }

        private void OnDisable()
        {
            Destroy(compass);
            try { MonkeTunesLeave(); }
            catch { Console.WriteLine("No MonkeTunes"); }
        }
    }
}
