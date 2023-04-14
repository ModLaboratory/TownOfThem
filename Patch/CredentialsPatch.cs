using HarmonyLib;
using System;
using UnityEngine;
using static TownOfThem.Language.Translation;

//used TownOfHost's code
namespace TownOfThem.Patch
{
    [HarmonyPatch]
    public class MainMenuButtonPatch
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
            Color githubColor = new Color32(255, 255, 255, byte.MaxValue);
            PassiveButton githubPassiveButton = githubButton.GetComponent<PassiveButton>();
            SpriteRenderer githubButtonSprite = githubButton.GetComponent<SpriteRenderer>();
            githubPassiveButton.OnClick = new();
            githubPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL(TownOfThem.Main.GithubLink)));
            githubPassiveButton.OnMouseOut.AddListener((Action)(() => githubButtonSprite.color = githubText.color = githubColor));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => githubText.SetText("Github"))));
            githubButtonSprite.color = githubText.color = githubColor;
            githubButton.gameObject.SetActive(true);

            var howToPlayButton = GameObject.Find("HowToPlayButton");
            if (howToPlayButton != null)
            {
                var bilibiliText = howToPlayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                Color bilibiliColor = new Color32(0, 174, 236, byte.MaxValue);
                howToPlayButton.GetComponent<PassiveButton>().OnClick = new();
                howToPlayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL(TownOfThem.Main.BilibiliLink)));
                howToPlayButton.GetComponent<PassiveButton>().OnMouseOut.AddListener((Action)(() => howToPlayButton.GetComponent<SpriteRenderer>().color = bilibiliText.color = bilibiliColor));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => howToPlayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(LoadTranslation("Bilibili")))));
                howToPlayButton.GetComponent<SpriteRenderer>().color = bilibiliText.color = bilibiliColor;
            }
        }
        
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    class ModLogoPatch
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
            if((DateTime.Now.Month == 12) && (DateTime.Now.Day == 21))
            {
                credentials.text = LoadTranslation("totBirthday")+$"{TownOfThem.Main.ModName}!\nv{TownOfThem.Main.ModVer}\n{TownOfThem.Language.Translation.Translations["ModInfo1"]}";
            }
            else
            {
                credentials.text = $"{TownOfThem.Main.ModName}\nv{TownOfThem.Main.ModVer}\n" + LoadTranslation("ModInfo1");
            }
            credentials.alignment = TMPro.TextAlignmentOptions.TopRight;
            credentials.transform.position = new Vector3(4.6f, 3.2f, 0);
        }
     }

    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    class ModStampPatch
    {
        public static void Prefix(ModManager __instance)
        {
            __instance.ShowModStamp();
        }
    }

    [HarmonyPatch(typeof(PingTracker),nameof(PingTracker.Update))]
    class PingTrackerPatch
    {
        public static void Postfix(PingTracker __instance)
        {
            __instance.text.text = $"{__instance.text.text}\n{TownOfThem.Main.ModName}\nv{TownOfThem.Main.ModVer}\n" + LoadTranslation("ModInfo1");
            __instance.transform.localPosition = new Vector3(1.25f, 3f, __instance.transform.localPosition.z);
        }
    }
}
    