

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour),nameof(OptionsMenuBehaviour.Start))]
    public static class OptionsMenuBehaviourPatch
    {
        public static void Postfix(OptionsMenuBehaviour __instance)
        {

        }
    }
}
