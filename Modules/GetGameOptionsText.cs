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
    class GetGameOptionText
    {
        public static string GetOptByType(CustomOptionType type)
        {
            string txt = "";
            List<CustomOption> opt= new();
            foreach(var option in options)
            {
                if (option.type == type)
                {
                    opt.Add(option);
                }
            }
            foreach(var option in opt)
            {
                txt += option.name + ": " + option.selections[option.selection].ToString() + "\n";
            }
            return txt;
        }

    }
}
