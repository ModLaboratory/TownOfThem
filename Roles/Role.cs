using System.Collections.Generic;
using UnityEngine;

namespace TownOfThem.Roles
{
    public enum RoleId
    {
        Crewmate = -1,
        Impostor = -2,
        Unknown = -3,
        Sheriff = 1,

    }
    public enum Camp
    {
        Crew,
        Neu,
        Imp,
        Unknown,
    }
    class RoleInfo
    {
        public static Dictionary<PlayerControl, RoleId> PlayerRoles = new();
    }
    public abstract class Role
    {
        public static Color color;
        public static List<PlayerControl> players;
        public static RoleId RoleID;
        public static bool enable;
        public static int maxPlayerCount;
        public static Sprite button;
        public abstract void reset();
    }
}
