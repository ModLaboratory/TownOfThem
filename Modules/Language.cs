using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TownOfThem.Main;

namespace TownOfThem.Language
{
    public enum StringKey
    {
        ModInfo1,
        totBirthday,
        totSettings,
        ImpostorSettings,
        NeutralSettings,
        CrewmateSettings,
        ModifierSettings,
        Preset1,
        Preset2,
        Preset3,
        Preset4,
        Preset5,
        MaxPlayer,
        Sheriff,
        SheriffCD,
        SheriffKillLimit,
        Jester,
        Handicapped,
        HandicappedSpeed,
        On,
        Off,
        Bilibili,
        ckptPage1,
        ckptPage2,
        ckptPage3,
        ckptPage4,
        ckptPage5,
        ckptPage6,
        ckptToOtherPages,
        cmdHelp,
        DebugMode,
        HostSuggestName,
        HostSuggestName2,
        HostSuggestName3,
        CantPlayWithoutMod,
        CustomGamemodes,
        Gamemode_Classic,
        Gamemode_BattleRoyale,
        PlayerCheckError1,
        PlayerCheckError2,
        ModExpired,
        KickByHacking,
        ExiledPlayerIsJester,
    }

    public static class Translation
    {
        #region Obsoleted
        //public static Dictionary<string, string> Translations =new Dictionary<string,string>();
        //public static void LoadLanguage(SupportedLangs lang)
        //{
        //    Translations.Clear();
        //    Translations = AddLanguageText(lang);
        //    Log.LogInfo("Language Text Added!");
        //    if (Translations.ContainsKey("Error"))
        //    {
        //        Log.LogError("Unknown Language Load Error!");
        //    }
        //    if (Translations.Count == 0)
        //    {
        //        Log.LogError("Language Pack:" + lang + " Is Empty!");
        //        Translations.Clear();
        //        Translations = AddLanguageText(SupportedLangs.English);
        //        if (Translations.Count == 0)
        //        {
        //            Log.LogError("Hard Error:Language Pack" + SupportedLangs.English + " is Missing!");
        //            ModDamaged = true;
        //        }
        //    }
        //}
        //public static Dictionary<string,string> AddLanguageText(SupportedLangs id)
        //{
        //    bool ContainsLang = id == SupportedLangs.English || id == SupportedLangs.SChinese;//Add Your Languages Here
        //    Log.LogInfo("Language:"+id);
        //    if (!ContainsLang)
        //    {
        //        Log.LogError("Language " + id + " Not Support,Now Mod Language is:" + SupportedLangs.English);
        //        id = SupportedLangs.English;
        //    }
        //    switch (id)
        //    {
        //        case SupportedLangs.English:
        //            return new Dictionary<string, string>()
        //            {
        //                ["ModLanguage"] = "0",
        //                ["ModInfo1"] = "Modded By JieGe ",
        //                ["totBirthday"] = "Happy Birthday To ",
        //                ["totSettings"] = "Town Of Them Settings ",
        //                ["ImpostorSettings"] = "Impostor Roles Settings ",
        //                ["NeutralSettings"] = "Neutral Roles Settings ",
        //                ["CrewmateSettings"] = "Crewmate Roles Settings ",
        //                ["ModifierSettings"] = "Modifier Settings ",
        //                ["Preset1"] = "Preset 1",
        //                ["Preset2"] = "Preset 2",
        //                ["Preset3"] = "Preset 3",
        //                ["Preset4"] = "Preset 4",
        //                ["Preset5"] = "Preset 5",
        //                ["MaxPlayer"] = "Max Player",
        //                ["Sheriff"] = "<color=#F8CD46>Sheriff</color>",
        //                ["SheriffCD"] = "Sheriff Kill Cooldown",
        //                ["SheriffKillLimit"] = "Sheriff Kill Limit",
        //                ["Jester"] = "<color=#EC62A5>Jester</color>",
        //                ["Handicapped"] = "<color=#808080>Handicapped</color>",
        //                ["HandicappedSpeed"] = "Handicapped speed",
        //                ["On"] = "On",
        //                ["Off"] = "Off",
        //                ["Bilibili"] = "BILIBILI",
        //                ["ckptPage1"] = "Page 1 : Original Game Settings",
        //                ["ckptPage2"] = "Page 2 : Town Of Them Settings",
        //                ["ckptPage3"] = "Page 3 : Impostor Role Settings",
        //                ["ckptPage4"] = "Page 4 : Neutral Role Settings",
        //                ["ckptPage5"] = "Page 5 : Crewmate Role Settings",
        //                ["ckptPage6"] = "Page 6 : Modifier Settings",
        //                ["ckptToOtherPages"] = "Press TAB or Page Number for more...",
        //                ["cmdHelp"] = "Welcome to Among Us Mod---Town Of Them!\nChat command help:\n/help---Show me\n/language---Change language",
        //                ["DebugMode"] = "Debug Mode",
        //                ["HostSuggestName"] = "Host Suggest Name(You Can Add Custom Text In Config File)",
        //                ["HostSuggestName2"] = "Mod Name+Ver",
        //                ["HostSuggestName3"] = "Custom",
        //                ["CantPlayWithoutMod"] = "Can't Play With Players Who Don't Have Mod",
        //                ["CustomGamemodes"] = "Gamemodes",
        //                ["Gamemode_Classic"] = "Classic",
        //                ["Gamemode_BattleRoyale"] = "Battle Royale",
        //                ["PlayerCheckError1"] = "has no mod or other mods.",
        //                ["PlayerCheckError2"] = "has a different version of TownOfThem or other mods.",
        //            };
        //        case SupportedLangs.SChinese:
        //            return new Dictionary<string, string>()
        //            {
        //                ["ModLanguage"] = "13",
        //                ["ModInfo1"] = "杰哥制作",
        //                ["totBirthday"] = "生日快乐，",
        //                ["totSettings"] = "他们的小镇设置",
        //                ["ImpostorSettings"] = "内鬼职业设置",
        //                ["NeutralSettings"] = "中立职业设置",
        //                ["CrewmateSettings"] = "船员职业设置",
        //                ["ModifierSettings"] = "附加职业设置",
        //                ["Preset1"] = "预设1",
        //                ["Preset2"] = "预设2",
        //                ["Preset3"] = "预设3",
        //                ["Preset4"] = "预设4",
        //                ["Preset5"] = "预设5",
        //                ["MaxPlayer"] = "最大玩家数",
        //                ["Sheriff"] = "<color=#F8CD46>警长</color>",
        //                ["SheriffCD"] = "警长击杀冷却",
        //                ["SheriffKillLimit"] = "警长击杀次数",
        //                ["Jester"] = "<color=#EC62A5>小丑</color>",
        //                ["Handicapped"] = "<color=#808080>残疾人</color>",
        //                ["HandicappedSpeed"] = "残疾人速度",
        //                ["On"] = "开",
        //                ["Off"] = "关",
        //                ["Bilibili"]="哔哩哔哩",
        //                ["ckptPage1"] = "第一页：原版游戏设置",
        //                ["ckptPage2"] = "第二页：他们的小镇设置",
        //                ["ckptPage3"] = "第三页：内鬼职业设置",
        //                ["ckptPage4"] = "第四页：中立职业设置",
        //                ["ckptPage5"] = "第五页：船员职业设置",
        //                ["ckptPage6"] = "第六页：附加职业设置",
        //                ["ckptToOtherPages"] = "按下 TAB 键/数字键切换到下一页/指定页……",
        //                ["cmdHelp"] = "欢迎游玩Among Us模组——他们的小镇！\n指令教程：\n/help——显示此消息\n/language——切换语言",
        //                ["DebugMode"] = "调试模式",
        //                ["HostSuggestName"] = "房主建议名称（可以在配置文件中自定义文本）",
        //                ["HostSuggestName2"] = "模组名称+版本",
        //                ["HostSuggestName3"] = "自定义",
        //                ["CantPlayWithoutMod"] = "无法和原版玩家玩",
        //                ["CustomGamemodes"] = "游戏模式",
        //                ["Gamemode_Classic"] = "原版玩法",
        //                ["Gamemode_BattleRoyale"] = "太空决斗",
        //                ["PlayerCheckError1"] = "没有安装模组或安装了其它模组",
        //                ["PlayerCheckError2"] = "安装了其他版本的Town Of Them或其它模组",
        //            };
        //        //template
        //        /*
        //        case SupportedLangs.YourLanguage:
        //            return new Dictionary<string, string>()
        //            {
        //                ["ModLanguage"]="YourLanguageID",
        //                ["Key1"]="TranslatedText1",
        //                ["Key2"]="TranslatedText2",
        //            };
        //         */
        //        default:
        //            return new Dictionary<string, string>() { ["Error"]="Error" };
        //    }
        //}
        #endregion
        public static Dictionary<string, string> langDic = null;
        public static string GetString(StringKey key)
        {
            return GetString(key.ToString());
        }
        public static string GetString(string key)
        {
            string returnValue = "";
            if (key == null)
            {
                Log.LogError("Error - Key is null");
                return "STRERR";
            }
            try
            {
                returnValue = langDic[key];
            }
            catch (Exception Error)
            {
                if(Error is KeyNotFoundException)
                {
                    Log.LogError($"Error loading translation - KeyNotFoundException:\r\nKey: {key}\r\nMore info: {Error.Message}");
                }
                else if(Error is NullReferenceException)
                {
                    Log.LogError($"Error loading translation - NullReferenceException:\r\nKey: {key}\r\nMore info: {Error.Message}");
                }
                else
                {
                    Log.LogError($"Error loading translation - Unknown exception:\r\nKey: {key}\r\nMore info: {Error.Message}");
                }
                return key;
            }
            
            //Log.LogInfo("Translation Loaded:" + key + "," + Translations[key]);
            return returnValue;
        }
        
    }
    //嘿兄弟，你都有这能力了，就别来改什么搞笑翻译、生草翻译之类的了吧？
    //开发者请忽视上面那段话
    public static class Language
    {
        public static Dictionary<string,string> GetLangDictionary(SupportedLangs lang)
        {
            switch (lang)
            {
                case SupportedLangs.English:
                    return English;
                case SupportedLangs.SChinese:
                    return Chinese;
                default:
                    return English;
            }
        }
        public static void SetLangDic(SupportedLangs lang)
        {
            langDic = GetLangDictionary(lang);
        }
        public static Dictionary<string, string> English = new Dictionary<string, string>()
        {
            ["ModInfo1"] = "Modded By JieGe ",
            ["totBirthday"] = "Happy Birthday To {0}!",
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
            ["Sheriff"] = "Sheriff",
            ["SheriffCD"] = "Sheriff Kill Cooldown",
            ["SheriffKillLimit"] = "Sheriff Kill Limit",
            ["Jester"] = "Jester",
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
            ["ModExpired"] = "The mod was expired. Click OK to quit game.",
            [StringKey.KickByHacking.ToString()] = "You were banned by Innersloth.\n\nMaybe there is a BUG what will treat as hacking in this mod or you are hacking.",
            [StringKey.ExiledPlayerIsJester.ToString()]="{0} was a {1}.\n\nI tricked y'all! Hahahahaha..."
        };
        public static Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            ["ModInfo1"] = "杰哥制作",
            ["totBirthday"] = "生日快乐，{0}！",
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
            ["ModExpired"] = "模组已过期。按下“确定”以退出游戏。",
            [StringKey.KickByHacking.ToString()] = "你被树懒的反作弊系统踢了（树懒每日发癫1/1）。\n\n可能是一个bug引发了它，也有可能是你在开挂。",
            [StringKey.ExiledPlayerIsJester.ToString()] = "{0} 是 {1}.\n\n不足挂齿的小把戏，你们真信啊！哈哈哈哈哈哈……"
        };
        
    }
}