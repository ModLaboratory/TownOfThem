using HarmonyLib;
using Sentry;
using System.Collections.Generic;
using System.Linq;
using TownOfThem.Modules;
using TownOfThem.Roles.Crew;

namespace TownOfThem.Roles
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class SelectRolesPatch
    {
        public static List<RoleId> enableCrewRoles = new();
        public static List<RoleId> enableNeuRoles = new();
        public static List<RoleId> enableImpRoles = new();
        public static void Postfix()
        {
            if (!AmongUsClient.Instance.AmHost) return;
            switch (CustomGameOptions.gameModes.GetSelection())
            {
                case 0:
                    GetEnableRoles();
                    AssignRoles();
                    foreach (var pair in RoleInfo.PlayerRoles) Main.Log.LogInfo($"{pair.Key.Data.PlayerName}: {pair.Value.ToString()}");
                    break;
                case 1:
                    break;
            }

        }
        public static void GetEnableRoles()
        {
            if (Sheriff.enable) for (int a = 0; a < Sheriff.maxPlayerCount; a++) enableCrewRoles.Add(Sheriff.RoleID);
            
        }
        static void AssignRoles()
        {
            System.Random role = new(System.DateTime.Now.Millisecond);
            foreach (var pc in PlayerControl.AllPlayerControls) pc.Assign(role);
            
        }
        static void Assign(this PlayerControl pc, System.Random role)
        {
            int roleIdx = -1;
            RoleId thisRole = RoleId.Unknown;
            var roles = GetDictionary(pc);
            if (roles.Count == 0) return;
            if (roles.Count == 1)
            {
                thisRole = roles.FirstOrDefault();
                goto AddRole;
            }
            roleIdx = role.Next(1, roles.Count);
            thisRole = roles[roleIdx];
        AddRole:
            roles.RemoveAt(roleIdx);
            pc.RpcSetRole(thisRole);
            Main.Log.LogInfo($"{pc.Data.PlayerName}: {thisRole.ToString()}");
        }

        static List<RoleId> GetDictionary(PlayerControl pc)
        {
            switch (pc.Data.Role.TeamType)
            {
                default:
                case RoleTeamTypes.Crewmate:
                    System.Random r = new();
                    int isCrew = r.Next(0, 2);
                    return isCrew == 0 ? enableNeuRoles : enableCrewRoles;
                case RoleTeamTypes.Impostor:
                    return enableImpRoles;
            }
        }

        
        public static int AUGetTOTRoleID(RoleTeamTypes t)
        {
            switch (t)
            {
                case RoleTeamTypes.Crewmate:
                    return (int)RoleId.Crewmate;
                case RoleTeamTypes.Impostor:
                    return (int)RoleId.Impostor;
            }
            return (int)RoleId.Unknown;
        }
    }
}
