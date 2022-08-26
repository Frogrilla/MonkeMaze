using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using MonkeMaze.Generation;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace MonkeMaze.Tunnel
{
    internal class Entrance : MonoBehaviour
    {
        internal static List<Entrance> Instances = new List<Entrance>();
        internal static GameObject Parent = new GameObject();
        private static bool open = true;
        internal static bool Open
        {
            get => open;
            set
            {
                Instances.ForEach(i =>
                {
                    i.transform.Find("Closed").gameObject.SetActive(!value);
                    i.transform.Find("Tunnel/default").GetComponent<Collider>().enabled = value;
                });
                open = value;
            }
        }

        public Vector3 pos = Vector3.zero;
        public Vector3 rot = Vector3.zero;
        public Vector3 scale = Vector3.zero;
        public Vector3 exitPos = Vector3.zero;
        private void Awake()
        {
            Instances.Add(this);
            transform.Find("Closed").gameObject.SetActive(false);
            transform.Find("Tunnel/default").GetComponent<Collider>().enabled = true;

            transform.parent = Parent.transform;
            transform.position = pos;
            transform.eulerAngles = rot;
            transform.localScale = scale;
        } 
        private void OnTriggerEnter(Collider collider)
        {
            if (!open || collider.name != "Body Collider") return;
            Rng.NewRandom();
            Creation.GenerateAll();
            Teleportation.TeleportPlayer(Creation.GetTilePos(Creation.start[0], Creation.start[1], false));
            Teleportation.ExitPos = exitPos;
        }
    }
}
