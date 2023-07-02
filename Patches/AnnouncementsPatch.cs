using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppSystem.Runtime.ExceptionServices;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(AnnouncementPopUp),nameof(AnnouncementPopUp.Init))]
    class AnnouncementPopUpPatch
    {

    }
    [HarmonyPatch(typeof(PlayerAnnouncementData),nameof(PlayerAnnouncementData.AddAnnouncement))]
    class PlayerAnnouncementDataPatch
    {

        public static void Postfix(PlayerAnnouncementData __instance)
        {

        }
    }
}
