using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Il2CppSystem.Net.Http.Headers.Parser;

namespace TownOfThem.Patches
{
    enum ButtonActionType
    {
        Close,
        Quit,
        Custom
    }
    [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start))]
    class MainMenuManagerStartPatch
    {
        public static GenericPopup InfoPopup;
        public static void Prefix()
        {
            InfoPopup = UnityEngine.Object.Instantiate(Twitch.TwitchManager.Instance.TwitchPopup);
            InfoPopup.name = "InfoPopup";
            InfoPopup.TextAreaTMP.GetComponent<RectTransform>().sizeDelta = new(2.5f, 2f);
        }
        public static void Postfix(MainMenuManager __instance)
        {
            var button = __instance.GetComponentInChildren<DoNotPressButton>(true);
            SpriteRenderer pedestal = button.GetComponent<SpriteRenderer>();
            SpriteRenderer pressed = button.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer unpressed = button.transform.GetChild(1).GetComponent<SpriteRenderer>();
            pedestal.gameObject.SetActive(true);
            pressed.gameObject.SetActive(true);
            unpressed.gameObject.SetActive(true);
            unpressed.enabled = true;
            pressed.enabled = false;
            PassiveButton creditsButton = button.GetComponent<PassiveButton>();

            creditsButton.OnMouseOver.AddListener((UnityAction)(() =>
            {
                pressed.enabled = true;
                unpressed.enabled = false;
            }));
            creditsButton.OnMouseOut.AddListener((UnityAction)(() =>
            {
                pressed.enabled = false;
                unpressed.enabled = true;
            }));
            

            if (Main.IsBeta)
            {
                if (System.DateTime.UtcNow >= Main.ExpireTime.ToUniversalTime())
                {
                    ShowPopup(GetString(StringKey.ModExpired), bat: ButtonActionType.Quit);
                    return;
                }
            }
            if (Main.EnableDevMode.Value)
            {
                ShowPopup(GetString(StringKey.DevModeWarning));
            }

        }
        public static void ShowPopup(string message, bool showButton = true, ButtonActionType bat = ButtonActionType.Close, System.Action action = null)
        {
            System.Action a = null;
            if (InfoPopup != null)
            {
                InfoPopup.Show(message);
                var button = InfoPopup.transform.FindChild("ExitGame");
                if (button != null)
                {
                    button.gameObject.SetActive(showButton);
                    button.GetChild(0).GetComponent<TextTranslatorTMP>().TargetText = StringNames.QuitLabel;
                    button.GetComponent<PassiveButton>().OnClick = new();
                    switch (bat)
                    {
                        case ButtonActionType.Close:
                            a = () => InfoPopup.Close();
                            break;
                        case ButtonActionType.Quit:
                            a = () => Application.Quit();
                            break;
                        case ButtonActionType.Custom:
                            a = action;
                            break;
                    }
                    button.GetComponent<PassiveButton>().OnClick.AddListener(a);
                }
            }
        }
    }
    
}
