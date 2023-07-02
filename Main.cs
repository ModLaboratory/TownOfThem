global using Hazel;
global using static TownOfThem.Modules.Translation;
global using TownOfThem.Modules;
global using HarmonyLib;
global using TownOfThem.Utilities;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using BepInEx.Configuration;
using System;
using System.Runtime;
using BepInEx.Unity.IL2CPP;
using System.Text;
using UnityEngine;

namespace TownOfThem
{
    
    [BepInPlugin(ModGUID, "TownOfThem", ModVer)]
    [BepInProcess("Among Us.exe")]
    public class Main : BasePlugin
    {

        public static DateTime ExpireTime = new DateTime(2023, 12, 31, 23, 59, 59);
        public static DateTime BuildTime = new DateTime(2023, 6, 27);
        public static bool IsBeta = true;
        internal static new ManualLogSource Log;
        public static Main Instance;
        public static bool ModDamaged = false;
        public static int optionsPage = 0;
        public const string ModGUID = "cn.JieGe.TownOfThem";
        public const string ModName = "<size=130%><color=#FF8C00>Town Of Them</color></size>";
        public const string ModVer = "0.0.0.1";
        public static readonly string GithubLink = "https://github.com/TownOfThemAU/TownOfThem";
        public static readonly string BilibiliLink = "https://space.bilibili.com/483236840";
        public static ConfigEntry<bool> EnableDevMode;


        public static bool IsMeeting = false;


        public Harmony Harmony { get; } = new Harmony(ModGUID);
        public override void Load()
        {
            Log = base.Log;
            EnableDevMode = Config.Bind("TownOfThemMod", "EnableDevMode", false, "Enable developer mode");

            Log.LogInfo("========== Town Of Them loaded! ==========");

            DataManager.Init();
            Harmony.PatchAll();
        }
    }
}
