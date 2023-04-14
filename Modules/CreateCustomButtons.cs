using HarmonyLib;
using Hazel;
using Il2CppSystem.Web.Util;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfThem.CustomObjects;
using TownOfThem.CustomRPCs;
using TownOfThem.Roles;
using TownOfThem.Utilities;
using UnityEngine;
using TownOfThem.PlayersPatch;
using TownOfThem.ModHelpers;

namespace TownOfThem.CreateCustomObjects
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    class CustomCustomButtons
    {
        public static bool initialized = false;
        public static CustomButton sheriffKillButton;
        public static CustomButton battleRoyaleKillButton;
        public static void Postfix(HudManager __instance)
        {
            initialized = false;

            try
            {
                createButtonsPostfix(__instance);
            }
            catch { }
        }

        public static void createButtonsPostfix(HudManager __instance)
        {
            /*battleRoyaleKillButton = new CustomButton
            (
                () => 
                {

                },
                true,
                true,
            );*/
            //sheriffKillButton = new CustomButton(
            //    () =>
            //    {
            //        MurderAttemptResult murderAttemptResult = TownOfThem.ModHelpers.ModHelpers.checkMuderAttempt(Sheriff.sheriff, Sheriff.currentTarget);
            //        if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;

            //        if (murderAttemptResult == MurderAttemptResult.PerformKill)
            //        {
            //            byte targetId = 0;
            //            if (Sheriff.currentTarget.Data.Role.IsImpostor)
            //            {
            //                targetId = Sheriff.currentTarget.PlayerId;
            //            }
            //            else
            //            {
            //                targetId = CachedPlayer.LocalPlayer.PlayerId;
            //            }

            //            MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
            //            killWriter.Write(Sheriff.sheriff.Data.PlayerId);
            //            killWriter.Write(targetId);
            //            killWriter.Write(byte.MaxValue);
            //            AmongUsClient.Instance.FinishRpcImmediately(killWriter);
            //            RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, targetId, Byte.MaxValue);
            //        }

            //        sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
            //        Sheriff.currentTarget = null;
            //    },
            //    () => { return Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            //    () => { return Sheriff.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            //    () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            //    __instance.KillButton.graphic.sprite,
            //    CustomButton.ButtonPositions.upperRowRight,
            //    __instance,
            //    KeyCode.Q
            //);
            initialized = true;
        }


    }
}
