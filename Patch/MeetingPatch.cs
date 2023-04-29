using HarmonyLib;

namespace TownOfThem.Patch;

[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
class MeetingStartPatch
{
    public static void Postfix()
    {
        Main.IsMeeting = true;
    }
}
[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.OnDestroy))]
class MeetingOnDestroyPatch
{
    public static void Postfix()
    {
        Main.IsMeeting = false;
    }
}
