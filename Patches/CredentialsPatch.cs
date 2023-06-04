using Epic.OnlineServices.Mods;
using HarmonyLib;
using System;
using System.Text;
using UnityEngine;
using static TownOfThem.Language.Translation;
using static TownOfThem.Main;

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
            githubPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL(GithubLink)));
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
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => howToPlayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(GetString("Bilibili")))));
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
            renderer.sprite = ModHelpers.LoadSprite("TownOfThem.Resources.totLogo.png", 300f);
            totlogo.transform.SetLocalX(-0.5f);
        }
    }
     [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
     class VersionShowerPatch
     {

        public static StringBuilder modInfo = new();
        static void getInfo()
        {
            modInfo.Clear();
            //birthday
            if ((DateTime.Now.Month == 12) && (DateTime.Now.Day == 21))
            {
                modInfo.Append(string.Format(GetString(Language.StringKey.totBirthday), IsBeta ? ModName + "<color=#00b4eb> Beta</color>" : ModName));
            }
            else
            {
                modInfo.Append(IsBeta ? ModName + "<color=#00b4eb> Beta </color>" : ModName);
            }

            //version + credits
            modInfo.Append($"v{ModVer}\n<size=100%>" + GetString(Language.StringKey.ModInfo1) + "</size>");
        }
        static void Postfix(VersionShower __instance)
        {
            var credentials = UnityEngine.Object.Instantiate(__instance.text);
            getInfo();
            credentials.text = modInfo.ToString();
            credentials.alignment = TMPro.TextAlignmentOptions.TopRight;
            credentials.transform.position = new Vector3(4f, 3f, 0);
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
            StringBuilder ping = new();
            ping.Append("\n<color=");
            if (AmongUsClient.Instance.Ping < 100)
            {
                ping.Append("#00ff00>");
            }
            else if(AmongUsClient.Instance.Ping < 300)
            {
                ping.Append("#ffff00>");
            }
            else if (AmongUsClient.Instance.Ping > 300)
            {
                ping.Append("#ff0000>");
            }
            ping.Append(string.Format(GetString(Language.StringKey.Ping), AmongUsClient.Instance.Ping)).Append("</color>");
            __instance.text.text = VersionShowerPatch.modInfo.ToString() + ping.ToString();
            __instance.transform.localPosition = new Vector3(1f, 3f, __instance.transform.localPosition.z);
        }
    }
}
    