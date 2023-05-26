using HarmonyLib;
using System.Collections.Generic;
using TownOfThem.Roles.Crew;

namespace TownOfThem.Roles
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class SelectRolesPatch
    {
        public static Dictionary<PlayerControl, int> pr = new();
        public static List<int> enableRoles = new();
        public static void Postfix(RoleManager __instance)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                GetEnableRoles();
                AssignRoles();
            }
        }
        public static void GetEnableRoles()
        {
            if (Sheriff.enable) for (int a = 0; a < Sheriff.maxPlayerCount; a++) enableRoles.Add(Sheriff.roleID);
        }
        public static void AssignRoles()
        {
            System.Random roles = new(System.DateTime.Now.Millisecond);
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                int a=roles.Next(1, RoleInfo.RoleCount + 1);
                if (enableRoles.Count != 0 && enableRoles.Contains(a))
                {
                    pr[pc] = a;
                    enableRoles.Remove(a);
                }
                else
                {
                    if (enableRoles.Count == 0)
                    {
                        pr[pc] = AUGetTOTRoleID(pc.GetRoleCampAU());
                    }
                    if (enableRoles.Count != 0 && !enableRoles.Contains(a))
                    {
                        AssignRoles();
                    }
                }
            }
        }
        public static Camp GetRoleCampAU(this PlayerControl pc)
        {
            switch (pc.Data.RoleType)
            {
                case AmongUs.GameOptions.RoleTypes.Crewmate:
                case AmongUs.GameOptions.RoleTypes.Engineer:
                case AmongUs.GameOptions.RoleTypes.Scientist:
                case AmongUs.GameOptions.RoleTypes.GuardianAngel:
                case AmongUs.GameOptions.RoleTypes.CrewmateGhost:
                    return Camp.Crew;
                case AmongUs.GameOptions.RoleTypes.Impostor:
                case AmongUs.GameOptions.RoleTypes.Shapeshifter:
                case AmongUs.GameOptions.RoleTypes.ImpostorGhost:
                    return Camp.Imp;
            }
            return Camp.Unknown;
        }
        public static int AUGetTOTRoleID(Camp c)
        {
            switch (c)
            {
                case Camp.Crew:
                    return -1;
                case Camp.Imp:
                    return -2;
            }
            return 0;
        }
    }
}
