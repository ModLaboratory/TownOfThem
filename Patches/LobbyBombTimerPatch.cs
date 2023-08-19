using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TownOfThem.Patches
{
    //codes from KARPED1EM's pull request and thank him for contributing
    //https://github.com/TownOfThemAU/TownOfThem/pull/5/

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    class LobbyBombTimerPatch
    {
        private static bool update = false;
        private static string currentText = "";
        private static float timer = 600f;
        public static void Prefix(GameStartManager __instance)
        {
            // Lobby code
            if (!AmongUsClient.Instance.AmHost || !GameData.Instance || AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame) return; // Not host or no instance or LocalGame
            update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
        }
        public static void Postfix(GameStartManager __instance)
        {
            if (!AmongUsClient.Instance) return;

            // Lobby timer
            if (!AmongUsClient.Instance.AmHost || !GameData.Instance) return;

            if (update) currentText = __instance.PlayerCounter.text;

            timer = Mathf.Max(0f, timer -= Time.deltaTime);
            int minutes = (int)timer / 60;
            int seconds = (int)timer % 60;
            string suffix = $" ({GetString(StringKey.LobbyBombTimerText)} {minutes:00}:{seconds:00})";
            if (timer <= 60) suffix = Utils.ColorString(Color.red, suffix);

            __instance.PlayerCounter.text = currentText + suffix;
            __instance.PlayerCounter.autoSizeTextContainer = true;
        }
    }
}

