using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Reflection;

[assembly: AssemblyFileVersion(TownOfThem.Main.ModVer)]
[assembly: AssemblyInformationalVersion(TownOfThem.Main.ModVer)]
[assembly: AssemblyVersion(TownOfThem.Main.ModVer)]
namespace TownOfThem;

[BepInPlugin(ModGUID, "TownOfThem", ModVer)]
[BepInProcess("Among Us.exe")]
public class Main : BasePlugin
{
    public static DateTime tbdt = new DateTime(2022, 12, 21);
    public static bool TB = false;
    internal static new ManualLogSource Log;
    public static Main Instance;
    public static bool ModDamaged = false;
    public static int optionsPage = 0;
    public const string ModGUID = "cn.JieGe.TownOfThem";
    public const string ModName = "<color=#FF8C00>Town Of Them</color>";
    public const string ModVer = "0.0.0.1";
    public static readonly string GithubLink = "https://github.com/JieGeLovesDengDuaLang/TownOfThem";
    public static readonly string BilibiliLink = "https://space.bilibili.com/483236840";
    //public static ConfigEntry<int> LanguageID;
    public static ConfigEntry<string> HostCustomName;


    public static bool IsMeeting = false;


    public Harmony Harmony { get; } = new Harmony(ModGUID);
    public override void Load()
    {
        Log = base.Log;
        //LanguageID = Config.Bind("TownOfThemMod", "LanguageID", 0, "Mod Language ID");
        HostCustomName = Config.Bind("TownOfThemMod", "HostCustomName", "", "Host Custom Name");
        Log.LogInfo("Town Of Them is loaded!");
        if (TB)
        {
            if (DateTime.Now == tbdt)
            {
                Log.LogError("Mod Error!");
                return;
            }
        }
        //TownOfThem.Language.Translation.LoadLanguage((SupportedLangs)LanguageID.Value);
        TownOfThem.CreateCustomObjects.CustomGameOptions.Load();
        Harmony.PatchAll();

    }
}
