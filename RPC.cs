using HarmonyLib;
using Hazel;
using System;
using System.Linq;
using TownOfThem.CustomObjects;
using TownOfThem.Patch;

namespace TownOfThem.CustomRPCs;

public enum CustomRPC : uint
{
    ShareOptions = 100,
    ShareModVersion,
    UncheckedMurderPlayer,
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
        PlayerControl source = ModHelpers.ModHelpers.playerById(sourceId);
        PlayerControl target = ModHelpers.ModHelpers.playerById(targetId);
        if (source != null && target != null)
        {
            if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
            source.MurderPlayer(target);
        }
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
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
            default:
                if (!Enum.IsDefined(typeof(RpcCalls), packetId))
                {
                    Main.Log.LogError($"Unknown RPC: {packetId.ToString()}");
                }
                break;
        }
    }
}