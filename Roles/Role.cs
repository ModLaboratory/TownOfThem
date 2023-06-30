using System.Collections.Generic;
using TownOfThem.Roles.Crew;
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
    public abstract class Role
    {
        public static Color color;
        public static List<PlayerControl> players;
        public static int roleID;
        public static bool enable;
        public static int maxPlayerCount;
        public static Sprite button;
        public abstract void reset();
    }
}
