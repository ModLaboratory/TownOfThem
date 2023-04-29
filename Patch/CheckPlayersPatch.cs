﻿using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using TownOfThem.CustomRPCs;
using static TownOfThem.Patch.SendModVer;

namespace TownOfThem.Patch;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined))]
class SendModVer
{
    public static Dictionary<uint, string> playerVersion;
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
        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(netID, (byte)CustomRPC.ShareModVersion, SendOption.Reliable, -1);
        writer.Write(netID);
        writer.Write(Main.ModVer);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
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
            uint netID = pc.NetId;
            if (pc == null || pc.Data.Disconnected) continue;
            if (playerVersion.ContainsKey(netID))
            {
                if (playerVersion[netID] != Main.ModVer)
                {

                    //canStart = false;
                    //message += $"<color=#FF0000FF>{pc.Data.PlayerName} {LoadTranslation("PlayerCheckError2")}</color>";
                    //AmongUsClient.Instance.KickPlayer()
                }
            }
            else
            {
                //canStart = false;
                //message += $"<color=#FF0000FF>{pc.Data.PlayerName} {LoadTranslation("PlayerCheckError1")}</color>\n";
            }

        }
        //if (AmongUsClient.Instance.AmHost)
        //{
        //    if (!canStart)
        //    {
        //        __instance.StartButton.color = __instance.startLabelText.color = Palette.DisabledClear;
        //        __instance.GameStartText.text = message;
        //        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 2;
        //    }
        //    else
        //    {
        //        __instance.StartButton.color = __instance.startLabelText.color = ((__instance.LastPlayerCount >= __instance.MinPlayers) ? Palette.EnabledColor : Palette.DisabledClear);
        //        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;
        //    }

        //}

    }

}

