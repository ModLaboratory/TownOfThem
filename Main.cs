global using Hazel;
global using HarmonyLib;
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
        internal static new ManualLogSource Log;
        public static Main Instance;
        public static int optionsPage = 0;
        public const string ModGUID = "cn.JieGe.TownOfThem";
        public const string ModName = "<size=130%><color=#FF8C00>Town Of Them</color></size>";
        public const string ModVer = "0.0.0.1";
        public static readonly Color ModColor = new(255, 140, 0);
        public static readonly string GithubLink = "https://github.com/TownOfThemAU/TownOfThem";
        public static readonly string BilibiliLink = "https://space.bilibili.com/483236840";


        public Harmony Harmony { get; } = new Harmony(ModGUID);
        public override void Load()
        {
            Log = base.Log;

            Log.LogInfo("========== Town Of Them loaded! ==========\r\n========== TOT，启动！ ==========");

            Harmony.PatchAll();
        }
    }
}
