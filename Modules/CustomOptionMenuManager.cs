using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TownOfThem.Modules
{
    public static class CustomOptionsMenuManager
    {
        private static GameObject popUp;
        private static TextMeshPro titleText;
        private static List<ModOption> buttons = new(); 

        private static ToggleButtonBehaviour prefab;
        private static Vector3? _origin;

        private static bool inited = false;


        public static void Init(OptionsMenuBehaviour __instance)
        {
            if (inited) return;
            if (!__instance.CensorChatButton) return;

            if (!prefab)
            {
                prefab = Object.Instantiate(__instance.CensorChatButton);
                Object.DontDestroyOnLoad(prefab);
                prefab.name = "CensorChatPrefab";
                prefab.gameObject.SetActive(false);
            }
            CreateUI(__instance);
            LoadButton(__instance);

            inited = true;
        }
        private static void CreateUI(OptionsMenuBehaviour __instance)
        {
            var popUp = Object.Instantiate(__instance.gameObject);
            Object.DontDestroyOnLoad(popUp);
            popUp.transform.SetLocalZ(-810f);
            Object.Destroy(popUp.GetComponent<OptionsMenuBehaviour>());
            foreach (var go in popUp.gameObject.GetAllChilds())
            {
                if (go == null || popUp == null) continue;
                if (go.name != "Background" && go.name != "CloseButton") Object.Destroy(go);
            }
            popUp.SetActive(false);
        }
        private static void LoadButton(OptionsMenuBehaviour __instance)
        {
            //Vanilla buttons
            var modOptions = Object.Instantiate(prefab, __instance.CensorChatButton.transform.parent);
            var transform = __instance.CensorChatButton.transform;
            __instance.CensorChatButton.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            _origin ??= transform.localPosition;

            transform.localPosition = _origin.Value + Vector3.left * 0.45f;
            transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localPosition += Vector3.right * 0.5f;
            __instance.EnableFriendInvitesButton.Text.transform.localScale = new Vector3(1.2f, 1, 1);

            //Mod option button
            modOptions.transform.localPosition = _origin.Value + Vector3.right * 4f / 3f;
            modOptions.transform.localScale = new Vector3(0.66f, 1, 1);

            modOptions.gameObject.SetActive(true);
            modOptions.Text.text = GetString(StringKey.totOptions);
            modOptions.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            modOptions.Background.color = Main.ModColor;
            var modOptionsButton = modOptions.GetComponent<PassiveButton>();
            modOptionsButton.OnClick = new();

            modOptionsButton.OnClick.AddListener((System.Action)(() =>
            {
                if (!popUp) return;

                if (__instance.transform.parent && __instance.transform.parent == FastDestroyableSingleton<HudManager>.Instance.transform)
                {
                    popUp.transform.SetParent(FastDestroyableSingleton<HudManager>.Instance.transform);
                    popUp.transform.localPosition = new Vector3(0, 0, -800f);
                }
                else
                {
                    popUp.transform.SetParent(null);
                    Object.DontDestroyOnLoad(popUp);
                }
                Open();
                LoadButtons(__instance);
            }));
        }
        private static void LoadButtons(OptionsMenuBehaviour __instance)
        {

            int num = 0;
            foreach(var btn in buttons)
            {
                num++;
                CreateButton(btn, num);
            }
        }
        private static void CreateButton(ModOption button, int num)
        {
            if (popUp.transform.GetComponentInChildren<ToggleButtonBehaviour>()) return;

            var btn = Object.Instantiate(prefab, popUp.transform);
            var pos = new Vector3(num % 2 == 0 ? -1.17f : 1.17f, 1.3f - num / 2 * 0.8f, -.5f);

            button.ToggleButton = btn;
            btn.transform.localPosition = pos;
            btn.onState = button.DefaultValue;
            btn.Background.color = btn.onState ? Color.green : Palette.ClearWhite;
            btn.Text.text = GetString(button.Text);
            btn.Text.fontSizeMin = btn.Text.fontSizeMax = 1.8f;
            btn.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);
            btn.name = button.Text + "_Option";
            btn.gameObject.SetActive(true);

            var passive = btn.GetComponent<PassiveButton>();
            var collider = btn.GetComponent<BoxCollider2D>();

            collider.size = new(2.2f, .7f);
            passive.OnClick = new();
            passive.OnMouseOut = new();
            passive.OnMouseOver = new();

            passive.OnClick.AddListener((System.Action)(() =>
            {
                btn.onState = button.OnClick();
                btn.Background.color = btn.onState ? Color.green : Palette.ImpostorRed;
            }));
            passive.OnMouseOver.AddListener((System.Action)(() => btn.Background.color = new(34, 139, 34, byte.MaxValue)));
            passive.OnMouseOut.AddListener((System.Action)(() => btn.Background.color = btn.onState ? Color.green : Palette.ImpostorRed));
            foreach (var spr in btn.gameObject.GetComponentsInChildren<SpriteRenderer>()) spr.size = new Vector2(2.2f, .7f);
        }
        private static void Open()
        {
            popUp.SetActive(false);
            popUp.SetActive(true);
        }


        private static IEnumerable<GameObject> GetAllChilds(this GameObject Go)
        {
            for (var i = 0; i < Go.transform.childCount; i++)
            {
                yield return Go.transform.GetChild(i).gameObject;
            }
        }

        public class ModOption
        {
            public StringKey Text;
            public System.Func<bool> OnClick;
            public bool DefaultValue;
            public ToggleButtonBehaviour ToggleButton;
            public ModOption(StringKey text, System.Func<bool> onClick, bool defaultValue)
            {
                this.Text = text;
                this.OnClick = onClick;
                this.DefaultValue = defaultValue;
                buttons.Add(this);
            }
        }
    }
}
