using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using System;

namespace TownOfThem
{
    
    [BepInPlugin("cn.JieGe.TownOfThem", "TownOfThem", "0.0.0.1")]
    [BepInProcess("Among Us.exe")]
    public class Main : BasePlugin
    {
        internal static new ManualLogSource Log;
        public static Main Instance;
        public static bool ModDamaged = false;
        public static int optionsPage = 0;
        public const string ModGUID = "cn.JieGe.TownOfThem";
        public const string ModName = "<color=#FF8C00>Town Of Them</color>";
        public const string ModVer = "0.0.0.1";
        public static readonly string GithubLink = "https://github.com/JieGeLovesDengDuaLang/TownOfThem";
        public static readonly string BilibiliLink = "https://space.bilibili.com/483236840";
        public static ConfigEntry<int> LanguageID;
        public static ConfigEntry<string> HostCustomName;

        public Harmony Harmony { get; } = new Harmony(ModGUID);
        public override void Load()
        {
            Main.Log = base.Log;
            LanguageID = Config.Bind("TownOfThemMod", "LanguageID", 0, "Mod Language ID");
            HostCustomName = Config.Bind("TownOfThemMod", "HostCustomName", "", "Host Custom Name");
            Main.Log.LogInfo("Town Of Them is loaded!");
            Harmony.PatchAll();
            TownOfThem.Language.Language.LoadLanguage((SupportedLangs)TownOfThem.Main.LanguageID.Value);
            TownOfThem.CreateGameOptions.CustomGameOptions.Load();
        }
    }
}
