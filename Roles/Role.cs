using UnityEngine;

namespace TownOfThem.Roles
{
    enum RoleId
    {
        Crewmate = -1,
        Impostor = -2,
        Unknown = -3,
        Sheriff = 1,

    }
    enum Camp
    {
        Crew,
        Neu,
        Imp,
        Unknown,
    }
    class RoleInfo
    {
        public static int RoleCount = 1;
    }
}
