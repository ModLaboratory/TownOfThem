using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfThem.DebugModePatch
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    class StartGamePatch
    {
        public static void Prefix(GameStartManager __instance)
        {
            if (TownOfThem.CreateCustomObjects.CustomGameOptions.DebugMode.getBool())
            {
                __instance.MinPlayers = 0;
                __instance.countDownTimer = 0;
            }
            else
            {
                __instance.MinPlayers = 4;
                __instance.countDownTimer = 5;
            }
        }
    }
}
