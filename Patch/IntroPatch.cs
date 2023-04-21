﻿using Cpp2IL.Core.Analysis.Actions.x86.Important;
using HarmonyLib;
using System;
using TownOfThem.CreateCustomObjects;
using TownOfThem.Utilities;

namespace TownOfThem.Patch
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    class ImpostorIntroPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            switch (CustomGameOptions.gameModes.selection)
            {
                case 0:
                    break;
                case 1:
                    __instance.TeamTitle.text = PlayerControl.LocalPlayer.Data.PlayerName;
                    __instance.YouAreText.text = "youaretext";
                    __instance.RoleBlurbText.text = "roleblurb";
                    __instance.RoleText.text = "roletext";

                    break;
            };
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
                    __instance.TeamTitle.text = PlayerControl.LocalPlayer.Data.PlayerName;
                    __instance.RoleText.text = "roletext";
                    break;
            };
        }

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    class ShowRolePatch
    {
        public static void SetRoleTexts(IntroCutscene __instance)
        {
            switch (CustomGameOptions.gameModes.selection)
            {
                case 0:
                    break;
                case 1:
                    __instance.YouAreText.text = PlayerControl.LocalPlayer.Data.PlayerName;
                    __instance.RoleBlurbText.text = "成为最后一个站着的男人！";
                    __instance.RoleText.text = "太空决斗";
                    break;
            }
        }
        public static bool Prefix(IntroCutscene __instance)
        {
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => { SetRoleTexts(__instance); })));
            return true;
            
        }
    }
    

}