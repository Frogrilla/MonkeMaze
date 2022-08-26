using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MonkeMaze.Gamemode
{
    internal class Relic : MonoBehaviour
    {
        public Vector3 rotation = new Vector3(0, 90, 0);
        private void Update()
        {
            transform.eulerAngles += rotation * Time.deltaTime;
        }
    }
}
