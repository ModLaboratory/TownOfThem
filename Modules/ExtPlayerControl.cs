using System.Collections.Generic;
using System.Linq;
using TownOfThem.Roles;
using TownOfThem.Utilities;
using UnityEngine;

namespace TownOfThem.Modules
{
    public static class ExtPlayerControl
    {
        public static PlayerControl setTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false, List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null)
        {
            PlayerControl result = null;
            float num = AmongUs.GameOptions.GameOptionsData.KillDistances[Mathf.Clamp(GameOptionsManager.Instance.currentNormalGameOptions.KillDistance, 0, 2)];
            if (targetingPlayer == null) targetingPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            if (targetingPlayer.Data.IsDead) return result;

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            foreach (var playerInfo in GameData.Instance.AllPlayers)
            {
                if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && !playerInfo.IsDead && (!onlyCrewmates || !playerInfo.Role.IsImpostor))
                {
                    PlayerControl @object = playerInfo.Object;
                    if (untargetablePlayers != null && untargetablePlayers.Any(x => x == @object))
                    {
                        // if that player is not targetable: skip check
                        continue;
                    }

                    if (@object && (!@object.inVent || targetPlayersInVents))
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return result;
        }
        public static void setPlayerOutline(this PlayerControl target, Color color)
        {
            if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) return;



            target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
            target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
        }
        public static bool IsImp(this PlayerControl pc)
        {
            return GetCamp(GetRole(pc)) == Camp.Imp;
        }
        public static RoleId GetRole(this PlayerControl pc)
        {
            return (RoleId)SelectRolesPatch.pr[pc];
        }
        public static Camp GetCamp(this RoleId role)
        {
            return role switch
            {
                RoleId.Crewmate => Camp.Crew,
                RoleId.Impostor => Camp.Imp,
                RoleId.Sheriff => Camp.Crew,

                _ => Camp.Crew,
            };
        }
    }
}
