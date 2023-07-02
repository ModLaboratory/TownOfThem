using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TownOfThem.Roles.Crew;
using TownOfThem.Utilities;
using UnityEngine;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    class KillAnimationCoPerformKillPatch
    {
        public static bool hideNextAnimation = false;
        public static void Prefix(KillAnimation __instance, [HarmonyArgument(0)] ref PlayerControl source, [HarmonyArgument(1)] ref PlayerControl target)
        {
            if (hideNextAnimation)
                source = target;
            hideNextAnimation = false;
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFixedUpdatePatch
    {
        
        public static void Postfix(PlayerControl __instance)
        {
            
        }
    }
}
