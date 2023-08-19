using System.Collections.Generic;
using TownOfThem.Modules;
using static TownOfThem.Modules.Translation;

namespace TownOfThem.Modules 
{
    public class CustomGameOptions
    {

        public static string[] rates = new string[] { "0%", "100%" };
        public static string[] ratesModifier = new string[] { "1", "2", "3" };
        public static string[] presets = new string[] { GetString("Preset1"), GetString("Preset2"), GetString("Preset3"), GetString("Preset4"), GetString("Preset5") };
        //public static string[] hostName = new string[] { GetString("Off"), GetString("HostSuggestName2"), GetString("HostSuggestName3") };
        public static string[] gamemodes = new string[] { GetString("Gamemode_Classic"), GetString("Gamemode_BattleRoyale") ,/*"Host Of Them"     planning*/ };
        //host
        //public static CustomOption HostName;
        //debug
        public static CustomOption DebugMode;
        //roleCount
        /*public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifiersCountMin;
        public static CustomOption modifiersCountMax;*/
        //gamemodes
        public static CustomOption gameModes;
        //sheriff
        public static CustomOption Sheriff;
        public static CustomOption SheriffMaxPlayer;
        public static CustomOption SheriffCooldown;
        public static CustomOption SheriffKillLimit;
        ////jester
        //public static CustomOption Jester;
        //public static CustomOption JesterMaxPlayer;
        ////handicapped
        //public static CustomOption Handicapped;
        //public static CustomOption HandicappedMaxPlayer;
        //public static CustomOption HandicappedSpeed;
        public static void Load()
        {
            //debug
            DebugMode = CustomOption.Create(50, CustomOptionType.General, "Debug Mode", false, null, true);

            //sheriff
            Sheriff = CustomOption.Create(100, CustomOptionType.Crewmate, Utils.ColorString(Roles.Crew.Sheriff.color, GetString("Sheriff")), false, null, true);
            SheriffMaxPlayer = CustomOption.Create(101, CustomOptionType.Crewmate, GetString("MaxPlayer"), 1, 1, 15, 1, Sheriff, false);
            SheriffCooldown = CustomOption.Create(102, CustomOptionType.Crewmate, GetString("SheriffCD"), 20f, 10f, 60f, 0.5f, Sheriff, false);
            SheriffKillLimit = CustomOption.Create(103, CustomOptionType.Crewmate, GetString("SheriffKillLimit"), 2, 1, 5, 1, Sheriff, false);
            
            //if (Main.EnableDevMode.Value)
            //{
            //    //jester
            //    Jester = CustomOption.Create(110, CustomOption.CustomOptionType.Neutral, cs(Roles.Neu.Jester.color, GetString("Jester")), false, null, true);
            //    JesterMaxPlayer = CustomOption.Create(111, CustomOption.CustomOptionType.Neutral, GetString("MaxPlayer"), 1, 1, 15, 1, Jester, false);
            //    //handicapped
            //    Handicapped = CustomOption.Create(120, CustomOption.CustomOptionType.Modifier, GetString("Handicapped"), false, null, true);
            //    HandicappedMaxPlayer = CustomOption.Create(121, CustomOption.CustomOptionType.Modifier, GetString("MaxPlayer"), 1, 1, 15, 1, Handicapped, false);
            //    HandicappedSpeed = CustomOption.Create(121, CustomOption.CustomOptionType.Modifier, GetString("HandicappedSpeed"), 1, 1, 3, 0.25f, Handicapped, false);
            //}
            
            //gamemodes
            gameModes = CustomOption.Create(130, CustomOptionType.General, GetString("CustomGamemodes"), gamemodes, null, true);
        }
    }
    
}