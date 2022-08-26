using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GorillaLocomotion;
using MonkeMaze.Generation;
using HarmonyLib;

namespace MonkeMaze.Tunnel
{
    internal static class Teleportation
    {
        internal static Vector3 ExitPos = Vector3.zero;
        internal static Traverse GetPrivate(object grab, string name) //Private function, yay!
        {
            return Traverse.Create(grab).Field(name);
        }
        internal static void TeleportPlayer(Vector3 target) //Teleport function! Yay!
        {
            Player __instance = Player.Instance;
            GetPrivate(__instance, "lastPosition").SetValue(target);
            GetPrivate(__instance, "lastLeftHandPosition").SetValue(target);
            GetPrivate(__instance, "lastRightHandPosition").SetValue(target);
            GetPrivate(__instance, "lastHeadPosition").SetValue(target); // Set last position
            __instance.headCollider.attachedRigidbody.transform.position = target;
            __instance.leftHandTransform.position = target;
            __instance.rightHandTransform.position = target;
            __instance.bodyCollider.attachedRigidbody.transform.position = target;
            __instance.GetComponent<Rigidbody>().position = target;
        }
    }

    // Thank you Auralius
}
