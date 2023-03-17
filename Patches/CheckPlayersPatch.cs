//using HarmonyLib;
//using Hazel;
//using TownOfThem.Utilities;
//using TownOfThem.CustomRPCs;
//using System.Collections.Generic;
//using Il2CppSystem.Web.Util;
//using Il2CppSystem.Linq.Expressions;
//using static TownOfThem.PlayersPatch.SendModVer;
//using Reactor.Networking.Attributes;

//namespace TownOfThem.PlayersPatch
//{
//    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined))]
//    class SendModVer
//    {
//        public static Dictionary<PlayerControl, string> playerVersion;
//        public static bool versionSent = false;
//        public static void Postfix()
//        {
//            if (!versionSent)
//            {
//                ShareVersion(PlayerControl.LocalPlayer, Main.ModVer);
//            }
//        }
//        [MethodRpc((uint)CustomRPC.ShareModVersion)]
//        public static void ShareVersion(PlayerControl pc, string pv)
//        {
//            //MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)TownOfThem.CustomRPCs.CustomRPC.ShareModVersion, Hazel.SendOption.Reliable, -1);
//            //writer.Write((byte)AmongUsClient.Instance.ClientId);
//            //writer.Write((byte)Main.IntModVer);
//            //AmongUsClient.Instance.FinishRpcImmediately(writer);
//            //RPCProcedure.ShareModVersion(AmongUsClient.Instance.ClientId, Main.IntModVer);
//            playerVersion[pc] = pv;
//        }
//    }
//    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
//    class CheckPlayer
//    {
//        public static string message = "";
//        public static void Postfix(GameStartManager __instance)
//        {
//            if (PlayerControl.LocalPlayer != null && !versionSent)
//            {
//                ShareVersion(PlayerControl.LocalPlayer, Main.ModVer);
//                versionSent = true;
//            }
//            foreach (PlayerControl player in CachedPlayer.AllPlayers)
//            {
//                if (player == null) continue;
//                if (CachedPlayer.AllPlayers == null) return;

//                if (playerVersion.ContainsKey(player))
//                {
//                    string playerVer = playerVersion[player];
//                    if (playerVer != Main.ModVer)
//                    {
//                        message += $"{player.Data.PlayerName} Has a different version mod!\n";
//                    }
//                }
//                else
//                {
//                    message += $"{player.Data.PlayerName} No Mod!\n";
//                }

//            }
//            Main.Log.LogInfo(message);
//        }

//    }
//}

