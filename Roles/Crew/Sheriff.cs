using System.Collections.Generic;
using UnityEngine;

namespace TownOfThem.Roles.Crew
{
    public static class Sheriff
    {
        public static Color color = new Color32(248, 205, 70, byte.MaxValue);
        public static List<PlayerControl> players;
        public static PlayerControl target;
        public static int roleID => (int)RoleId.Sheriff;
        public static bool enable => (bool)CreateCustomObjects.CustomGameOptions.Sheriff.getBool();
        public static int maxPlayerCount => (int)CreateCustomObjects.CustomGameOptions.SheriffMaxPlayer.selections[CreateCustomObjects.CustomGameOptions.SheriffMaxPlayer.selection];
        public static int cd => (int)CreateCustomObjects.CustomGameOptions.SheriffCooldown.selections[CreateCustomObjects.CustomGameOptions.SheriffCooldown.selection];
        public static int limit => (int)CreateCustomObjects.CustomGameOptions.SheriffKillLimit.selections[CreateCustomObjects.CustomGameOptions.SheriffKillLimit.selection];
        public static void reset()
        {

        }
    }
}