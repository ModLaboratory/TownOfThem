using AmongUs.GameOptions;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace TownOfThem.Roles
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class SelectRolesPatch
    {

        public static void Postfix(RoleManager __instance)
        {
            System.Random role = new(System.DateTime.Now.Millisecond);
        }
    }
}
