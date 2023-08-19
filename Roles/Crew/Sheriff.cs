using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

                try
                {
                    p = PlayerControl.AllPlayerControls.ToArray().Where(p => p.GetRole() == RoleID).ToList();
                }
                catch
                {
                    return new();
                }

                return p;
            }
        }
        public static PlayerControl target;
        public static new RoleId RoleID
        {
            get;
        } = RoleId.Sheriff;
        public static new bool enable
        {
            get
            {
                return CustomGameOptions.Sheriff.GetBool();
            }
        }
        public static new int maxPlayerCount
        {
            get
            {
                return (int)CustomGameOptions.SheriffMaxPlayer.GetFloat();
            }
        }
        public static int cd
        {
            get
            {
                return (int)CustomGameOptions.SheriffCooldown.GetFloat();
            }
        }
        public static int limit {
            get
            {
                return (int)CustomGameOptions.SheriffKillLimit.selections[CustomGameOptions.SheriffKillLimit.selection];
            }
        }
        public override void reset()
        {
            target = null;
        }

        public static void SetTarget()
        {
            target = ExtPlayerControl.setTarget();
            target.SetPlayerOutline(color);
            
        }
        public static bool CheckTarget(PlayerControl target)
        {
            return target.IsImp();
        }
    }
}