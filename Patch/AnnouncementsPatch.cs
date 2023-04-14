using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Il2CppSystem.Runtime.ExceptionServices;

namespace TownOfThem.AnnouncementsPatch
{    [HarmonyPatch(typeof(AnnouncementPopUp),nameof(AnnouncementPopUp.UpdateAnnouncementText))]
    class AnnouncementsPatch
    {
        
        public static string AnnouncementText = "test";
        /*public static bool Prefix(AnnouncementPopUp __instance)
        {
            return false;
        }*/
    }
}
