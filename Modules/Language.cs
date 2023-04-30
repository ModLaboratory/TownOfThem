using HarmonyLib;
using System.Collections.Generic;
using static TownOfThem.Main;

namespace TownOfThem.Language;

public static class Translation
{
    public static Dictionary<string, string> Translations = new Dictionary<string, string>();
    public static void LoadLanguage(SupportedLangs lang)
    {
        Translations.Clear();
        Translations = AddLanguageText(lang);
        Log.LogInfo("Language Text Added!");
        if (Translations.ContainsKey("Error"))
        {
            Log.LogError("Unknown Language Load Error!");
        }
        if (Translations.Count == 0)
        {
            Log.LogError("Language Pack:" + lang + " Is Empty!");
            Translations.Clear();
            Translations = AddLanguageText(SupportedLangs.English);
            if (Translations.Count == 0)
            {
                Log.LogError("Hard Error:Language Pack" + SupportedLangs.English + " is Missing!");
                ModDamaged = true;
            }
        }
    }
    public static Dictionary<string, string> AddLanguageText(SupportedLangs id)
    {
        bool ContainsLang = id == SupportedLangs.English || id == SupportedLangs.SChinese;//Add Your Languages Here
        Log.LogInfo("Language:" + id);
        if (!ContainsLang)
        {
            Log.LogError("Language " + id + " Not Support,Now Mod Language is:" + SupportedLangs.English);
            id = SupportedLangs.English;
        }
        switch (id)
        {
            case SupportedLangs.English:
                return new Dictionary<string, string>()
                {
                    ["ModLanguage"] = "0",
                    ["ModInfo1"] = "Modded By JieGe ",
                    ["totBirthday"] = "Happy Birthday To ",
                    ["totSettings"] = "Town Of Them Settings ",
                    ["ImpostorSettings"] = "Impostor Roles Settings ",
                    ["NeutralSettings"] = "Neutral Roles Settings ",
                    ["CrewmateSettings"] = "Crewmate Roles Settings ",
                    ["ModifierSettings"] = "Modifier Settings ",
                    ["Preset1"] = "Preset 1",
                    ["Preset2"] = "Preset 2",
                    ["Preset3"] = "Preset 3",
                    ["Preset4"] = "Preset 4",
                    ["Preset5"] = "Preset 5",
                    ["MaxPlayer"] = "Max Player",
                    ["Sheriff"] = "<color=#F8CD46>Sheriff</color>",
                    ["SheriffCD"] = "Sheriff Kill Cooldown",
                    ["SheriffKillLimit"] = "Sheriff Kill Limit",
                    ["Jester"] = "<color=#EC62A5>Jester</color>",
                    ["Handicapped"] = "<color=#808080>Handicapped</color>",
                    ["HandicappedSpeed"] = "Handicapped speed",
                    ["On"] = "On",
                    ["Off"] = "Off",
                    ["Bilibili"] = "BILIBILI",
                    ["ckptPage1"] = "Page 1 : Original Game Settings",
                    ["ckptPage2"] = "Page 2 : Town Of Them Settings",
                    ["ckptPage3"] = "Page 3 : Impostor Role Settings",
                    ["ckptPage4"] = "Page 4 : Neutral Role Settings",
                    ["ckptPage5"] = "Page 5 : Crewmate Role Settings",
                    ["ckptPage6"] = "Page 6 : Modifier Settings",
                    ["ckptToOtherPages"] = "Press TAB or Page Number for more...",
                    ["cmdHelp"] = "Welcome to Among Us Mod---Town Of Them!\nChat command help:\n/help---Show me\n/language---Change language",
                    ["DebugMode"] = "Debug Mode",
                    ["HostSuggestName"] = "Host Suggest Name(You Can Add Custom Text In Config File)",
                    ["HostSuggestName2"] = "Mod Name+Ver",
                    ["HostSuggestName3"] = "Custom",
                    ["CantPlayWithoutMod"] = "Can't Play With Players Who Don't Have Mod",
                    ["CustomGamemodes"] = "Gamemodes",
                    ["Gamemode_Classic"] = "Classic",
                    ["Gamemode_BattleRoyale"] = "Battle Royale",
                    ["PlayerCheckError1"] = "has no mod or other mods.",
                    ["PlayerCheckError2"] = "has a different version of TownOfThem or other mods.",
                };
            case SupportedLangs.SChinese:
                return new Dictionary<string, string>()
                {
                    ["ModLanguage"] = "13",
                    ["ModInfo1"] = "杰哥制作",
                    ["totBirthday"] = "生日快乐，",
                    ["totSettings"] = "他们的小镇设置",
                    ["ImpostorSettings"] = "内鬼职业设置",
                    ["NeutralSettings"] = "中立职业设置",
                    ["CrewmateSettings"] = "船员职业设置",
                    ["ModifierSettings"] = "附加职业设置",
                    ["Preset1"] = "预设1",
                    ["Preset2"] = "预设2",
                    ["Preset3"] = "预设3",
                    ["Preset4"] = "预设4",
                    ["Preset5"] = "预设5",
                    ["MaxPlayer"] = "最大玩家数",
                    ["Sheriff"] = "<color=#F8CD46>警长</color>",
                    ["SheriffCD"] = "警长击杀冷却",
                    ["SheriffKillLimit"] = "警长击杀次数",
                    ["Jester"] = "<color=#EC62A5>小丑</color>",
                    ["Handicapped"] = "<color=#808080>残疾人</color>",
                    ["HandicappedSpeed"] = "残疾人速度",
                    ["On"] = "开",
                    ["Off"] = "关",
                    ["Bilibili"] = "哔哩哔哩",
                    ["ckptPage1"] = "第一页：原版游戏设置",
                    ["ckptPage2"] = "第二页：他们的小镇设置",
                    ["ckptPage3"] = "第三页：内鬼职业设置",
                    ["ckptPage4"] = "第四页：中立职业设置",
                    ["ckptPage5"] = "第五页：船员职业设置",
                    ["ckptPage6"] = "第六页：附加职业设置",
                    ["ckptToOtherPages"] = "按下 TAB 键/数字键切换到下一页/指定页……",
                    ["cmdHelp"] = "欢迎游玩Among Us模组——他们的小镇！\n指令教程：\n/help——显示此消息\n/language——切换语言",
                    ["DebugMode"] = "调试模式",
                    ["HostSuggestName"] = "房主建议名称（可以在配置文件中自定义文本）",
                    ["HostSuggestName2"] = "模组名称+版本",
                    ["HostSuggestName3"] = "自定义",
                    ["CantPlayWithoutMod"] = "无法和原版玩家玩",
                    ["CustomGamemodes"] = "游戏模式",
                    ["Gamemode_Classic"] = "原版玩法",
                    ["Gamemode_BattleRoyale"] = "太空决斗",
                    ["PlayerCheckError1"] = "没有安装模组或安装了其它模组",
                    ["PlayerCheckError2"] = "安装了其他版本的Town Of Them或其它模组",
                };
                /*
            case SupportedLangs.Latam:
                break;
            case SupportedLangs.Brazilian:
                break;
            case SupportedLangs.Portuguese:
                break;
            case SupportedLangs.Korean:
                break;
            case SupportedLangs.Russian:
                break;
            case SupportedLangs.Dutch:
                break;
            case SupportedLangs.Filipino:
                break;
            case SupportedLangs.French:
                break;
            case SupportedLangs.German:
                break;
            case SupportedLangs.Italian:
                break;
            case SupportedLangs.Japanese:
                break;
            case SupportedLangs.Spanish:
                break;
            case SupportedLangs.TChinese:
                break;
            case SupportedLangs.Irish:
                break;
                */
            //template
            /*
            case SupportedLangs.YourLanguage:
                return new Dictionary<string, string>()
                {
                    ["ModLanguage"]="YourLanguageID",
                    ["Key1"]="TranslatedText1",
                    ["Key2"]="TranslatedText2",
                };
             */
            default:
                return new Dictionary<string, string>() { ["Error"] = "Error" };
        }
    }
    public static string LoadTranslation(string key)
    {
        try
        {
            string returnValue = Translations[key];
        }
        catch (KeyNotFoundException Error)
        {
            Log.LogError("Can not find translation key: " + key + " More Infomation: " + Error);
            return key;
        }
        //Log.LogInfo("Translation Loaded:" + key + "," + Translations[key]);
        return Translations[key];
    }

}
[HarmonyPatch(typeof(TranslationController), nameof(TranslationController.Initialize))]
class TrsCtrlInitPatch
{
    public static void Postfix(TranslationController __instance)
    {
        Translation.LoadLanguage(__instance.currentLanguage.languageID);
    }
}
[HarmonyPatch(typeof(TranslationController), nameof(TranslationController.SetLanguage))]
class TrsCtrlSetLanguagePatch
{
    public static bool Prefix(TranslationController __instance, TranslatedImageSet lang)
    {
        foreach (var l in __instance.Languages)
        {
            if (l.Value == lang)
            {
                Translation.LoadLanguage(l.Key);
                break;
            }
        }
        return true;
    }
}