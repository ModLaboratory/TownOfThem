using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace TownOfThem
{
    [BepInPlugin("cn.JieGe.TownOfThem", "Town Of Them", "0.0.0.1")]
    [BepInProcess("Among Us.exe")]
    public class Plugin : BasePlugin
    {
        public const string ModName = "Town Of Them";
        public const string ModVer = "0.0.0.1";
        public const string ModMaker = "JieGe";
        public const bool IsDev = true;

        public override void Load()
        {
            var TownOfThemMessage = new ManualLogSource("Town Of Them");
            BepInEx.Logging.Logger.Sources.Add(TownOfThemMessage);
            TownOfThemMessage.LogInfo($"{TownOfThem.Plugin.ModName} is loaded!");
            if (IsDev) TownOfThemMessage.LogWarning($"v{TownOfThem.Plugin.ModVer} is Dev version!");
        }
    }
}