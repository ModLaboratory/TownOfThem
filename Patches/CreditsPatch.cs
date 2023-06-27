namespace TownOfThem.Patches
{
    [HarmonyPatch]
    class CreditsPatch
    {
        public static bool IsModCredits = false;
        public static Il2CppSystem.Collections.Generic.List<CreditsScreenPopUp.CreditsBlock> VanillaCredits = new();
        [HarmonyPatch(typeof(CreditsScreenPopUp), nameof(CreditsScreenPopUp.OnEnable))]
        class CreditsOnEnablePatch
        {
            public static void Postfix(CreditsScreenPopUp __instance)
            {
                if (!IsModCredits) return;
                
                __instance.allCredits.Clear();
                SetModCredits(__instance);
            }
            static void SetModCredits(CreditsScreenPopUp __instance)
            {
                
                CreditsScreenPopUp.CreditsBlock ModCredits=new();
                CreditsScreenPopUp.CreditsLine a =new();
                a.Name = "114514";
                a.Title = "1919810";
                ModCredits.Lines.Add(a);
                ModCredits.Header = "head";
                __instance.allCredits.Add(ModCredits);
            }
        }
        [HarmonyPatch(typeof(CreditsScreenPopUp), nameof(CreditsScreenPopUp.OnDisable))]
        class CreditsOnDisablePatch
        {
            public static void Postfix(CreditsScreenPopUp __instance)
            {
                __instance.allCredits = VanillaCredits;
                IsModCredits = false;
            }
        }
        [HarmonyPatch(typeof(CreditsScreenPopUp),nameof(CreditsScreenPopUp.Awake))]
        class SyncVanillaCreditsPatch
        {
            public static void Postfix(CreditsScreenPopUp __instance)
            {
                VanillaCredits = __instance.allCredits;
            }
        }
    }
}
