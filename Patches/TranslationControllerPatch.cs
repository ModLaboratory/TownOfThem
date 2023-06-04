using static TownOfThem.Language.Language;
using TownOfThem.Roles.Neu;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(TranslationController),nameof(TranslationController.Initialize))]
    class TranslationControllerInitPatch
    {
        public static void Postfix()
        {
            SetLangDic(TranslationController.Instance.currentLanguage.languageID);
            init = true;

            CreateCustomObjects.CustomGameOptions.Load();
        }
    }
    //[HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString))]
    //class TranslationControllerGetStringPatch
    //{
    //    public static void Postfix([HarmonyArgument(0)] StringNames id, ref string __result)
    //    {
    //        var player = ExileControllerWrapUpPatch.lastExiledPlayer;
    //        switch (id)
    //        {
    //            case StringNames.ExileTextPN:
    //            case StringNames.ExileTextPP:
    //            case StringNames.ExileTextSN:
    //            case StringNames.ExileTextSP:
    //                if (Jester.players.Contains(player))
    //                {
    //                    __result = string.Format(GetString(Language.StringKey.ExiledPlayerIsJester), player, ModHelpers.cs(Jester.color, GetString(Language.StringKey.Jester)));
    //                }
    //                break;
    //        }
    //    }
    //}
    [HarmonyPatch(typeof(TranslationController),nameof(TranslationController.SetLanguage))]
    class TranslationControllerSetLanguagePatch
    {
        public static void Postfix([HarmonyArgument(0)] TranslatedImageSet lang)
        {
            SetLangDic(lang.languageID);
            CreateCustomObjects.CustomGameOptions.Load();
        }
    }
}
