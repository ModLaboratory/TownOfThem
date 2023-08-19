using NextTheirTown.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextTheirTown.Patches
{
    [HarmonyPatch(typeof(HudManager))]
    class HudManagerPatch
    {
        [HarmonyPatch(nameof(HudManager.Start))]
        [HarmonyPostfix]
        public static void HudStartPatch(HudManager __instance) => CustomButton.Init(__instance);

        [HarmonyPatch(nameof(HudManager.Update))]
        [HarmonyPostfix]
        public static void HudUpdatePatch() => CustomButton.UpdateAll();
    }
    
}
