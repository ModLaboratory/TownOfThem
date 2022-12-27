using System;
using HarmonyLib;
using UnityEngine;

namespace MainMenuButtonPatches
{
    //code from @tukasa001's TownOfHost
    [HarmonyPatch]
    public class MainMenuManagerPatch
    {
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            var HowToPlayButton = GameObject.Find("/MainUI/HowToPlayButton");
            if (HowToPlayButton != null)
            {
                HowToPlayButton.GetComponent<PassiveButton>().OnClick = new();
                HowToPlayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL("https://github.com/JieGeLovesDengDuaLang/TownOfThem")));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => HowToPlayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("GitHub"))));//idk why it cannot pass the complie:(
            }

        }
    }
}
