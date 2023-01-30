using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace TownOfThem.AnnouncementsPatch
{
    [HarmonyPatch(typeof(AnnouncementPopUp),nameof(AnnouncementPopUp.UpdateAnnounceText))]
    class AnnouncementsPatch
    {
        public static string AnnouncementText = "I am an idiot";
        public static bool Prefix(AnnouncementPopUp __instance)
        {
            __instance.AnnounceTextMeshPro.text = AnnouncementText;
            return false;
        }
    }
}
