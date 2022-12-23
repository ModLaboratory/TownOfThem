using System;
using HarmonyLib;
using UnityEngine;


namespace MainMenuPatches
{
    [HarmonyPatch]
    public class MainMenuManagerPatch
    {
        public static GameObject template;
        public static GameObject githubButton;
        public static GameObject updateButton;

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            if (template == null) template = GameObject.Find("/MainUI/ExitGameButton");
            if (template == null) return;
            if (githubButton == null) githubButton = UnityEngine.Object.Instantiate(template, template.transform.parent);
            githubButton.name = "GithubButton";
            githubButton.transform.position = Vector3.Reflect(template.transform.position, Vector3.left);

            var githubText = githubButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            Color githubColor = new Color32(225, 165, 0, byte.MaxValue);
            PassiveButton githubPassiveButton = githubButton.GetComponent<PassiveButton>();
            SpriteRenderer githubButtonSprite = githubButton.GetComponent<SpriteRenderer>();
            githubPassiveButton.OnClick = new();
            githubPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL("https://github.com/JieGeLovesDengDuaLang/TownOfThem")));
            githubPassiveButton.OnMouseOut.AddListener((Action)(() => githubButtonSprite.color = githubText.color = githubColor));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => githubText.SetText("Github"))));
            githubButtonSprite.color = githubText.color = githubColor;
            githubButton.gameObject.SetActive(Main.ShowgithubButton);

        }
    }
}