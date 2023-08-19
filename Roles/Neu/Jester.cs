using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.Data;
using HarmonyLib;
using BepInEx;
using UnityEngine;

namespace TownOfThem.Roles.Neu
{
    public class Jester : Role
    {
        public static new Color color = new Color(236, 98, 165);
        public static new List<PlayerControl> players
        {
            get
            {
                List<PlayerControl> p = new();

                //foreach (var a in RoleInfo.PlayerRoles)
                //    if (a.Value == (RoleId)roleID)
                //        p.Add(a.Key);

                return p;
            }
        }
        public override void reset()
        { 

        }

        public static void OnExiled()
        {

        }

    }
}  


