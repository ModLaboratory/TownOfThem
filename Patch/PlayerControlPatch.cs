using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownOfThem.Patch
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
    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.OnGameStart))]
    class OnGameStartPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            __instance.SetRole(AmongUs.GameOptions.RoleTypes.Impostor);
        }
    }
}
