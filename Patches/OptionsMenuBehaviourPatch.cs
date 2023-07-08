using TownOfThem.Modules;
using static TownOfThem.Modules.CustomOptionsMenuManager;


namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour),nameof(OptionsMenuBehaviour.Start))]
    public static class OptionsMenuBehaviourPatch
    {
        public static ModOption testOpt;
        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            CustomOptionsMenuManager.Init(__instance);
            CreateAllButtons();
        }
        public static void CreateAllButtons()
        {
            testOpt = new(StringKey.test, () => { return !testOpt.ToggleButton.onState; }, false);
        }
    }
}
