using static TownOfThem.Modules.Language;
using TownOfThem.Roles.Neu;
using TownOfThem.Modules;
using TownOfThem.Modules;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(TranslationController),nameof(TranslationController.Initialize))]
    class TranslationControllerInitPatch
    {
        public static void Postfix()
        {
            SetLangDic(TranslationController.Instance.currentLanguage.languageID);
            init = true;
            
            CustomGameOptions.Load();
        }
    }
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new Type[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    class TranslationControllerGetStringPatch
    {
        public static void Postfix([HarmonyArgument(0)] StringNames id, ref string __result)
        {
            var player = ExileControllerWrapUpPatch.lastExiledPlayer;
            switch (id)
            {
                case StringNames.ExileTextPN:
                case StringNames.ExileTextPP:
                case StringNames.ExileTextSN:
                case StringNames.ExileTextSP:
                    if (Jester.players.Contains(player))
                    {
                        __result = string.Format(GetString(StringKey.ExiledText), player, ModHelpers.cs(Jester.color, GetString(StringKey.Jester)),"JesterExiledText","ImpostorCountInfo");
                    }
                    break;
            }
        }
    }
    [HarmonyPatch(typeof(TranslationController),nameof(TranslationController.SetLanguage))]
    class TranslationControllerSetLanguagePatch
    {
        public static void Postfix([HarmonyArgument(0)] TranslatedImageSet lang)
        {
            SetLangDic(lang.languageID);
            GameOptionsMenuStartPatch.destroyAllOptions();
            CustomGameOptions.Load();
        }
    }
}
