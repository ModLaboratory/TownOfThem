using HarmonyLib;
using TownOfThem.CreateCustomObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Sentry;

namespace TownOfThem.IntroPatch
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    class ImpostorIntroPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            
        }

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
    class CrewmateIntroPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            switch (CustomGameOptions.gameModes.selection)
            {
                case 0:
                    break;
                case 1:
                    //__instance.TeamTitle.text = PlayerControl.LocalPlayer.Data.PlayerName;
                    __instance.TeamTitle.text = "teamtitle";
                    __instance.YouAreText.text = "youaretext";
                    __instance.RoleBlurbText.text = "roleblurb";
                    __instance.RoleText.text = "roletext";
                    
                    break;
            };
        }

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    class ShowRolePatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            switch (CustomGameOptions.gameModes.selection)
            {
                case 0:
                    break;
                case 1:
                    /*__instance.YouAreText.text = PlayerControl.LocalPlayer.Data.PlayerName;
                    __instance.RoleBlurbText.text = "成为最后一个站着的男人！";
                    __instance.YouAreText.text = "youaretext";*/
                    __instance.RoleBlurbText.text = "roleblurb";
                    break;
            };
            ;
        }
    }

}
