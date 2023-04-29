using HarmonyLib;

namespace TownOfThem.Patch;
[HarmonyPatch(typeof(AnnouncementPopUp), nameof(AnnouncementPopUp.UpdateAnnouncementText))]
class AnnouncementsPatch
{

    public static string AnnouncementText = "test";
    /*public static bool Prefix(AnnouncementPopUp __instance)
    {
        return false;
    }*/
}
