using System;
using HarmonyLib;
using UnityEngine;
using TownOfThem.ModHelpers;

//used TownOfHost's code
namespace TownOfThem.MainMenuPatches
{
    [HarmonyPatch]
    public class MainMenuButtonPatches
    {
        public static GameObject template;
        public static GameObject githubButton;
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            if (template == null) template = GameObject.Find("/MainUI/ExitGameButton");
            if (template == null) return;
            if (githubButton == null) githubButton = UnityEngine.Object.Instantiate(template, template.transform.parent);
            githubButton.name = "githubButton";
            githubButton.transform.position = Vector3.Reflect(template.transform.position, Vector3.left);

            var githubText = githubButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            Color githubColor = new Color32(88, 101, 242, byte.MaxValue);
            PassiveButton githubPassiveButton = githubButton.GetComponent<PassiveButton>();
            SpriteRenderer githubButtonSprite = githubButton.GetComponent<SpriteRenderer>();
            githubPassiveButton.OnClick = new();
            githubPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL(TownOfThem.Main.GithubLink)));
            githubPassiveButton.OnMouseOut.AddListener((Action)(() => githubButtonSprite.color = githubText.color = githubColor));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => githubText.SetText("Github"))));
            githubButtonSprite.color = githubText.color = githubColor;
            githubButton.gameObject.SetActive(true);
        }
        
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    class ModLogoPatches
    {
        public static GameObject amongUsLogo;
        static void Postfix(MainMenuManager __instance)
        {
            if ((amongUsLogo = GameObject.Find("bannerLogo_AmongUs")) != null)
            {
                amongUsLogo.transform.localScale *= 0.4f;
                amongUsLogo.transform.position += Vector3.up * 0.25f;
            }

            var totlogo = new GameObject("totLogo");
            totlogo.transform.position = Vector3.up;
            totlogo.transform.localScale *= 1.2f;
            var renderer = totlogo.AddComponent<SpriteRenderer>();
            renderer.sprite = ModHelpers.ModHelpers.LoadSprite("TownOfThem.Resources.totLogo.png", 300f);
        }


     }
     
     [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
     class ModOtherInfosPatch
     {
        static void Postfix(VersionShower __instance)
        {
            var credentials = UnityEngine.Object.Instantiate(__instance.text);
            credentials.text = $"{TownOfThem.Main.ModName}\nv{TownOfThem.Main.ModVer}\vModded By JieGe";
            credentials.alignment = TMPro.TextAlignmentOptions.TopRight;
            credentials.transform.position = new Vector3(4.6f, 3.2f, 0);
        }
     }
    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    class ModStampPatches
    {
        public static void Prefix(ModManager __instance)
        {
            __instance.ShowModStamp();
        }
    }
}
    
