using NextTheirTown.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace NextTheirTown.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour))]
    internal class ModOptionPatch
    {
        private static readonly List<Transform> Vanilla = new();

        [HarmonyPatch(nameof(OptionsMenuBehaviour.Start))]
        [HarmonyPostfix]
        public static void OptionMenuBehaviour_StartPostfix(OptionsMenuBehaviour __instance)
        {
            var transform1 = __instance.CensorChatButton.transform;
            Vector3? position = transform1.localPosition;
            var button = __instance.EnableFriendInvitesButton;

            var modOptions = Object.Instantiate(__instance.CensorChatButton, transform1.parent);

            //设置原版按钮的大小/位置
            __instance.CensorChatButton.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            transform1.localPosition = position.Value + Vector3.left * 0.45f;
            transform1.localScale = new Vector3(0.66f, 1, 1);

            var transform = button.transform;
            transform.localScale = new Vector3(0.66f, 1, 1);
            transform.localPosition += Vector3.right * 0.5f;
            button.Text.transform.localScale = new Vector3(1.2f, 1, 1);

            //设置模组选项按钮
            modOptions.gameObject.SetActive(true);
            modOptions.Text.text = "More Options...";
            var transform2 = modOptions.transform;
            transform2.localPosition = position.Value + Vector3.right * 4f / 3f;
            transform2.localScale = new Vector3(0.66f, 1, 1);
            modOptions.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            modOptions.Background.color = Palette.EnabledColor;

            var modOptionsButton = modOptions.GetComponent<PassiveButton>();
            modOptionsButton.OnClick = new();
            modOptionsButton.OnClick.AddListener((Action)(() =>
            {
                LoadButtons(__instance);
                HideVanillaButtons(__instance);
                foreach (var btn in ModOption.Buttons)
                    if (btn.ToggleButton != null)
                        btn.ToggleButton.gameObject.SetActive(true);
            }));
        }

        public static void HideVanillaButtons(OptionsMenuBehaviour menu)
        {
            Vanilla.Clear();
            for (var i = 0; i < menu.transform.childCount; i++)
            {
                var child = menu.transform.GetChild(i);
                if (child.name == "Background" ||
                    child.name == "CloseButton" ||
                    child.name == "Tint" ||
                    child.name == "TabButtons" ||
                    child.name == "GeneralButton" ||
                    child.name == "GraphicsButton" ||
                    !child.gameObject.active) continue;
                Vanilla.Add(child);
                child.gameObject.SetActive(false);
            }
        }

        public static void LoadButtons(OptionsMenuBehaviour menu)
        {
            var a = 0;
            foreach (var btn in ModOption.Buttons)
            {
                CreateButton(menu, a, btn);
                a++;
            }
        }


        public static void CreateButton(OptionsMenuBehaviour menu, int idx, ModOption option)
        {
            var template = menu.CensorChatButton;
            var button = Object.Instantiate(template, menu.transform);
            Vector3 pos = new(idx % 2 == 0 ? -1.17f : 1.17f, 1.7f - idx / 2 * 0.8f, -0.5f);

            button.transform.localPosition = pos;
            button.onState = option.DefaultValue;
            button.Background.color = button.onState ? Palette.AcceptedGreen : Palette.EnabledColor;
            button.Text.text = option.Text;
            button.name = option.Text.Replace(" ", "");
            button.gameObject.SetActive(true);

            var passive = button.GetComponent<PassiveButton>();
            passive.OnClick = new();
            passive.OnMouseOut = new UnityEvent();
            passive.OnMouseOver = new UnityEvent();

            passive.OnMouseOut.AddListener((Action)(() =>
                button.Background.color = button.onState ? Palette.AcceptedGreen : Palette.EnabledColor));
            passive.OnMouseOver.AddListener((Action)(() =>
            {
                if (!button.onState) button.Background.color = Palette.AcceptedGreen;
            }));
            passive.OnClick.AddListener((Action)(() =>
            {
                button.onState = option.OnClick();
                button.Background.color = button.onState ? Palette.AcceptedGreen : Palette.EnabledColor;
            }));

            option.ToggleButton = button;

            button.gameObject.SetActive(false);
        }

        [HarmonyPatch(nameof(OptionsMenuBehaviour.Close))]
        [HarmonyPostfix]
        public static void OptionMenuBehaviour_ClosePostfix() => HideAllButtons();

        public static void HideAllButtons() => ModOption.Buttons.Where(btn => btn.ToggleButton).Select(b => b.ToggleButton.gameObject).ToList().ForEach(b => b.SetActive(false));

    }

    [HarmonyPatch(typeof(TabGroup), nameof(TabGroup.Open))]
    internal class OnTabGroupOpen
    {
        private static void Postfix(TabGroup __instance) => ModOptionPatch.HideAllButtons();
    }
}
