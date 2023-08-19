using System.Collections.Generic;
using System.Linq;
using TownOfThem.Roles;
using UnityEngine;

namespace TownOfThem.Modules
{
    public static class ExtPlayerControl
    {
        public static PlayerControl setTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false, List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null)
        {
            PlayerControl result = null;
            float num = AmongUs.GameOptions.GameOptionsData.KillDistances[Mathf.Clamp(GameOptionsManager.Instance.currentNormalGameOptions.KillDistance, 0, 2)];
            if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
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
        public static void SetPlayerOutline(this PlayerControl target, Color color)
        {
            if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) return;



            target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
            target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
        }
        public static bool IsImp(this PlayerControl pc)
        {
            return GetRole(pc).GetCamp() == Camp.Imp;
        }
        public static RoleId GetRole(this PlayerControl pc)
        {
            var role = RoleId.Unknown;
            try
            {
                role = RoleInfo.PlayerRoles[pc];
            }
            catch { }
            return role;

        }
        public static void SetRole(this PlayerControl pc, RoleId role)
        {
            RoleInfo.PlayerRoles[pc] = role;
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
        public static void RpcSetRole(this PlayerControl pc, RoleId role)
        {
            CustomRpcSender sender = new(pc, CustomRPC.SetRole);
            sender.Write(pc.PlayerId).Write((int)role).EndRpc();
            pc.SetRole(role);
        }
        public static void RpcSendModVersion(this PlayerControl pc, string version)
        {
            CustomRpcSender writer = new(pc, CustomRPC.ShareModVersion);
            writer.Write(pc.NetId).Write(version).EndRpc();
        }
        public static void RpcUncheckedStartMeeting(this PlayerControl pc)
        {
            CustomRpcSender writer = new(pc, RpcCalls.StartMeeting);
            writer.EndRpc();
        }
        public static void RpcUncheckedMurderPlayer(this PlayerControl source, PlayerControl target, bool showAnimation = true)
        {
            CustomRpcSender writer = new(source, CustomRPC.UncheckedMurderPlayer);
            writer.Write(source.PlayerId).Write(target.PlayerId).Write(showAnimation).EndRpc();

        }
        //public static void RpcUncheckedMurderPlayer(this PlayerControl source, PlayerControl target)
        //{
        //    CustomRpcSender writer = new(source, RpcCalls.MurderPlayer);
        //    writer.Write(target.NetId).EndRpc();
        //}
    }
}
