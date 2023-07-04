using TownOfThem.Modules;
using static TownOfThem.Modules.CustomOptionsMenuManager;


namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour),nameof(OptionsMenuBehaviour.Start))]
    public static class OptionsMenuBehaviourPatch
    {
        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            CustomOptionsMenuManager.Init(__instance);
            CreateAllButtons();
        }
        public static void CreateAllButtons()
        {
            ModOption[] allButtons =
            {

            };
        }
    }
}
