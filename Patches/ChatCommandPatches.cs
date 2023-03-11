using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static PlayerControl;
using static TownOfThem.Language.Translation;

namespace TownOfThem.ChatCommandPatches
{
    [HarmonyPatch(typeof(ChatController),nameof(ChatController.SendChat))]
    class ChatCommandPatches
    {
        public static void Prefix(ChatController __instance)
        {
            string text = __instance.TextArea.text;
            string[] command = text.Split(" ");
            switch (command[0].ToLower())
            {
                case "/help":
                    LocalPlayer.RpcSendChat(LoadTranslation("cmdHelp"));
                    break;
                case "/name":
                    LocalPlayer.RpcSetName(command[1]);
                    break;
                case "/startmeeting":
                    GameData.PlayerInfo test1 = new GameData.PlayerInfo(LocalPlayer);
                    LocalPlayer.RpcStartMeeting(test1);
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
                case "/language":
                    TownOfThem.Language.Translation.LoadLanguage((SupportedLangs)Convert.ToInt32(command[1]));
                    if (TownOfThem.Language.Translation.Translations["ModLanguage"] == "0" && command[1] != "0")
                    {
                        LocalPlayer.RpcSendChat($"Your Language:{(SupportedLangs)Convert.ToInt32(command[1])} Load Error!\nLanguage Not Support.\nBack To Old Language:{(SupportedLangs)Main.LanguageID.Value}");
                        TownOfThem.Language.Translation.LoadLanguage((SupportedLangs)Main.LanguageID.Value);
                    }
                    else
                    {
                        Main.LanguageID.Value = Convert.ToInt32(command[1]);
                        LocalPlayer.RpcSendChat($"OK!\nNow Mod Language:{(SupportedLangs)Convert.ToInt32(command[1])}\nConfig file(Mod Default Language)Edited!");
                    }
                    break;
                case "/hostname":
                    string playerName = LocalPlayer.name;
                    if (AmongUsClient.Instance.AmHost)
                    {
                        switch (command[1])
                        {
                            case "0":
                                break;
                            case "1":
                                LocalPlayer.RpcSetName($"{playerName}\n{TownOfThem.Main.ModName} v{TownOfThem.Main.ModVer}\n{LoadTranslation("CantPlayWithoutMod")}");
                                break;
                            case "2":
                                LocalPlayer.RpcSetName($"{playerName}\n{TownOfThem.Main.HostCustomName.Value}");
                                break;
                        }
                    }
                    break;
                case "/startminplayer":
                    GameStartManager.Instance.MinPlayers = Convert.ToInt32(command[1]);
                    break;
                case "/pet":
                    LocalPlayer.RpcSetPet(command[1]);
                    break;
            }
            
        }
    }
}
