using HarmonyLib;
using System;
using TownOfThem.Patches;
using static PlayerControl;
using static TownOfThem.Modules.Translation;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(ChatController),nameof(ChatController.AddChat))]
    class ChatCommandPatch
    {
        public static void Prefix(ChatController __instance, PlayerControl sourcePlayer, string chatText)
        {
            string[] command = chatText.Split(" ");
            switch (command[0].ToLower())
            {
                case "/help":
                    __instance.AddChat(LocalPlayer, GetString("cmdHelp"));
                    break;
                case "/name":
                    LocalPlayer.RpcSetName(command[1]);
                    break;
                case "/startrpc":
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, Convert.ToByte(command[1]), SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    break;
                case "/shapeshift":
                    LocalPlayer.RpcShapeshift(LocalPlayer,Convert.ToBoolean(command[1]));
                    break;
                case "/color":
                    LocalPlayer.RpcSetColor(Convert.ToByte(command[1]));
                    break;
                case "/report":
                    GameData.PlayerInfo test2 = new GameData.PlayerInfo(LocalPlayer);
                    LocalPlayer.ReportDeadBody(test2);
                    break;
                case "/role":
                    LocalPlayer.RpcSetRole((AmongUs.GameOptions.RoleTypes)Convert.ToInt32(command[1]));
                    break;
                case "/scan":
                    LocalPlayer.RpcSetScanner(Convert.ToBoolean(command[1]));
                    break;
                case "/finishtask":
                    LocalPlayer.RpcCompleteTask(Convert.ToUInt32(command[1]));
                    break;
                case "/startminplayer":
                    GameStartManager.Instance.MinPlayers = Convert.ToInt32(command[1]);
                    break;
                case "/pet":
                    LocalPlayer.RpcSetPet(command[1]);
                    break;
                case "/popup":
                    HudManager.Instance.ShowPopUp(command[1]);
                    break;
                case "/pid":
                    string a = "";
                    foreach(var pc in PlayerControl.AllPlayerControls)
                    {
                        a += pc.Data.PlayerName + " " + pc.PlayerId + "\n";
                    }
                    __instance.AddChat(LocalPlayer, a);
                    break;
                case "sn":
                    PlayerControl playerToSetName1 = null;
                    foreach(var pc in PlayerControl.AllPlayerControls)
                    {
                        if (pc.PlayerId == Convert.ToByte(command[1]))
                        {
                            playerToSetName1 = pc;
                        }
                    }
                    playerToSetName1.RpcSetName(command[2]);
                    break;
                case "sm":
                    PlayerControl playerToSetName2 = null;
                    PlayerControl targ = null;
                    foreach (var pc in PlayerControl.AllPlayerControls)
                    {
                        if (pc.PlayerId == Convert.ToByte(command[1]))
                        {
                            playerToSetName2 = pc;
                        }
                    }
                    foreach (var pc in PlayerControl.AllPlayerControls)
                    {
                        if (pc.PlayerId == Convert.ToByte(command[2]))
                        {
                            targ = pc;
                        }
                    }
                    playerToSetName2.RpcMurderPlayer(targ);
                    break;
                case "/sc":
                    PlayerControl player1 = null;
                    foreach (var pc in PlayerControl.AllPlayerControls)
                    {
                        if (pc.PlayerId == Convert.ToByte(command[1]))
                        {
                            player1 = pc;
                        }
                    }
                    player1.RpcSendChatNote(player1.PlayerId, ChatNoteTypes.DidVote);
                    break;


            }
            
        }
    }
}
