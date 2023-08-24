using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using Hazel;
using System.Reflection;
using System.Text;
using static NextTheirTown.Modules.CustomOption;
using AmongUs.GameOptions;
using Il2CppSystem.Web.Util;

namespace NextTheirTown.Modules
{
    public class CustomOption
    {
        public enum CustomOptionType
        {
            General,
            Impostor,
            Neutral,
            Crewmate,
            Modifier,
        }

        public static int page = 0;

        public static List<CustomOption> Options = new List<CustomOption>();

        public int Id;
        public string Name;
        public System.Object[] Selections;

        public int DefaultSelection;
        public int Selection;
        public OptionBehaviour OptionBehaviour;
        public CustomOption Parent;
        public bool IsHeader;
        public CustomOptionType Type;

        // Option creation

        public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader)
        {
            this.Id = id;// 0 for preset
            this.Name = parent == null ? name : "- " + name;
            this.Selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.DefaultSelection = index >= 0 ? index : 0;
            this.Parent = parent;
            this.IsHeader = isHeader;
            this.Type = type;
            Selection = 0;
            
            Options.Add(this);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false)
        {
            return new CustomOption(id, type, name, selections, "", parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false)
        {
            List<object> selections = new();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, type, name, selections.ToArray(), defaultValue, parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false)
        {
            return new CustomOption(id, type, name, new string[] { "Off", "On" }, defaultValue ? "On" : "Off", parent, isHeader);
        }

        public static void ShareOptionChange(uint optionId)
        {
            var option = Options.FirstOrDefault(x => x.Id == optionId);
            if (option == null) return;
            //var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
            //writer.Write((byte)1);
            //writer.WritePacked((uint)option.id);
            //writer.WritePacked(Convert.ToUInt32(option.selection));
            //AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void ShareOptionSelections()
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance!.AmHost == false && PlayerControl.LocalPlayer == null) return;
            var optionsList = new List<CustomOption>(CustomOption.Options);
            while (optionsList.Any())
            {
                byte amount = (byte)Math.Min(optionsList.Count, 200); // takes less than 3 bytes per option on average
                //var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
                //writer.Write(amount);
                //for (int i = 0; i < amount; i++)
                //{
                //    var option = optionsList[0];
                //    optionsList.RemoveAt(0);
                //    writer.WritePacked((uint)option.id);
                //    writer.WritePacked(Convert.ToUInt32(option.selection));
                //}
                //AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        // Getter

        public int getSelection()
        {
            return Selection;
        }

        public bool getBool()
        {
            return Selection > 0;
        }

        public float getFloat()
        {
            return (float)Selections[Selection];
        }

        public int getQuantity()
        {
            return Selection + 1;
        }

        // Option changes

        public void updateSelection(int newSelection)
        {
            Selection = Mathf.Clamp((newSelection + Selections.Length) % Selections.Length, 0, Selections.Length - 1);
            if (OptionBehaviour != null && OptionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = Selection;
                stringOption.ValueText.text = Selections[Selection].ToString();

            }
            ShareOptionSelections();// Share all selections
            
        }


    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            createClassicTabs(__instance);

        }

        private static void createClassicTabs(GameOptionsMenu __instance)
        {
            bool isReturn = setNames(
                new Dictionary<string, string>()
                {
                    ["TOTSettings"] = "Next Their Town Settings",
                    ["ImpostorSettings"] = "Impostor Roles Settings",
                    ["NeutralSettings"] = "Neutral Roles Settings",
                    ["CrewmateSettings"] = "Crewmate Roles Settings",
                    ["ModifierSettings"] = "Modifier Settings"
                });

            if (isReturn) return;

            // Setup TOT tab
            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var torSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var torMenu = getMenu(torSettings, "TOTSettings");

            var impostorSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var impostorMenu = getMenu(impostorSettings, "ImpostorSettings");

            var neutralSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

            var crewmateSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

            var modifierSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");

            var torTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var torTabHighlight = getTabHighlight(torTab, "TownOfThemTab", "");

            var impostorTab = UnityEngine.Object.Instantiate(roleTab, torTab.transform);
            var impostorTabHighlight = getTabHighlight(impostorTab, "ImpostorTab", "");

            var neutralTab = UnityEngine.Object.Instantiate(roleTab, impostorTab.transform);
            var neutralTabHighlight = getTabHighlight(neutralTab, "NeutralTab", "");

            var crewmateTab = UnityEngine.Object.Instantiate(roleTab, neutralTab.transform);
            var crewmateTabHighlight = getTabHighlight(crewmateTab, "CrewmateTab", "");

            var modifierTab = UnityEngine.Object.Instantiate(roleTab, crewmateTab.transform);
            var modifierTabHighlight = getTabHighlight(modifierTab, "ModifierTab", "");

            // Position of Tab Icons
            gameTab.transform.position += Vector3.left * 3f;
            roleTab.transform.position += Vector3.left * 3f;
            torTab.transform.position += Vector3.left * 2f;
            impostorTab.transform.localPosition = Vector3.right * 1f;
            neutralTab.transform.localPosition = Vector3.right * 1f;
            crewmateTab.transform.localPosition = Vector3.right * 1f;
            modifierTab.transform.localPosition = Vector3.right * 1f;

            var tabs = new GameObject[] { gameTab, roleTab, torTab, impostorTab, neutralTab, crewmateTab, modifierTab };
            var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
            {
                [gameSettingMenu.RegularGameSettings] = gameSettingMenu.GameSettingsHightlight,
                [gameSettingMenu.RolesSettings.gameObject] = gameSettingMenu.RolesSettingsHightlight,
                [torSettings.gameObject] = torTabHighlight,
                [impostorSettings.gameObject] = impostorTabHighlight,
                [neutralSettings.gameObject] = neutralTabHighlight,
                [crewmateSettings.gameObject] = crewmateTabHighlight,
                [modifierSettings.gameObject] = modifierTabHighlight
            };
            for (int i = 0; i < tabs.Length; i++)
            {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button == null) continue;
                int copiedIndex = i;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((System.Action)(() => {
                    SetListener(settingsHighlightMap, copiedIndex);
                }));
            }

            DestroyOptions(new List<List<OptionBehaviour>>{
                torMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                impostorMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                neutralMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                crewmateMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                modifierMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
            });

            List<OptionBehaviour> torOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> impostorOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> neutralOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> crewmateOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> modifierOptions = new List<OptionBehaviour>();


            List<Transform> menus = new List<Transform>() { torMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform, modifierMenu.transform };
            List<List<OptionBehaviour>> optionBehaviours = new List<List<OptionBehaviour>>() { torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions };

            for (int i = 0; i < CustomOption.Options.Count; i++)
            {
                CustomOption option = CustomOption.Options[i];
                if ((int)option.Type > 4) continue;
                if (option.OptionBehaviour == null)
                {
                    StringOption stringOption = Object.Instantiate(template, menus[(int)option.Type]);
                    optionBehaviours[(int)option.Type].Add(stringOption);
                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                    stringOption.TitleText.text = option.Name;
                    stringOption.Value = stringOption.oldValue = option.Selection;
                    stringOption.ValueText.text = option.Selections[option.Selection].ToString();

                    option.OptionBehaviour = stringOption;
                }
                option.OptionBehaviour.gameObject.SetActive(true);
            }

            SetOptions(
                new List<GameOptionsMenu> { torMenu, impostorMenu, neutralMenu, crewmateMenu, modifierMenu },
                new List<List<OptionBehaviour>> { torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions },
                new List<GameObject> { torSettings, impostorSettings, neutralSettings, crewmateSettings, modifierSettings }
            );

            AdaptTaskCount(__instance);
        }

        


