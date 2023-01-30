using System;
using System.Collections.Generic;
using System.Text;
using static TownOfThem.CreateGameOptions.CustomGameOptions;

namespace TownOfThem.GetGameOptionsText
{
    class Cockpit_GetGameOptionText
    {
        public static string GetImpostorGameOptions()
        {
            
            string ImpostorOptions = "";
            return ImpostorOptions;
        }
        public static string GetNeutralGameOptions()
        {
            string JesterOptions = Jester.name + ":" + Jester.selection * 10 + "\n" + JesterMaxPlayer.name + ":" + JesterMaxPlayer.selection + "\n";
            
            string NeutralOptions = JesterOptions;
            return NeutralOptions;
        }
        public static string GetCrewmateGameOptions()
        {
            string SheriffOptions = Sheriff.name + ":" + Sheriff.selection * 10 + "%\n" + SheriffMaxPlayer.name + ":" + SheriffMaxPlayer.selection + "\n " + SheriffCooldown.name + ":" + SheriffCooldown.selection + "\n " + SheriffKillLimit.name + ":" + SheriffKillLimit.selection + "\n";
            
            string CrewmateOptions = SheriffOptions;
            return CrewmateOptions;
        }
        public static string GetModSettingGameOptions()
        {
            
            string ModSettingOptions = "";
            return ModSettingOptions;
        }
        public static string GetModifierGameOptions()
        {
            string HandicappedOptions = Handicapped.name + ":" + Handicapped.selection * 10 + "%\n " + HandicappedMaxPlayer.name + ":" + HandicappedMaxPlayer.selection + "\n";

            string ModifierOptions = HandicappedOptions;
            return ModifierOptions;
        }
    }
}
