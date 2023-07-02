using Il2CppInterop.Runtime.InteropTypes.Arrays;
using InnerNet;
using System;
using System.Linq;
using TownOfThem.Patches;
using TownOfThem.Roles;
using TownOfThem.Roles.Crew;

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
        UncheckedStartMeeting,
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
        public static void UncheckedMurderPlayer(byte sourceId, byte targetId, bool showAnimation)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            PlayerControl source = ModHelpers.playerById(sourceId);
            PlayerControl target = ModHelpers.playerById(targetId);
            if (source != null && target != null)
            {
                if (showAnimation) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
            }
        }
        public static void SetRole(byte target, int roleID)
        {
            var targetPC = ModHelpers.playerById(target);
            switch (roleID)
            {
                case (int)RoleId.Sheriff:
                    Sheriff.players.Add(targetPC);
                    break;
            }
        }
        public static void UncheckedStartMeeting(byte source)
        {
            PlayerControl.LocalPlayer.StartMeeting(ModHelpers.playerById(source).Data);
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
                    bool showAnimation = reader.ReadBoolean();
                    RPCProcedure.UncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case (byte)CustomRPC.SetRole:
                    byte roleTarget = reader.ReadByte();
                    int roleID = reader.ReadInt32();
                    break;
                case (byte)CustomRPC.AssignRoles:
                    int count = reader.ReadPackedInt32();
                    for(int a = 0; a < count; a++)
                    {
                        byte assignPlayerID = reader.ReadByte();
                        int assignRoleID = reader.ReadInt32();
                        SelectRolesPatch.pr[ModHelpers.playerById(assignPlayerID)] = assignRoleID;
                    }
                    break;
                case (byte)CustomRPC.ResetVariables:
                    
                    break;
                case (byte)CustomRPC.UncheckedStartMeeting:
                    byte meetingStartPlayer = reader.ReadByte();
                    RPCProcedure.UncheckedStartMeeting(meetingStartPlayer);
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
        public static void RpcSendModVersion(this PlayerControl pc, string version)
        {
            try
            {
                CustomRpcSender writer = new(pc, CustomRPC.ShareModVersion);
                writer.Write(pc.NetId).Write(version).EndRpc();
            }
            catch
            {
                Main.Log.LogError($"Error sending mod version! If you are local playing, please ignore this error.\r\nSender: {pc?.Data?.PlayerName} Version: {version}");
            }
            
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
        public static void RpcUncheckedMurderPlayer(this PlayerControl source, PlayerControl target)
        {
            CustomRpcSender writer = new(source, RpcCalls.MurderPlayer);
            writer.Write(target.NetId).EndRpc();
        }
    }
    public class CustomRpcSender
    {
        private MessageWriter writer;
        public bool WasEnded = false;
        public CustomRpcSender(PlayerControl source, CustomRPC rpc, int target = -1, SendOption sendOption = SendOption.Reliable)
        {
            writer = AmongUsClient.Instance.StartRpcImmediately(source.NetId, (byte)rpc, sendOption, target);
            Main.Log.LogInfo($"Rpc Started: {source.NetId}, {target}, {rpc.ToString()}, {sendOption.ToString()}");
        }
        public CustomRpcSender(PlayerControl source, CustomRPC rpc, PlayerControl target, SendOption sendOption = SendOption.Reliable)
        {
            writer = AmongUsClient.Instance.StartRpcImmediately(source.NetId, (byte)rpc, sendOption, (int)target.NetId);
            Main.Log.LogInfo($"Rpc Started: {source.NetId}, {target.NetId}, {rpc.ToString()}, {sendOption.ToString()}");
        }
        public CustomRpcSender(PlayerControl source, RpcCalls rpc, PlayerControl target, SendOption sendOption = SendOption.Reliable)
        {
            writer = AmongUsClient.Instance.StartRpcImmediately(source.NetId, (byte)rpc, sendOption, (int)target.NetId);
            Main.Log.LogInfo($"Rpc Started: {source.NetId}, {target.NetId}, {rpc.ToString()}, {sendOption.ToString()}");
        }
        public CustomRpcSender(PlayerControl source, RpcCalls rpc, int target = -1, SendOption sendOption = SendOption.Reliable)
        {
            writer = AmongUsClient.Instance.StartRpcImmediately(source.NetId, (byte)rpc, sendOption, target);
            Main.Log.LogInfo($"Rpc Started: {source.NetId}, {target}, {rpc.ToString()}, {sendOption.ToString()}");
        }
        
        public CustomRpcSender Write(float val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(string val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(ulong val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(int val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(uint val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(ushort val)
        {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(byte val) {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(sbyte val) {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(bool val) {
            writer.Write(val);
            return this;
        }
        public CustomRpcSender Write(Il2CppStructArray<byte> bytes)
        {
            writer.Write(bytes);
            return this;
        }
        public CustomRpcSender Write(Il2CppStructArray<byte> bytes, int offset, int length) {
            writer.Write(bytes, offset, length);
            return this;
        }
        public CustomRpcSender WriteBytesAndSize(Il2CppStructArray<byte> bytes)
        {
            writer.Write(bytes);
            return this;
        }
        public CustomRpcSender WritePacked(int val)
        {
            writer.WritePacked(val);
            return this;
        }
        public CustomRpcSender WritePacked(uint val)
        {
            writer.WritePacked(val);
            return this;
        }
        public CustomRpcSender WriteNetObject(InnerNetObject obj)
        {
            writer.WriteNetObject(obj);
            return this;
        }
        public void EndRpc()
        {
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            this.WasEnded = true;
        }
    }
}