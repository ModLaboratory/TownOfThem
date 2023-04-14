using System;
using HarmonyLib;
using UnityEngine;
using static TownOfThem.Main;

namespace TownOfThem.Patch
{
    [HarmonyPatch]
    class DeleteAllMainMenuButton
    {
        [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start)),HarmonyPrefix]
        public static void Prefix(MainMenuManager __instance)
        {
            if (ModDamaged) 
            {
                var localButton = GameObject.Find("PlayLocalButton");
                GameObject.Destroy(localButton);
                var onlineButton = GameObject.Find("PlayOnlineButton");
                GameObject.Destroy(onlineButton);
                var freeplayButton = GameObject.Find("FreePlayButton");
                freeplayButton.GetComponent<PassiveButton>().OnClick = new();
                freeplayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL(GithubLink)));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => freeplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("Mod Damaged!"))));
            }
        
        }
    }
}
