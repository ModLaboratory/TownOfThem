using System;
using System.Collections.Generic;
using static TownOfThem.Main;

namespace TownOfThem.Modules
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
        ExiledText,
        Ping,
        SheriffShoot,
        DevModeWarning,
        FunnyOk,
        BuildTime,
        ExpireTime,
        DeveloperMode,
        totOptions,
        test,
    }

    public static class Translation
    {
        public static bool init = false;
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
        public static Dictionary<StringKey, string> langDic = null;
        public static string GetString(string key)
        {
            return GetString((StringKey)Enum.Parse(typeof(StringKey), key));
        }
        public static string GetString(StringKey key)
        {
            string returnValue = "";
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
                return "<ERR_GET_TRSLARTION: " + key.ToString() + ">";
            }
            
            //Log.LogInfo("Translation Loaded:" + key + "," + Translations[key]);
            return returnValue;
        }
        
    }
    //给国内玩家：请不要随意修改翻译，如生草翻译等！
    //For other developers/translators: If you want to add your translation to Town Of Them, please fork this repo and create a new pull request, thanks 
    public static class Language
    {
        public static Dictionary<StringKey, string> GetLangDictionary(SupportedLangs lang)
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
        public static Dictionary<StringKey, string> English = new Dictionary<StringKey, string>()
        {
            [StringKey.ModInfo1] = "Programming: Jiege  Artist: Xiaolu",
            [StringKey.totBirthday] = "Happy Birthday To {0}!",
            [StringKey.totSettings] = "Town Of Them Settings ",
            [StringKey.ImpostorSettings] = "Impostor Roles Settings ",
            [StringKey.NeutralSettings] = "Neutral Roles Settings ",
            [StringKey.CrewmateSettings] = "Crewmate Roles Settings ",
            [StringKey.ModifierSettings] = "Modifier Settings ",
            [StringKey.Preset1] = "Preset 1",
            [StringKey.Preset2] = "Preset 2",
            [StringKey.Preset3] = "Preset 3",
            [StringKey.Preset4] = "Preset 4",
            [StringKey.Preset5] = "Preset 5",
            [StringKey.MaxPlayer] = "Max Player",
            [StringKey.Sheriff] = "Sheriff",
            [StringKey.SheriffCD] = "Sheriff Kill Cooldown",
            [StringKey.SheriffKillLimit] = "Sheriff Kill Limit",
            [StringKey.Jester] = "Jester",
            [StringKey.Handicapped] = "<color=#808080>Handicapped</color>",
            [StringKey.HandicappedSpeed] = "Handicapped speed",
            [StringKey.On] = "On",
            [StringKey.Off] = "Off",
            [StringKey.Bilibili] = "BILIBILI",
            [StringKey.ckptPage1] = "Page 1 : Original Game Settings",
            [StringKey.ckptPage2] = "Page 2 : Town Of Them Settings",
            [StringKey.ckptPage3] = "Page 3 : Impostor Role Settings",
            [StringKey.ckptPage4] = "Page 4 : Neutral Role Settings",
            [StringKey.ckptPage5] = "Page 5 : Crewmate Role Settings",
            [StringKey.ckptPage6] = "Page 6 : Modifier Settings",
            [StringKey.ckptToOtherPages] = "Press TAB or Page Number for more...",
            [StringKey.cmdHelp] = "Welcome to Among Us Mod---Town Of Them!\nChat command help:\n/help---Show me\n/language---Change language",
            [StringKey.DebugMode] = "Debug Mode",
            [StringKey.CantPlayWithoutMod] = "Can't Play With Players Who Don't Have Mod",
            [StringKey.CustomGamemodes] = "Gamemodes",
            [StringKey.Gamemode_Classic] = "Classic",
            [StringKey.Gamemode_BattleRoyale] = "Battle Royale",
            [StringKey.PlayerCheckError1] = "has no mod or other mods.",
            [StringKey.PlayerCheckError2] = "has a different version of TownOfThem or other mods.",
            [StringKey.ModExpired] = "The mod was expired. Click OK to quit game.",
            [StringKey.KickByHacking] = "You were banned by Innersloth.\n\nMaybe there is a BUG what will treat as hacking in this mod or you are hacking.",
            [StringKey.ExiledText] = "{0} was a {1}.\n{2}\n{3}",
            [StringKey.Ping] = "Ping: {0} ms",
            [StringKey.SheriffShoot] = "Shoot",
            [StringKey.DevModeWarning] = "You are playing Town Of Them on developer mode!\nIt will active some hidden things, but maybe it will make some trouble...",
            [StringKey.BuildTime] = "Build time: ",
            [StringKey.ExpireTime] = "Expire time: ",
            [StringKey.DeveloperMode] = "Developer Mode",
            [StringKey.totOptions] = "TOT Options",
            [StringKey.test] = "TEST",
        };
        public static Dictionary<StringKey, string> Chinese = new Dictionary<StringKey, string>()
        {
            [StringKey.ModInfo1] = "程序：杰哥  美术：小鹿",
            [StringKey.totBirthday] = "生日快乐，{0}！",
            [StringKey.totSettings] = "他们的小镇设置",
            [StringKey.ImpostorSettings] = "内鬼职业设置",
            [StringKey.NeutralSettings] = "中立职业设置",
            [StringKey.CrewmateSettings] = "船员职业设置",
            [StringKey.ModifierSettings] = "附加职业设置",
            [StringKey.Preset1] = "预设1",
            [StringKey.Preset2] = "预设2",
            [StringKey.Preset3] = "预设3",
            [StringKey.Preset4] = "预设4",
            [StringKey.Preset5] = "预设5",
            [StringKey.MaxPlayer] = "最大玩家数",
            [StringKey.Sheriff] = "警长",
            [StringKey.SheriffCD] = "警长击杀冷却",
            [StringKey.SheriffKillLimit] = "警长击杀次数",
            [StringKey.Jester] = "小丑",
            [StringKey.Handicapped] = "<color=#808080>残疾人</color>",
            [StringKey.HandicappedSpeed] = "残疾人速度",
            [StringKey.On] = "开",
            [StringKey.Off] = "关",
            [StringKey.Bilibili] = "哔哩哔哩",
            [StringKey.ckptPage1] = "第一页：原版游戏设置",
            [StringKey.ckptPage2] = "第二页：Town Of Them设置",
            [StringKey.ckptPage3] = "第三页：内鬼职业设置",
            [StringKey.ckptPage4] = "第四页：中立职业设置",
            [StringKey.ckptPage5] = "第五页：船员职业设置",
            [StringKey.ckptPage6] = "第六页：附加职业设置",
            [StringKey.ckptToOtherPages] = "按下 TAB 键/数字键切换到下一页/指定页……",
            [StringKey.cmdHelp] = "欢迎游玩Among Us模组——他们的小镇！\n指令教程：\n/help——显示此消息\n/language——切换语言",
            [StringKey.DebugMode] = "调试模式",
            [StringKey.CantPlayWithoutMod] = "无法和原版玩家玩",
            [StringKey.CustomGamemodes] = "游戏模式",
            [StringKey.Gamemode_Classic] = "原版玩法",
            [StringKey.Gamemode_BattleRoyale] = "太空决斗",
            [StringKey.PlayerCheckError1] = "没有安装模组或安装了其它模组",
            [StringKey.PlayerCheckError2] = "安装了其他版本的Town Of Them或其它模组",
            [StringKey.ModExpired] = "模组已过期。按下“确定”以退出游戏。",
            [StringKey.KickByHacking] = "你被树懒的反作弊系统踢了（树懒每日发癫1/1）。\n\n可能是一个bug引发了它，也有可能是你在开挂。",
            [StringKey.ExiledText] = "{0} 是 {1}.\n{2}\n{3}",
            [StringKey.Ping] = "延迟：{0} 毫秒",
            [StringKey.SheriffShoot] = "执法",
            [StringKey.DevModeWarning] = "您已启用了开发者模式。\n这会开启一些隐藏的功能，但也有可能会引发未知的错误……",
            [StringKey.BuildTime] = "构建时间：",
            [StringKey.ExpireTime] = "到期时间： ",
            [StringKey.DeveloperMode] = "开发者模式",
            [StringKey.totOptions] = "TOT 选项",
            [StringKey.test] = "测试",
        };
        
    }
}