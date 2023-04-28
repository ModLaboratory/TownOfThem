using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppSystem.Xml.Schema;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace TownOfThem.Roles
{
    enum RoleId
    {
        Sheriff = 100,

    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetRole))]
    class RpcSetRolePatch
    {
        public static void Prefix(PlayerControl __instance,RoleTypes roleType)
        {

        }
    }

}
