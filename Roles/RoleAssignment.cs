using HarmonyLib;
using Sentry;
using System.Collections.Generic;
using TownOfThem.Roles.Crew;

namespace TownOfThem.Roles
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class SelectRolesPatch
    {
        public static Dictionary<PlayerControl, int> pr = new();
        public static List<int> enableCrewRoles = new();
        public static void Postfix(RoleManager __instance)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                GetEnableRoles();
                AssignRoles();
                RPCAssignRoles();
            }
        }
        public static void GetEnableRoles()
        {
            if (Sheriff.enable) for (int a = 0; a < Sheriff.maxPlayerCount; a++) enableCrewRoles.Add(Sheriff.roleID);

        }
        public static void AssignRoles()
        {
            System.Random role = new(System.DateTime.Now.Millisecond);
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                pc.AssignByRoleCamp(role, enableCrewRoles);

            }
        }
        public static void AssignByRoleCamp(this PlayerControl pc, System.Random role, List<int> roles)
        {
            Assign:
            //随机职业ID
            int a = role.Next(0, roles.Count - 1);
            //如果未分配完成，分配职业并删除待分配职业中的此职业
            if (roles.Count != 0)
            {
                pr[pc] = a;
                roles.Remove(a);
            }
            else
            {
                if (roles.Count == 0)
                {
                    //职业分配完毕，开始将未分配到职业的玩家的aus职业加入玩家职业字典
                    pr[pc] = AUGetTOTRoleID(pc.GetRoleCampAU());
                }
                if (roles.Count != 0 && !roles.Contains(a))
                {
                    //随机到的职业不在未分配职业列表，再随机一次
                    goto Assign;
                }
            }
        }
        public static void RPCAssignRoles()
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.AssignRoles, SendOption.Reliable, -1);
            writer.WritePacked(pr.Count);
            foreach (var pair in pr)
            {
                writer.Write(pair.Key.PlayerId);
                writer.Write(pair.Value);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
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
