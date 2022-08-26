using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonkeMaze.Generation;
using Photon.Pun;
using GorillaLocomotion;

namespace MonkeMaze.Tunnel
{
    internal class Exit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name != "Body Collider") return;
            if (PhotonNetwork.InRoom) Creation.MonkeMaze.SetActive(false);
            else Creation.Remove();
            Teleportation.TeleportPlayer(Teleportation.ExitPos);
        }
    }
}
