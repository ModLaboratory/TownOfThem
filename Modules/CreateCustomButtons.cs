using HarmonyLib;
using TownOfThem.CustomObjects;
using TownOfThem.Roles.Crew;
using TownOfThem.Patch;
using UnityEngine;

namespace TownOfThem.CreateCustomObjects
{
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
        //    leaderMeetingButton = new CustomButton
        //    (
        //        () =>
        //        {
        //            foreach (var pc in PlayerControl.AllPlayerControls)
        //            {
        //                pc.ReportDeadBody(PlayerControl.LocalPlayer.Data);
        //            }
        //        },
        //        () =>
        //        {
        //            return true;
        //        },
        //        () =>
        //        {
        //            return true;
        //        },
        //        () =>
        //        {
        //            leaderMeetingButton.setActive(true);
        //        },
        //        HudManager.Instance.ReportButton.graphic.sprite,
        //        CustomButton.ButtonPositions.lowerRowRight,
        //        __instance,
        //        KeyCode.M,
        //        false,
        //        "Meeting"
        //    );
            /*battleRoyaleKillButton = new CustomButton
            (
                () => 
                {

                },
                true,
                true,
            );*/
            sheriffKillButton = new CustomButton(
                () =>
                {
                    if (Sheriff.target == null) return;
                    //if(Sheriff.target==)
                    
                },
                () => {  },
                () => {  },
                () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );



            leaderMeetingButton.setActive(true);


            initialized = true;
        }


    }
}
