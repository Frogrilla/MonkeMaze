using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using MonkeMaze.Generation;
using GorillaLocomotion;

namespace MonkeMaze.Tunnel
{
    internal class Ending : GorillaPressableButton
    {
        public string score = "";
        public override void ButtonActivation()
        {
            base.ButtonActivation();
            if (PhotonNetwork.InRoom) Creation.MonkeMaze.SetActive(false);
            else Creation.Remove();
            Teleportation.TeleportPlayer(Teleportation.ExitPos);
            Gamemode.Stats.AddScore(score, 1);
        }
    }
}
