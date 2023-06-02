using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppSystem.Runtime.ExceptionServices;

namespace TownOfThem.Patch
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
            Announcement a=new();
            a.Date = "1145/1/4";
            a.Id = "114514";
            a.Language = (uint)DataManager.Settings.Language.CurrentLanguage;
            a.Text = "test";
            a.ShortTitle = "55555";
            a.SubTitle = "99999";
            a.Title = "news";
            a.Number = 1145141919;
            __instance.AddAnnouncement(a);
            __instance.HandleChange();

        }
    }
}
