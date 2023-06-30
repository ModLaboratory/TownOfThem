
using System.Drawing;
using System.Text;
using TMPro;
using UnityEngine;
using static TownOfThem.Main;

namespace TownOfThem.Patch
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuButtonPatch
    {
        public static PassiveButton template;
        public static PassiveButton github;
        public static PassiveButton bilibili;
        public static void Postfix(MainMenuManager __instance)
        {
            template = __instance.quitButton;
            if (template == null) return;
            if (github == null)
            {
                github = CreateButton(
                    "GitHubButton",
                    new(1f, -1f, 1f),
                    new(153, 153, 153, byte.MaxValue),
                    new(209, 209, 209, byte.MaxValue),
                    () => Application.OpenURL(GithubLink),
                    "GitHub");
            }
            if (bilibili == null)
            {
                bilibili = CreateButton(
                    "BilibiliButton",
                    new(-1f, -1f, 1f),
                    new(0, 174, 236, byte.MaxValue),
                    new(0, 134, 236, byte.MaxValue),
                    () => Application.OpenURL(Main.BilibiliLink),
                    GetString(StringKey.Bilibili));
            }

        }
        //https://github.com/tukasa0001/TownOfHost/blob/main/Patches/MainMenuManagerPatch.cs
        private static PassiveButton CreateButton(string name, Vector3 localPosition, Color32 normalColor, Color32 hoverColor, System.Action action, string label, Vector2? scale = null)
        {
            var button = Object.Instantiate(template, ModLogoPatch.amongUsLogo.transform);
            button.name = name;
            Object.Destroy(button.GetComponent<AspectPosition>());
            button.transform.localPosition = localPosition;

            button.OnClick = new();
            button.OnClick.AddListener(action);

            var buttonText = button.transform.Find("FontPlacer/Text_TMP").GetComponent<TMP_Text>();
            var translator = buttonText.GetComponent<TextTranslatorTMP>();
            if (translator != null)
            {
                Object.Destroy(translator);
            }
            buttonText.fontSize = buttonText.fontSizeMax = buttonText.fontSizeMin = 3.5f;
            buttonText.enableWordWrapping = false;
            buttonText.text = label;
            var normalSprite = button.inactiveSprites.GetComponent<SpriteRenderer>();
            var hoverSprite = button.activeSprites.GetComponent<SpriteRenderer>();
            normalSprite.color = normalColor;
            hoverSprite.color = hoverColor;
            var container = buttonText.transform.parent;
            Object.Destroy(container.GetComponent<AspectPosition>());
            Object.Destroy(buttonText.GetComponent<AspectPosition>());
            container.SetLocalX(0f);
            buttonText.transform.SetLocalX(0f);
            buttonText.horizontalAlignment = HorizontalAlignmentOptions.Center;

            var buttonCollider = button.GetComponent<BoxCollider2D>();
            if (scale.HasValue)
            {
                normalSprite.size = hoverSprite.size = buttonCollider.size = scale.Value;
            }
            buttonCollider.offset = new(0f, 0f);

            return button;
        }

    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    class ModLogoPatch
    {
        public static GameObject amongUsLogo;
        public static SpriteRenderer totLogo;
        static void Postfix(MainMenuManager __instance)
        {
            amongUsLogo = GameObject.Find("LOGO-AU");

            var rightpanel = __instance.gameModeButtons.transform.parent;
            var logoObject = new GameObject("titleLogo_TOH");
            var logoTransform = logoObject.transform;
            totLogo = logoObject.AddComponent<SpriteRenderer>();
            logoTransform.parent = rightpanel;
            logoTransform.localPosition = new(0f, 0.15f, 1f);
            logoTransform.localScale *= 1.2f;
            totLogo.sprite = ModHelpers.LoadSprite("TownOfThem.Resources.totLogo.png", 300f);
        }
    }
     [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
     class VersionShowerPatch
     {

        public static StringBuilder modInfo = new();
        static void getInfo()
        {
            //修改必追究
            modInfo.Clear();
            //birthday
            if ((System.DateTime.Now.Month == 12) && (System.DateTime.Now.Day == 21))
            {
                modInfo.Append(string.Format(GetString(StringKey.totBirthday), IsBeta ? ModName + "<color=#00b4eb> Beta</color>" : ModName));
            }
            else
            {
                modInfo.Append(IsBeta ? ModName + "<color=#00b4eb> Beta </color>" : ModName);
            }
            //version + credits
            modInfo.Append($"v{ModVer}\n<size=80%>" + GetString(StringKey.ModInfo1) + "</size>\n");

            modInfo.Append("<size=65%>").Append(GetString(StringKey.BuildTime)).Append(BuildTime.ToString("yyyy-MM-dd"));
            if (IsBeta) modInfo.Append("\n").Append(GetString(StringKey.ExpireTime)).Append(ExpireTime.ToString());
            modInfo.Append("</size>\n");
            if (EnableDevMode.Value) modInfo.Append("<color=#00b4eb>").Append(GetString(StringKey.DeveloperMode)).Append("</color>\n");
        }
        static void Postfix(VersionShower __instance)
        {
            var credentials = UnityEngine.Object.Instantiate(__instance.text);
            getInfo();
            credentials.text = modInfo.ToString();
            credentials.alignment = TMPro.TextAlignmentOptions.BottomRight;
            credentials.transform.position = new Vector3(5f, 0, 0);
        }
    }

    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    class ModStampPatch
    {
        public static SpriteRenderer ModStamp = null;
        static void Prefix(ModManager __instance)
        {
            __instance.ShowModStamp();
            ModStamp = __instance.ModStamp;
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
            ping.Append(string.Format(GetString(StringKey.Ping), AmongUsClient.Instance.Ping)).Append("</color>");
            __instance.text.text = VersionShowerPatch.modInfo.ToString() + ping.ToString();
            __instance.transform.localPosition = new Vector3(1f, 3f, __instance.transform.localPosition.z);
        }
    }
}
    