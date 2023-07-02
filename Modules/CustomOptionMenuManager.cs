using TMPro;
using UnityEngine.Events;
using static UnityEngine.UI.Button;
using UnityEngine;
using System.Collections.Generic;

namespace TownOfThem.Modules
{
    public static class CustomOptionMenuManager
    {
        private static GameObject popUp;
        private static TextMeshPro titleText;

        private static ToggleButtonBehaviour prefab;
        private static Vector3? _origin;


        public static void Init(OptionsMenuBehaviour __instance)
        {
            prefab = Object.Instantiate(__instance.CensorChatButton);
            Object.DontDestroyOnLoad(prefab);
            prefab.name = "CensorChatPrefab";
            prefab.gameObject.SetActive(false);
        }
        public static void CreateButton(OptionsMenuBehaviour __instance)
        {
            var popUp = Object.Instantiate(__instance.gameObject);
            Object.DontDestroyOnLoad(popUp);
            popUp.transform.SetLocalZ(-810f);
            Object.Destroy(popUp.GetComponent<OptionsMenuBehaviour>());
            foreach (var go in popUp.gameObject.GetAllChilds())
            {
                if (go.name != "Background" && go.name != "CloseButton") Object.Destroy(go);
            }
            popUp.SetActive(false);
        }
        public static void LoadButton(OptionsMenuBehaviour __instance)
        {
            var modOptions = Object.Instantiate(prefab, __instance.CensorChatButton.transform.parent);
            var transform = __instance.CensorChatButton.transform;
            __instance.CensorChatButton.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            _origin ??= transform.localPosition;

            transform.localPosition = _origin.Value + Vector3.left * 0.45f;
            transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localPosition += Vector3.right * 0.5f;
            __instance.EnableFriendInvitesButton.Text.transform.localScale = new Vector3(1.2f, 1, 1);

            modOptions.transform.localPosition = _origin.Value + Vector3.right * 4f / 3f;
            modOptions.transform.localScale = new Vector3(0.66f, 1, 1);

            modOptions.gameObject.SetActive(true);
            modOptions.Text.text = "TOT Options";
            modOptions.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
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
                //TODO
            }));
        }


        private static IEnumerable<GameObject> GetAllChilds(this GameObject Go)
        {
            for (var i = 0; i < Go.transform.childCount; i++)
            {
                yield return Go.transform.GetChild(i).gameObject;
            }
        }
    }
}
