using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using System;
using System.Runtime;
using BepInEx.Unity.IL2CPP;
using Reactor.Networking.Attributes;
using Reactor.Networking;

namespace TownOfThem
{
    
    [BepInPlugin(ModGUID, "TownOfThem", ModVer)]
    [BepInProcess("Among Us.exe")]
    public class Main : BasePlugin
    {

        public static int TBY = 0;
        public static int TBMon = 0;
        public static int TBD = 0;
        public static int TBH = 0;
        public static int TBMin = 0;
        public static int TBS = 0;
        public static bool TB = false;
        public static bool versionSent = false;
        internal static new ManualLogSource Log;
        public static Main Instance;
        public static bool ModDamaged = false;
        public static int optionsPage = 0;
        public const string ModGUID = "cn.JieGe.TownOfThem";
        public const string ModName = "<color=#FF8C00>Town Of Them</color>";
        public const string ModVer = "0.0.0.1";
        public static int IntModVer = 0;
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
            //TownOfThem.Language.Translation.LoadLanguage(TranslationController.Instance.currentLanguage.languageID);
            if (TB)
            {
                if(
                    DateTime.Now.Year>=TBY &&
                    DateTime.Now.Month>=TBMon&&
                    DateTime.Now.Day>=TBD&&
                    DateTime.Now.Hour>=TBH&&
                    DateTime.Now.Minute>=TBMin&&
                    DateTime.Now.Second>=TBS
                  )
                {
                    Main.Log.LogError("Mod Error!");
                    return;
                }
            }
            TownOfThem.Language.Translation.LoadLanguage((SupportedLangs)LanguageID.Value);
            TownOfThem.CreateCustomObjects.CustomGameOptions.Load();
            Harmony.PatchAll();

        }
    }
}
