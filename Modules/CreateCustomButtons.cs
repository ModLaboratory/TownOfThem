using HarmonyLib;
using TownOfThem.CustomObjects;
using UnityEngine;

namespace TownOfThem.CreateCustomObjects;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
class CustomCustomButtons
{
    public static bool initialized = false;
    public static CustomButton sheriffKillButton;
    public static CustomButton leaderMeetingButton;
    public static CustomButton battleRoyaleKillButton;
    public static void Postfix(HudManager __instance)
    {
        initialized = false;

        try
        {
            createButtonsPostfix(__instance);
        }
        catch { Main.Log.LogMessage("Error creating button!"); }
    }

    public static void createButtonsPostfix(HudManager __instance)
    {
        leaderMeetingButton = new CustomButton
        (
            () =>
            {
                foreach (var pc in PlayerControl.AllPlayerControls)
                {
                    pc.ReportDeadBody(PlayerControl.LocalPlayer.Data);
                }
            },
            () =>
            {
                return true;
            },
            () =>
            {
                return true;
            },
            () =>
            {
                leaderMeetingButton.setActive(true);
            },
            HudManager.Instance.ReportButton.graphic.sprite,
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.M,
            false,
            "Meeting"
        );
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



        leaderMeetingButton.setActive(true);


        initialized = true;
    }


}
