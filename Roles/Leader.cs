using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfThem.Roles
{
    [HarmonyPatch(typeof(HudManager),nameof(HudManager.Start))]
    class Leader
    {
        public static void Postfix(HudManager __instance)
        {
            /*Main.Log.LogInfo("Leader");
            string PlayerName = PlayerControl.LocalPlayer.name;
            PlayerControl.LocalPlayer.RpcSetRole(AmongUs.GameOptions.RoleTypes.Impostor);
            var deleteKill = GameObject.Find("KillButton");
            deleteKill.SetActive(false);
            var deleteVent = GameObject.Find("VentButton");
            deleteVent.SetActive(false);
            var sabotageButton = GameObject.Find("SabotageButton");
            sabotageButton.GetComponent<PassiveButton>().OnClick = new();
            sabotageButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => PlayerControl.LocalPlayer.RpcStartMeeting(new GameData.PlayerInfo((byte)PlayerControl.LocalPlayer.NetId))));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => sabotageButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("Meeting"))));
            PlayerControl.LocalPlayer.SetRole(AmongUs.GameOptions.RoleTypes.Crewmate);
            PlayerControl.LocalPlayer.SetName(PlayerName + "\n<color=#F8CD46>Sheriff</color>");*/
        }
    }
}