        private static void SetListener(Dictionary<GameObject, SpriteRenderer> settingsHighlightMap, int index)
        {
            foreach (KeyValuePair<GameObject, SpriteRenderer> entry in settingsHighlightMap)
            {
                entry.Key.SetActive(false);
                entry.Value.enabled = false;
            }
            settingsHighlightMap.ElementAt(index).Key.SetActive(true);
            settingsHighlightMap.ElementAt(index).Value.enabled = true;
        }

        private static void DestroyOptions(List<List<OptionBehaviour>> optionBehavioursList)
        {
            foreach (List<OptionBehaviour> optionBehaviours in optionBehavioursList)
            {
                foreach (OptionBehaviour option in optionBehaviours)
                    Object.Destroy(option.gameObject);
            }
        }

        private static bool setNames(Dictionary<string, string> gameObjectNameDisplayNameMap)
        {
            foreach (KeyValuePair<string, string> entry in gameObjectNameDisplayNameMap)
            {
                if (GameObject.Find(entry.Key) != null)
                { // Settings setup has already been performed, fixing the title of the tab and returning
                    GameObject.Find(entry.Key).transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(entry.Value);
                    return true;
                }
            }

            return false;
        }

        private static GameOptionsMenu getMenu(GameObject setting, string settingName)
        {
            var menu = setting.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            setting.name = settingName;

            return menu;
        }

        private static SpriteRenderer getTabHighlight(GameObject tab, string tabName, string tabSpritePath)
        {
            var tabHighlight = tab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            tab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Utils.LoadSprite(tabSpritePath, 100f);
            tab.name = "tabName";

            return tabHighlight;
        }

