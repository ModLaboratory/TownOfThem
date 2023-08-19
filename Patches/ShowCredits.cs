using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextTheirTown;
using NextTheirTown.Modules;
using TMPro;
using UnityEngine;

namespace NextTheirTown.Patches
{
    [Patch]
    class ShowCredits
    {
        [Patch]
        class InGame
        {
            [Patch(typeof(PingTracker), nameof(PingTracker.Update))]
            [Postfix]
            static void PingTrackerPatch(PingTracker __instance)
            {
                __instance.text.text += "\n" + Main.ModName;
            }
        }

        [Patch]
        class MainMenu
        {
            public static GameObject ModLogo;
            public static List<PassiveButton> Buttons = new();

            [Patch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
            [Postfix]
            static void ShowModLogo(MainMenuManager __instance)
            {
                ModLogo = new GameObject("totLogo");
                ModLogo.transform.position = new Vector3(1.8f, 0.2f, 0f);
                var bgRenderer = ModLogo.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = Utils.LoadSprite("COG.Resources.InDLL.Images.COG-BG.png", 295f);
            }

            [Patch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
            [Postfix]
            static void ShowButtons(MainMenuManager __instance)
            {
                Buttons.Clear();
                var template = __instance.creditsButton;

                if (!template) return;

                CreateButton(__instance, template, GameObject.Find("RightPanel").transform, new Vector2(0.2f, 0.38f),
                    "GitHub",
                    () => Application.OpenURL(Main.GithubLink));

                CreateButton(__instance, template, GameObject.Find("RightPanel").transform, new Vector2(0.7f, 0.38f),
                    "Bilibili",
                    () => Application.OpenURL(Main.BilibiliLink));
            }

            private static void CreateButton(MainMenuManager __instance, PassiveButton template, Transform? parent,
            Vector2 anchorPoint, string text, Action action, Color? color = null)
            {
                if (!parent) return;

                var button = Object.Instantiate(template, parent);
                button.GetComponent<AspectPosition>().anchorPoint = anchorPoint;
                var buttonSprite = button.transform.FindChild("Inactive").GetComponent<SpriteRenderer>();
                if (color.HasValue) buttonSprite.color = color.Value;
                __instance.StartCoroutine(Effects.Lerp(0.5f,
                    new Action<float>(p => { button.GetComponentInChildren<TMP_Text>().SetText(text); })));

                button.OnClick = new();
                button.OnClick.AddListener(action);

                Buttons.Add(button);
            }

            [Patch(nameof(MainMenuManager.OpenAccountMenu))]
            [Patch(nameof(MainMenuManager.OpenCredits))]
            [Patch(nameof(MainMenuManager.OpenGameModeMenu))]
            [Postfix]
            private static void Hide() => ShowModStuffs(false);

            [Patch(nameof(MainMenuManager.ResetScreen))]
            [Postfix]
            private static void Show() => ShowModStuffs(true);

            public static void ShowModStuffs(bool active)
            {
                if (ModLogo != null) ModLogo.SetActive(active);
                Buttons.Where(b => b && b.gameObject).ToList().ForEach(p => p.gameObject.SetActive(active));
            }
        }
    }
}
