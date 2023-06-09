using HarmonyLib;
using Hazel;
using TownOfThem.Utilities;
using TownOfThem;
using System.Collections.Generic;
using Il2CppSystem.Web.Util;
using Il2CppSystem.Linq.Expressions;
using TownOfThem.Patch;
using static TownOfThem.Patch.SendModVer;
using static TownOfThem.Language.Translation;
using UnityEngine;

namespace TownOfThem.Patch
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined))]
    class SendModVer
    {
        public static Dictionary<uint, string> playerVersion = new Dictionary<uint, string>();
        public static bool versionSent = false;
        public static void Postfix()
        {
            if (!versionSent)
            {
                ShareVersion();
                versionSent = true;
            }
        }
        public static void ShareVersion()
        {
            uint netID = PlayerControl.LocalPlayer.NetId;
            PlayerControl.LocalPlayer.RpcSendModVersion(Main.ModVer);
            RPCProcedure.ShareModVersion(netID, Main.ModVer);
        }
    }
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    class CheckPlayer
    {
        public static float startingTimer = 0;
        public static void Postfix(GameStartManager __instance)
        {
            if (!versionSent)
            {
                ShareVersion();
                versionSent = true;
            }
            bool canStart = true;
            string message = "";
            foreach (PlayerControl pc in PlayerControl.AllPlayerControls)
            {
                //uint cntID = 0;
                uint netID = pc.NetId;
                if (pc == null||pc.Data.Disconnected) continue;
                if (playerVersion.ContainsKey(netID))
                {
                    if (playerVersion[netID] != Main.ModVer)
                    {
                        
                        canStart = false;
                        message += $"<color=#FF0000FF>{pc.Data.PlayerName} {GetString("PlayerCheckError2")}</color>";
                        //foreach (var cnt in AmongUsClient.Instance.allClients)
                        //{
                        //    cntID = cnt.Character.NetId;
                        //    if (cntID ==)
                        //}
                        //AmongUsClient.Instance.KickPlayer();
                        //Main.Log.LogInfo($"{cntID}   {netID}");
                    }
                }
                else
                {
                    canStart = false;
                    message += $"<color=#FF0000FF>{pc.Data.PlayerName} {GetString("PlayerCheckError1")}</color>\n";
                }

            }
            if (AmongUsClient.Instance.AmHost)
            {
                if (!canStart)
                {
                    __instance.StartButton.color = __instance.startLabelText.color = Palette.DisabledClear;
                    __instance.GameStartText.text = message;
                    __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 2;
                }
                else
                {
                    __instance.StartButton.color = __instance.startLabelText.color = ((__instance.LastPlayerCount >= __instance.MinPlayers) ? Palette.EnabledColor : Palette.DisabledClear);
                    __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;
                }

            }

        }

    }
}

