using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
namespace TownOfThem
{
    
    [BepInPlugin("cn.JieGe.TownOfThem", "TownOfThem", "0.0.0.1")]
    [BepInProcess("Among Us.exe")]

    public class Main : BasePlugin
    {
        public const string ModGUID = "cn.JieGe.TownOfThem";
        public const string ModName = "Town Of Them";
        public const string ModVer = "0.0.0.1";
        public static readonly string GithubLink = "https://github.com/JieGeLovesDengDuaLang/TownOfThem";
        public Harmony Harmony { get; } = new Harmony(ModGUID);
        public override void Load()
        {
            Harmony.PatchAll();
        }
    }
}
