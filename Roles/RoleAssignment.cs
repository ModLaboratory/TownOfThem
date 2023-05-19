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
            //System.Random role = new(System.DateTime.Now.Millisecond);
            if (AmongUsClient.Instance.AmHost)
            {
                //This client is host: Start assigning
                //如果本地客户端是房主的话，就开始分配

            }
            else
            {
                //Or show "Waiting for host to assign roles..."
                //不是的话就显示“正在等待房主分配职业……”
            }
        }
    }
}
