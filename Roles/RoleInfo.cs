using AmongUs.GameOptions;
using HarmonyLib;

namespace TownOfThem.Roles;

enum RoleId
{
    Sheriff = 100,

}
[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetRole))]
class RpcSetRolePatch
{
    public static void Prefix(PlayerControl __instance, RoleTypes roleType)
    {

    }
}