        private static void SetOptions(List<GameOptionsMenu> menus, List<List<OptionBehaviour>> options, List<GameObject> settings)
        {
            if (!(menus.Count == options.Count && options.Count == settings.Count))
            {
                Main.Log.LogError("List counts are not equal");
                return;
            }
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].Children = options[i].ToArray();
                settings[i].gameObject.SetActive(false);
            }
        }

        private static void AdaptTaskCount(GameOptionsMenu __instance)
        {
            // Adapt task count for main options
            var commonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = option.Name;
            __instance.Value = __instance.oldValue = option.Selection;
            __instance.ValueText.text = option.Selections[option.Selection].ToString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.Selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.Selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            ShareOptionSelections();
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    public class AmongUsClientOnPlayerJoinedPatch
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer != null && AmongUsClient.Instance.AmHost)
            {
                CustomOption.ShareOptionSelections();
            }
        }
    }


    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenuUpdatePatch
    {
        private static float timer = 1f;
        public static void Postfix(GameOptionsMenu __instance)
        {
            // Return Menu Update if in normal among us settings 
            var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            float offset = 2.75f;
            foreach (CustomOption option in CustomOption.Options)
            {
                if (GameObject.Find("TOTSettings") && option.Type != CustomOption.CustomOptionType.General)
                    continue;
                if (GameObject.Find("ImpostorSettings") && option.Type != CustomOption.CustomOptionType.Impostor)
                    continue;
                if (GameObject.Find("NeutralSettings") && option.Type != CustomOption.CustomOptionType.Neutral)
                    continue;
                if (GameObject.Find("CrewmateSettings") && option.Type != CustomOption.CustomOptionType.Crewmate)
                    continue;
                if (GameObject.Find("ModifierSettings") && option.Type != CustomOption.CustomOptionType.Modifier)
                    continue;
                if (option?.OptionBehaviour != null && option.OptionBehaviour.gameObject != null)
                {
                    bool enabled = true;
                    var parent = option.Parent;
                    while (parent != null && enabled)
                    {
                        enabled = parent.Selection != 0;
                        parent = parent.Parent;
                    }
                    option.OptionBehaviour.gameObject.SetActive(enabled);
                    if (enabled)
                    {
                        offset -= option.IsHeader ? 0.75f : 0.5f;
                        option.OptionBehaviour.transform.localPosition = new Vector3(option.OptionBehaviour.transform.localPosition.x, offset, option.OptionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }


    [HarmonyPatch]
    class HudStringPatch
    {

        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
        private static void Postfix(ref string __result)
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return; // Allow Vanilla Hide N Seek

            __result = GetOptByType((CustomOptionType)page);
        }

        public static string GetOptByType(CustomOptionType type)
        {
            var txt = "";
            List<CustomOption> opt = new();
            foreach (var option in Options)
                if (option != null && option.Type == type)
                    opt.Add(option);
            foreach (var option in opt)
                txt += option.Name + ": " + option.Selections[option.Selection] + Environment.NewLine;
            return txt;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                page = (page + 1) % 7;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                page = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                page = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                page = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                page = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                page = 4;
            }
            if (Input.GetKeyDown(KeyCode.F1))
                HudManagerUpdate.ToggleSettings(HudManager.Instance);
        }
    }


    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class GameSettingsScalePatch
    {
        public static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
        }
    }


    // This class is taken and adapted from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static float
            MinX,/*-5.3F*/
            OriginalY = 2.9F,
            MinY = 2.9F;


        public static Scroller Scroller;
        private static Vector3 LastPosition;
        private static float lastAspect;
        private static bool setLastPosition = false;

        public static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings?.transform == null) return;

            // Sets the MinX position to the left edge of the screen + 0.1 units
            Rect safeArea = Screen.safeArea;
            float aspect = Mathf.Min((Camera.main).aspect, safeArea.width / safeArea.height);
            float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
            MinX = 0.1f - safeOrthographicSize * aspect;

            if (!setLastPosition || aspect != lastAspect)
            {
                LastPosition = new Vector3(MinX, MinY);
                lastAspect = aspect;
                setLastPosition = true;
                if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            }

            CreateScroller(__instance);

            Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

            if (!Scroller.gameObject.active) return;

            var rows = __instance.GameSettings.text.Count(c => c == '\n');
            float LobbyTextRowHeight = 0.06F;
            var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

            Scroller.ContentYBounds = new FloatRange(MinY, maxY);

            // Prevent scrolling when the player is interacting with a menu
            if (PlayerControl.LocalPlayer.CanMove != true)
            {
                __instance.GameSettings.transform.localPosition = LastPosition;

                return;
            }

            if (__instance.GameSettings.transform.localPosition.x != MinX ||
                __instance.GameSettings.transform.localPosition.y < MinY) return;

            LastPosition = __instance.GameSettings.transform.localPosition;
        }

        private static void CreateScroller(HudManager __instance)
        {
            if (Scroller != null) return;

            Transform target = __instance.GameSettings.transform;

            Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
            Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
            Scroller.gameObject.layer = 5;

            Scroller.transform.localScale = Vector3.one;
            Scroller.allowX = false;
            Scroller.allowY = true;
            Scroller.active = true;
            Scroller.velocity = new Vector2(0, 0);
            Scroller.ScrollbarYBounds = new FloatRange(0, 0);
            Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            Scroller.enabled = true;

            Scroller.Inner = target;
            target.SetParent(Scroller.transform);
        }

        [HarmonyPrefix]
        public static void Prefix2(HudManager __instance)
        {
            if (!settingsTMPs[0]) return;
            foreach (var temp in settingsTMPs) temp.text = "";
            var tmp = "";
            Enum.GetValues<CustomOptionType>().ToList().ForEach(c => tmp += HudStringPatch.GetOptByType(c));
            var settingsString = tmp;
            var blocks = settingsString.Split("\n\n", StringSplitOptions.RemoveEmptyEntries); ;
            string curString = "";
            string curBlock;
            int j = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                curBlock = blocks[i];
                if (Utils.GetLineCount(curBlock) + Utils.GetLineCount(curString) < 43)
                {
                    curString += curBlock + "\n\n";
                }
                else
                {
                    settingsTMPs[j].text = curString;
                    j++;

                    curString = "\n" + curBlock + "\n\n";
                    if (curString.Substring(0, 2) != "\n\n") curString = "\n" + curString;
                }
            }
            if (j < settingsTMPs.Length) settingsTMPs[j].text = curString;
            int blockCount = 0;
            foreach (var temp in settingsTMPs)
            {
                if (temp.text != "")
                    blockCount++;
            }
            for (int i = 0; i < blockCount; i++)
            {
                Main.Log.LogMessage(blockCount);
                settingsTMPs[i].transform.localPosition = new Vector3(-blockCount * 1.2f + 2.7f * i, 2.2f, -500f);
            }
        }

        private static TMPro.TextMeshPro[] settingsTMPs = new TMPro.TextMeshPro[4];
        private static GameObject settingsBackground;
        public static void OpenSettings(HudManager __instance)
        {
            if (__instance.FullScreen == null || MapBehaviour.Instance && MapBehaviour.Instance.IsOpen
                /*|| AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started*/
                || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
            settingsBackground = GameObject.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
            settingsBackground.SetActive(true);
            var renderer = settingsBackground.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            renderer.enabled = true;

            for (int i = 0; i < settingsTMPs.Length; i++)
            {
                settingsTMPs[i] = GameObject.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
                settingsTMPs[i].alignment = TMPro.TextAlignmentOptions.TopLeft;
                settingsTMPs[i].enableWordWrapping = false;
                settingsTMPs[i].transform.localScale = Vector3.one * 0.25f;
                settingsTMPs[i].gameObject.SetActive(true);
            }
        }

        public static void CloseSettings()
        {
            foreach (var tmp in settingsTMPs)
                if (tmp) tmp.gameObject.Destroy();

            if (settingsBackground) settingsBackground.Destroy();
        }

        public static void ToggleSettings(HudManager __instance)
        {
            if (settingsTMPs[0]) CloseSettings();
            else OpenSettings(__instance);
        }

        static PassiveButton toggleSettingsButton;
        static GameObject toggleSettingsButtonObject;
        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            if (!toggleSettingsButton || !toggleSettingsButtonObject)
            {
                // add a special button for settings viewing:
                toggleSettingsButtonObject = GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
                toggleSettingsButtonObject.transform.localPosition = __instance.MapButton.transform.localPosition + new Vector3(0, -0.66f, -500f);
                SpriteRenderer renderer = toggleSettingsButtonObject.GetComponent<SpriteRenderer>();
                renderer.sprite = Utils.LoadSprite("TownOfThem.Resources.CurrentSettingsButton.png", 180f);
                toggleSettingsButton = toggleSettingsButtonObject.GetComponent<PassiveButton>();
                toggleSettingsButton.OnClick.RemoveAllListeners();
                toggleSettingsButton.OnClick.AddListener((Action)(() => ToggleSettings(__instance)));
            }
            toggleSettingsButtonObject.SetActive(__instance.MapButton.gameObject.active && !(MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) && GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.HideNSeek);
            toggleSettingsButtonObject.transform.localPosition = __instance.MapButton.transform.localPosition + new Vector3(0, -0.66f, -500f);
        }
    }
}