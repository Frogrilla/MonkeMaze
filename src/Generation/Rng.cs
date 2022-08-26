using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace MonkeMaze.Generation
{
    internal static class Rng
    {
        internal static System.Random rand;
        internal static int RandIndex(int cap)
        {
            return rand.Next(cap);
        }
        internal static void NewRandom()    
        {   
            if(PhotonNetwork.InRoom) {
                object time = 0;
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("CreationTime", out time);
                string seed = PhotonNetwork.CurrentRoom.Name + time;
                int hash = MyHash(seed);
                Console.WriteLine($"\n{time}\n{seed}\n{hash}");
                rand = new System.Random(hash); 
                return; 
            }
            rand = new System.Random(DateTimeOffset.Now.ToUnixTimeMilliseconds().GetHashCode());
        }

        internal static int MyHash(string str)
        {
            int hash = 0;
            foreach (byte b in Encoding.ASCII.GetBytes(str)) hash += b;
            return hash;
        }
    }
}
