using HarmonyLib;
using TownOfThem.Modules;
using TownOfThem.Roles.Crew;
using TownOfThem.Patch;
using UnityEngine;
using TownOfThem.Roles;

namespace TownOfThem.Modules
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    class CreateCustomButtons
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
                    RPCHelper.RpcUncheckedStartMeeting(PlayerControl.LocalPlayer);
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
            sheriffKillButton = new
            (
                () =>
                {
                    if (!Sheriff.players.Contains(PlayerControl.LocalPlayer)) return;
                    RPCHelper.RpcUncheckedMurderPlayer(PlayerControl.LocalPlayer, Sheriff.target.Data.Role.TeamType == RoleTeamTypes.Impostor ? Sheriff.target : PlayerControl.LocalPlayer);
                },
                () =>
                {
                    //Is sheriff & Isnt ded
                    return Sheriff.players.Contains(PlayerControl.LocalPlayer) && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () =>
                {
                    return Sheriff.target != null;
                },
                () =>
                {
                    sheriffKillButton.Timer = Sheriff.cd;
                },
                Sheriff.button,
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.Q,
                buttonText: GetString(StringKey.SheriffShoot)
            );



            switch (PlayerControl.LocalPlayer.GetRole())
            {
                case RoleId.Sheriff:
                    sheriffKillButton.setActive(true);
                    break;
            }

            initialized = true;
        }


    }
}
