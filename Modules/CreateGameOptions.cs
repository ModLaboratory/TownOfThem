using TownOfThem.CustomObjects;
using static TownOfThem.Language.Translation;

namespace TownOfThem.CreateCustomObjects 
{
    class CustomGameOptions
    {
        public static string[] rates = new string[] { "0%", "100%" };
        public static string[] ratesModifier = new string[] { "1", "2", "3" };
        public static string[] presets = new string[] { LoadTranslation("Preset1"), LoadTranslation("Preset2"), LoadTranslation("Preset3"), LoadTranslation("Preset4"), LoadTranslation("Preset5") };
        public static string[] hostName = new string[] { LoadTranslation("Off"), LoadTranslation("HostSuggestName2"), LoadTranslation("HostSuggestName3") };
        public static string[] gamemodes = new string[] { LoadTranslation("Gamemode_Classic"), LoadTranslation("Gamemode_BattleRoyale") };
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
        //jester
        public static CustomOption Jester;
        public static CustomOption JesterMaxPlayer;
        //handicapped
        public static CustomOption Handicapped;
        public static CustomOption HandicappedMaxPlayer;
        public static CustomOption HandicappedSpeed;
        public static void Load()
        {
            //host
            //HostName = CustomOption.Create(60, CustomOption.CustomOptionType.General, LoadTranslation("HostSuggestName"), hostName, null, true);
            //debug
            DebugMode = CustomOption.Create(50, CustomOption.CustomOptionType.General, "Debug Mode", false, null, true);
            
            //sheriff
            Sheriff = CustomOption.Create(100, CustomOption.CustomOptionType.Crewmate, LoadTranslation("Sheriff"), rates, null, true);
            SheriffMaxPlayer = CustomOption.Create(101, CustomOption.CustomOptionType.Crewmate, LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Sheriff, false);
            SheriffCooldown = CustomOption.Create(102, CustomOption.CustomOptionType.Crewmate, LoadTranslation("SheriffCD"), 20f, 10f, 60f, 0.5f, Sheriff, false);
            SheriffKillLimit = CustomOption.Create(103, CustomOption.CustomOptionType.Crewmate, LoadTranslation("SheriffKillLimit"), 2, 1, 5, 1, Sheriff, false);
            //jester
            Jester = CustomOption.Create(110, CustomOption.CustomOptionType.Neutral, LoadTranslation("Jester"), rates, null, true);
            JesterMaxPlayer = CustomOption.Create(111, CustomOption.CustomOptionType.Neutral, LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Jester, false);
            //handicapped
            Handicapped = CustomOption.Create(120, CustomOption.CustomOptionType.Modifier, LoadTranslation("Handicapped"), rates, null, true);
            HandicappedMaxPlayer = CustomOption.Create(121, CustomOption.CustomOptionType.Modifier, LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Handicapped, false);
            HandicappedSpeed = CustomOption.Create(121, CustomOption.CustomOptionType.Modifier, LoadTranslation("HandicappedSpeed"), 1, 1, 3, 0.25f, Handicapped, false);
            //gamemodes
            gameModes = CustomOption.Create(130, CustomOption.CustomOptionType.General, LoadTranslation("CustomGamemodes"), gamemodes, null, true);
        }
        public static void ReloadString()
        {

        }
    }
    
}