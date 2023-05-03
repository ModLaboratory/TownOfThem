using System;
using System.Collections.Generic;
using System.Text;
using static TownOfThem.CreateCustomObjects.CustomGameOptions;
using TownOfThem.CustomObjects;
using static TownOfThem.Language.Translation;
using static TownOfThem.CustomObjects.CustomOption;
using Il2CppSystem.Runtime.InteropServices;

namespace TownOfThem.GetGameOptionsText
{
    class Cockpit_GetGameOptionText
    {
        [Obsolete("我是傻逼，写完才发现这玩意兼容性不行\r\n还是留着这玩意吧，以后拿来改", true)]
        public static string GetRoleOptions(CustomOptionType type)
        {
            
            string result = "";
            List<CustomOption> ty = new List<CustomOption>();
            List<CustomOption> tyKey = new List<CustomOption>();
            List<CustomOption> tyValue = new List<CustomOption>();
            Dictionary<CustomOption, List<CustomOption>> tyT = new Dictionary<CustomOption, List<CustomOption>>();
            foreach (CustomOption option in options)
            {
                if (option.type == type)
                {
                    ty.Add(option);
                }
            }
            if (ty.Count == 0) return "";
            foreach (CustomOption tyOption in ty)
            {
                if (tyOption.isHeader)
                {
                    tyKey.Add(tyOption);
                }
                else
                {
                    tyValue.Add(tyOption);
                }
            }
            List<CustomOption> tyTemp = new List<CustomOption>();
            foreach (CustomOption tyH in tyKey)
            {
                tyTemp.Clear();
                foreach(CustomOption tyD in tyValue)
                {
                    if (tyD.parent == tyH)
                    {
                        tyTemp.Add(tyD);
                    }
                }
                tyT.Add(tyH, tyTemp);
            }
            foreach(var item in tyT)
            {
                result += item.Key.name + ":" + item.Key.selection * 100 + "%\n";
                foreach(var item2 in item.Value)
                {
                    result += item2.name + ":" + item2.selection + "%\n";
                }
            }
            return result;
        }
        public static string GetImpostorGameOptions()
        {
            string ImpostorOptions = "";
            return ImpostorOptions;
        }
        public static string GetNeutralGameOptions()
        {
            string JesterOptions =
                Jester.name + ":" +
                Jester.selection * 100 + "%\n" +
                JesterMaxPlayer.name + ":" +
                JesterMaxPlayer.selection + "\n";

            string NeutralOptions = JesterOptions;
            return NeutralOptions;
        }
        public static string GetCrewmateGameOptions()
        {
            string SheriffOptions =
                Sheriff.name + ":" +
                Sheriff.selection * 100 + "%\n" +
                SheriffMaxPlayer.name + ":" + SheriffMaxPlayer.selection + "\n " +
                SheriffCooldown.name + ":" + SheriffCooldown.selection + "\n " +
                SheriffKillLimit.name + ":" + SheriffKillLimit.selection + "\n";

            string CrewmateOptions = SheriffOptions;
            return CrewmateOptions;
        }
        public static string GetModSettingGameOptions()
        {

            //string ModSettingOptions =
            //    HostName.name + ":" + (HostName.getBool() ? LoadTranslation("On") : LoadTranslation("Off")) + "\n" + 
            //    DebugMode.name + ":"+ (DebugMode.getBool() ? LoadTranslation("On") : LoadTranslation("Off")) + "\n";
            return "";
        }
        public static string GetModifierGameOptions()
        {
            string HandicappedOptions = 
                Handicapped.name + ":" +
                Handicapped.selection * 100 + "%\n " +
                HandicappedMaxPlayer.name + ":" + HandicappedMaxPlayer.selection + "\n";

            string ModifierOptions = HandicappedOptions;
            return ModifierOptions;
        }

    }
}
