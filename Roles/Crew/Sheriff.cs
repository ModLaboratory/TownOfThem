using System.Collections.Generic;
using TownOfThem.Modules;
using UnityEngine;

namespace TownOfThem.Roles.Crew
{
    public class Sheriff : Role
    {
        public static new Color color = new Color(248, 205, 70);
        public static new Sprite button = ModHelpers.LoadSprite("TownOfThem.Resources.SheriffKillButton.png");
        public static new List<PlayerControl> players
        {
            get
            {
                List<PlayerControl> p = new();
                foreach(var a in SelectRolesPatch.pr)
                {
                    if (a.Value == roleID)
                    {
                        p.Add(a.Key);
                    }
                }
                return p;
            }
        }
        public static PlayerControl target;
        public static new int roleID
        {
            get
            {
                return (int)RoleId.Sheriff;
            }
        }
        public static new bool enable
        {
            get
            {
                return Modules.CustomGameOptions.Sheriff.getBool();
            }
        }
        public static new int maxPlayerCount
        {
            get
            {
                return (int)Modules.CustomGameOptions.SheriffMaxPlayer.selections[Modules.CustomGameOptions.SheriffMaxPlayer.selection];
            }
        }
        public static int cd
        {
            get
            {
                return (int)Modules.CustomGameOptions.SheriffCooldown.selections[Modules.CustomGameOptions.SheriffCooldown.selection];
            }
        }
        public static int limit {
            get
            {
                return (int)Modules.CustomGameOptions.SheriffKillLimit.selections[Modules.CustomGameOptions.SheriffKillLimit.selection];
            }
        }
        public override void reset()
        {
            target = null;
        }

        public static void SetTarget()
        {
            target = ExtPlayerControl.setTarget();
            ExtPlayerControl.setPlayerOutline(target, color);
            
        }
        public static bool CheckTarget(PlayerControl target)
        {
            return target.IsImp();
        }
    }
}