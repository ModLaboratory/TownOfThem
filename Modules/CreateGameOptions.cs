using TownOfThem.CustomGameOptions;

namespace TownOfThem.CreateGameOptions 
{
    class CustomGameOptions
    {
        public static string[] rates = new string[] { "0%", "100%" };
        public static string[] ratesModifier = new string[] { "1", "2", "3" };
        public static string[] presets = new string[] { TownOfThem.Language.Language.LoadTranslation("Preset1"), TownOfThem.Language.Language.LoadTranslation("Preset2"), TownOfThem.Language.Language.LoadTranslation("Preset3"), TownOfThem.Language.Language.LoadTranslation("Preset4"), TownOfThem.Language.Language.LoadTranslation("Preset5") };
        public static string[] hostName = new string[] { TownOfThem.Language.Language.LoadTranslation("Off"), TownOfThem.Language.Language.LoadTranslation("HostSuggestName2"), TownOfThem.Language.Language.LoadTranslation("HostSuggestName3") };
        //host
        public static CustomOption HostName;
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
        public static void Load()
        {
            //host
            HostName = CustomOption.Create(60, CustomOption.CustomOptionType.General, TownOfThem.Language.Language.LoadTranslation("HostSuggestName"), hostName, null, true);
            //debug
            DebugMode = CustomOption.Create(50, CustomOption.CustomOptionType.General, "Debug Mode", false, null, true);
            //sheriff
            Sheriff = CustomOption.Create(100, CustomOption.CustomOptionType.Crewmate, TownOfThem.Language.Language.LoadTranslation("Sheriff"), rates, null, true);
            SheriffMaxPlayer = CustomOption.Create(101, CustomOption.CustomOptionType.Crewmate, TownOfThem.Language.Language.LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Sheriff, false);
            SheriffCooldown = CustomOption.Create(102, CustomOption.CustomOptionType.Crewmate, TownOfThem.Language.Language.LoadTranslation("SheriffCD"), 20, 10, 60, 5, Sheriff, false);
            SheriffKillLimit = CustomOption.Create(103, CustomOption.CustomOptionType.Crewmate, TownOfThem.Language.Language.LoadTranslation("SheriffKillLimit"), 2, 1, 5, 1, Sheriff, false);
            //jester
            Jester = CustomOption.Create(110, CustomOption.CustomOptionType.Neutral, TownOfThem.Language.Language.LoadTranslation("Jester"), rates, null, true);
            JesterMaxPlayer = CustomOption.Create(111, CustomOption.CustomOptionType.Neutral, TownOfThem.Language.Language.LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Jester, false);
            //handicapped
            Handicapped = CustomOption.Create(120, CustomOption.CustomOptionType.Modifier, TownOfThem.Language.Language.LoadTranslation("Handicapped"), rates, null, true);
            HandicappedMaxPlayer = CustomOption.Create(121, CustomOption.CustomOptionType.Modifier, TownOfThem.Language.Language.LoadTranslation("MaxPlayer"), 1, 1, 15, 1, Handicapped, false);
        }
    }
    
}