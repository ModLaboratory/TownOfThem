

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(ExileController),nameof(ExileController.WrapUp))]
    class ExileControllerWrapUpPatch
    {
        public static PlayerControl lastExiledPlayer = null;
        public static void Postfix(ExileController __instance)
        {
            lastExiledPlayer = __instance.exiled.ToPlayerControl();
            Main.Log.LogInfo(__instance.Text.text);
        }
    }
}
