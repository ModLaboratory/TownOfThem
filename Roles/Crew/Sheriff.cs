//using AmongUs.Data;
//using BepInEx;
//using System;
//using BepInEx.IL2CPP;
//using HarmonyLib;
//using UnityEngine;
//using AmongUs.GameOptions;
//using InnerNet;
//using Hazel;

//namespace TownOfThem.Roles
//{
//    //[HarmonyPatch(typeof(HudManager),nameof(HudManager.Start))]
//    class Sheriff
//    {
//        //public static void Postfix()
//        //{
//        //    if (TownOfThem.IntroPatch.IntroPatch.MyRoleID != 100) return;
//        //    if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
//        //    {
//        //        string PlayerName = PlayerControl.LocalPlayer.name;
//        //        PlayerControl.LocalPlayer.SetRole(AmongUs.GameOptions.RoleTypes.Impostor);
//        //        var deleteSabotage = GameObject.Find("SabotageButton");
//        //        deleteSabotage.SetActive(false);
//        //        var deleteVent = GameObject.Find("VentButton");
//        //        deleteVent.SetActive(false);
//        //        PlayerControl.LocalPlayer.SetRole(AmongUs.GameOptions.RoleTypes.Crewmate);
//        //        PlayerControl.LocalPlayer.SetName(PlayerName + "\n<color=#F8CD46>Sheriff</color>");
//        //    }
//        //}
//    }
//    //[HarmonyPatch(typeof(KillButton),nameof(KillButton.DoClick))]
//    class SheriffKill
//    {
//        //public static bool Prefix(KillButton __instance)
//        //{
//        //    if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
//        //    {
//        //        if (TownOfThem.IntroPatch.IntroPatch.MyRoleID == 100)
//        //        {
//        //            if (__instance.currentTarget.Data.RoleType != RoleTypes.Impostor)
//        //            {
//        //                PlayerControl.LocalPlayer.RpcSetScanner(true);
//        //            }
//        //        }

//        //    }
//        //    return true;
//        //}
//    }
//}