using HarmonyLib;
using Hazel;
using Il2CppSystem.Web.Util;
using System;
using System.Linq;
using TownOfThem.CustomObjects;
using TownOfThem.Utilities;
using TownOfThem.Patch;
using TownOfThem.Roles;
using TownOfThem.Roles.Crew;
using System.Runtime.CompilerServices;

namespace TownOfThem
{
    public enum CustomRPC : uint
    {
        ShareOptions = 100,
        ShareModVersion,
        UncheckedMurderPlayer,
        SetRole,
        AssignRoles,
        ResetVariables,
    }

    class RPCProcedure
    {
        public static void HandleShareOptions(byte numberOfOptions, MessageReader reader)
        {
            try
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.First(option => option.id == (int)optionId);
                    option.updateSelection((int)selection);
                }
            }
            catch (Exception e)
            {
                Main.Log.LogError("Error while deserializing options: " + e.Message);
            }
        }
        public static void ShareModVersion(uint pcNetID, string ver)
        {
            SendModVer.playerVersion[pcNetID] = ver;
        }
        public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            PlayerControl source = ModHelpers.playerById(sourceId);
            PlayerControl target = ModHelpers.playerById(targetId);
            if (source != null && target != null)
            {
                if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
            }
        }
        public static void SetRole(byte target,int roleID)
        {
            var targetPC = ModHelpers.playerById(target);
            switch (roleID)
            {
                case (int)RoleId.Sheriff:
                    Sheriff.players.Add(targetPC);
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            byte packetId = callId;
            switch (packetId)
            {
                case (byte)CustomRPC.ShareOptions:
                    RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                    break;
                case (byte)CustomRPC.ShareModVersion:
                    uint netID = reader.ReadUInt32();
                    string ver = reader.ReadString();
                    RPCProcedure.ShareModVersion(netID, ver);
                    break;
                case (byte)CustomRPC.UncheckedMurderPlayer:
                    byte source = reader.ReadByte();
                    byte target = reader.ReadByte();
                    byte showAnimation = reader.ReadByte();
                    RPCProcedure.uncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case (byte)CustomRPC.SetRole:
                    byte roleTarget = reader.ReadByte();
                    int roleID = reader.ReadInt32();
                    break;
                case (byte)CustomRPC.AssignRoles:
                    int count = reader.ReadPackedInt32();
                    for(int a = 0; a < count; a++)
                    {
                        byte assignPlayerID=reader.ReadByte();
                        int assignRoleID=reader.ReadInt32();
                        SelectRolesPatch.pr[ModHelpers.playerById(assignPlayerID)] = assignRoleID;
                    }
                    break;
                default:
                    if (!Enum.IsDefined(typeof(RpcCalls), packetId))
                    {
                        Main.Log.LogError($"Unknown RPC: {packetId.ToString()}");
                    }
                    break;
            }
        }
    }

    public static class RPCHelper
    {
        public static void RPCSendModVersion(this PlayerControl pc, string version)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(pc.NetId, (byte)CustomRPC.ShareModVersion, SendOption.Reliable, -1);
            writer.Write(pc.NetId);
            writer.Write(version);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}