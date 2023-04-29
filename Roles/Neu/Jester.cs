using HarmonyLib;

namespace TownOfThem.Roles.Neu;

[HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
class Jester
{
    public static void PostFix(ExileController __instance)
    {
        var pd = __instance.exiled;
        if (pd != null)
        {

        }
    }
}
