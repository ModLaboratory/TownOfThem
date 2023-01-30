using HarmonyLib;
using Hazel;
using System;
using System.Linq;
using TownOfThem.CustomGameOptions;

namespace TownOfThem.CustomRPCs
{
    public enum CustomRPC:uint
    {
        ShareOptions=100,
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
    }

    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatches
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            byte packetId = callId;
            switch (packetId)
            {
                case (byte)CustomRPC.ShareOptions:
                    RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                    break;
            }
        }
    }
}