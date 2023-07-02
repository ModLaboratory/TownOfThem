using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    class StartGamePatch
    {
        public static void Prefix(GameStartManager __instance)
        {
            if (TownOfThem.Modules.CustomGameOptions.DebugMode.getBool())
            {
                __instance.MinPlayers = 0;
            }
            else
            {
                __instance.MinPlayers = 4;
            }
        }
    }
}
