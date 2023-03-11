//using HarmonyLib;
//using Hazel;
//using TownOfThem.Utilities;
//using TownOfThem.CustomRPCs;
//using System.Collections.Generic;
//using Il2CppSystem.Web.Util;
//using Il2CppSystem.Linq.Expressions;
//using static TownOfThem.PlayersPatch.SendModVer;

//namespace TownOfThem.PlayersPatch
//{
//	[HarmonyPatch(typeof(AmongUsClient),nameof(AmongUsClient.OnGameJoined))]
//	class SendModVer
//	{
//		public static Dictionary<int, int> playerVersion;
//		public static bool versionSent = false;
//		public static void Postfix()
//		{
//			if (!versionSent)
//			{
//				TownOfThem.ModHelpers.ModHelpers.ShareVersion();
//			}
//		}
//	}
//    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
//	class CheckPlayer
//	{
//		public static string message = "";
//		public static void Postfix(GameStartManager __instance)
//		{
//            if (PlayerControl.LocalPlayer != null && !versionSent)
//            {
//                TownOfThem.ModHelpers.ModHelpers.ShareVersion();
//                versionSent = true;
//            }
//            foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients.ToArray())
//            {
//                if (client == null) continue;
//                if (client.Character == null) continue;
//                var dummyComponent = client.Character.GetComponent<DummyBehaviour>();
//                if (dummyComponent != null && dummyComponent.enabled) continue;
//                if (AmongUsClient.Instance.allClients.ToArray() == null) return;

//                if (playerVersion.ContainsKey(client.Id))
//                {
//                    int playerVer = playerVersion[client.Id];
//                    if (playerVer != Main.IntModVer)
//                    {
//                        message += $"{client.Character.Data.PlayerName} Has a different version mod!\n";
//                    }
//                }
//                else
//                {
//                    message += $"{client.Character.Data.PlayerName} No Mod!\n";
//                }

//            }
//            //Main.Log.LogInfo(message);
//        }
			
//    }
//}

