

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(ExileController),nameof(ExileController.WrapUp))]
    class ExileControllerWrapUpPatch
    {
        public static PlayerControl lastExiledPlayer = null;
        public static void Postfix()
        {
            lastExiledPlayer = ExileController.Instance.exiled.ToPlayerControl();
        }
    }
}
