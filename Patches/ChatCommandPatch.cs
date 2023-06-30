using HarmonyLib;
using System;
using TownOfThem.Patches;
using static PlayerControl;
using static TownOfThem.Modules.Translation;

namespace TownOfThem.Patch
{
    [HarmonyPatch(typeof(ChatController),nameof(ChatController.SendChat))]
    class ChatCommandPatch
    {
        public static void Prefix(ChatController __instance)
        {
            string text = __instance.TextArea.text;
            string[] command = text.Split(" ");
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
                /*case "/language":
                    TownOfThem.Modules.Translation.LoadLanguage((SupportedLangs)Convert.ToInt32(command[1]));
                    if (TownOfThem.Modules.Translation.Translations["ModLanguage"] == "0" && command[1] != "0")
                    {
                        LocalPlayer.RpcSendChat($"Your Language:{(SupportedLangs)Convert.ToInt32(command[1])} Load Error!\nLanguage Not Support.\nBack To Old Language:{(SupportedLangs)Main.LanguageID.Value}");
                        TownOfThem.Modules.Translation.LoadLanguage((SupportedLangs)Main.LanguageID.Value);
                    }
                    else
                    {
                        Main.LanguageID.Value = Convert.ToInt32(command[1]);
                        LocalPlayer.RpcSendChat($"OK!\nNow Mod Language:{(SupportedLangs)Convert.ToInt32(command[1])}\nConfig file(Mod Default Language)Edited!");
                    }
                    break;*/
                case "/startminplayer":
                    GameStartManager.Instance.MinPlayers = Convert.ToInt32(command[1]);
                    break;
                case "/pet":
                    LocalPlayer.RpcSetPet(command[1]);
                    break;
                case "/popup":
                    MainMenuManagerStartPatch.ShowPopup(command[1]);
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
