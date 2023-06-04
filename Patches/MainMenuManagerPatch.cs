using HarmonyLib;
using UnityEngine;
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
            var localButton = GameObject.Find("PlayLocalButton");
            var onlineButton = GameObject.Find("PlayOnlineButton");
            var freeplayButton = GameObject.Find("FreePlayButton");
            var howtoplayButton = GameObject.Find("HowToPlayButton");
            localButton.transform.position = new Vector3(2.85f, 1.15f, 0);
            onlineButton.transform.position = new Vector3(2.85f, 0.3f, 0);
            freeplayButton.transform.position = new Vector3(2.85f, -0.95f, 0);
            howtoplayButton.transform.position = new Vector3(2.85f, -0.4f, 0);

            if (Main.IsBeta)
            {
                if (System.DateTime.UtcNow >= Main.ExpireTime.ToUniversalTime())
                {
                    ShowPopup(GetString(Language.StringKey.ModExpired), bat: ButtonActionType.Quit);
                }
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
