using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfThem.Utilities;
using static TownOfThem.CustomObjects.CustomOption;
using TownOfThem.GetGameOptionsText;
using static TownOfThem.Language.Translation;

//used TheOtherRoles' code
namespace TownOfThem.CustomObjects
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
            Guesser,
            HideNSeekMain,
            HideNSeekRoles
        }

        public static List<CustomOption> options = new List<CustomOption>();
        public static int preset = 0;
        public static ConfigEntry<string> vanillaSettings;

        public int id;
        public string name;
        public System.Object[] selections;

        public int defaultSelection;
        public ConfigEntry<int> entry;
        public int selection;
        public OptionBehaviour optionBehaviour;
        public CustomOption parent;
        public bool isHeader;
        public CustomOptionType type;

        // Option creation

        public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader)
        {
            this.id = id;
            this.name = parent == null ? name : "- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            this.type = type;
            selection = 0;
            /*if (id != 0)
            {
                entry = TownOfThem.Main.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }*/
            options.Add(this);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false)
        {
            Main.Log.LogInfo("Custom Option Added:" + id + "," + type + "," + name + "," + parent + "," + isHeader);
            return new CustomOption(id, type, name, selections, "", parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false)
        {
            List<object> selections = new();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            Main.Log.LogInfo("Custom Option Added:" + id + "," + type + "," + name + "," + defaultValue + "," + min + "," + max + "," + step + "," + parent + "," + isHeader);
            return new CustomOption(id, type, name, selections.ToArray(), defaultValue, parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false)
        {
            Main.Log.LogInfo("Custom Option Added:" + id + "," + type + "," + name + "," + defaultValue + "," + parent + "," + isHeader);
            return new CustomOption(id, type, name, new string[] { LoadTranslation("Off"), LoadTranslation("On") }, defaultValue ? LoadTranslation("On") : LoadTranslation("Off"), parent, isHeader);
        }

        // Static behaviour

        public static void switchPreset(int newPreset)
        {
            saveVanillaOptions();
            CustomOption.preset = newPreset;
            vanillaSettings = TownOfThem.Main.Instance.Config.Bind($"Preset{preset}", "GameOptions", "");
            loadVanillaOptions();
            foreach (CustomOption option in CustomOption.options)
            {
                if (option.id == 0) continue;

                option.entry = TownOfThem.Main.Instance.Config.Bind($"Preset{preset}", option.id.ToString(), option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();
                }
            }
        }

        public static void saveVanillaOptions()
        {
            vanillaSettings.Value = Convert.ToBase64String(GameOptionsManager.Instance.gameOptionsFactory.ToBytes(GameManager.Instance.LogicOptions.currentGameOptions));
        }

        public static void loadVanillaOptions()
        {
            string optionsString = vanillaSettings.Value;
            if (optionsString == "") return;
            GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.gameOptionsFactory.FromBytes(Convert.FromBase64String(optionsString));
            GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
            GameManager.Instance.LogicOptions.SetGameOptions(GameOptionsManager.Instance.CurrentGameOptions);
            GameManager.Instance.LogicOptions.SyncOptions();
        }

        public static void ShareOptionChange(uint optionId)
        {
            var option = options.FirstOrDefault(x => x.id == optionId);
            if (option == null) return;
            var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)TownOfThem.CustomRPCs.CustomRPC.ShareOptions, SendOption.Reliable, -1);
            writer.Write((byte)1);
            writer.WritePacked((uint)option.id);
            writer.WritePacked(Convert.ToUInt32(option.selection));
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void ShareOptionSelections()
        {
            if (CachedPlayer.AllPlayers.Count <= 1 || AmongUsClient.Instance!.AmHost == false && CachedPlayer.LocalPlayer.PlayerControl == null) return;
            var optionsList = new List<CustomOption>(CustomOption.options);
            while (optionsList.Any())
            {
                byte amount = (byte)Math.Min(optionsList.Count, 200); // takes less than 3 bytes per option on average
                var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)TownOfThem.CustomRPCs.CustomRPC.ShareOptions, SendOption.Reliable, -1);
                writer.Write(amount);
                for (int i = 0; i < amount; i++)
                {
                    var option = optionsList[0];
                    optionsList.RemoveAt(0);
                    writer.WritePacked((uint)option.id);
                    writer.WritePacked(Convert.ToUInt32(option.selection));
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        // Getter

        public int getSelection()
        {
            return selection;
        }

        public bool getBool()
        {
            return selection > 0;
        }

        public float getFloat()
        {
            return (float)selections[selection];
        }

        public int getQuantity()
        {
            return selection + 1;
        }

        // Option changes

        public void updateSelection(int newSelection)
        {
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (optionBehaviour != null && optionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = selections[selection].ToString();

                if (AmongUsClient.Instance?.AmHost == true && CachedPlayer.LocalPlayer.PlayerControl)
                {
                    if (id == 0 && selection != preset)
                    {
                        switchPreset(selection); // Switch presets
                        ShareOptionSelections();
                    }
                    else if (entry != null)
                    {
                        entry.Value = selection; // Save selection to config
                        ShareOptionChange((uint)id);// Share single selection
                    }
                }
            }
            else if (id == 0 && AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
            {  // Share the preset switch for random maps, even if the menu isnt open!
                switchPreset(selection);
                ShareOptionSelections();// Share all selections
            }
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
                    ["totSettings"] = LoadTranslation("totSettings"),
                    ["ImpostorSettings"] = LoadTranslation("ImpostorSettings"),
                    ["NeutralSettings"] = LoadTranslation("NeutralSettings"),
                    ["CrewmateSettings"] = LoadTranslation("CrewmateSettings"),
                    ["ModifierSettings"] = LoadTranslation("ModifierSettings")
                });

            if (isReturn) return;

            // Setup TOR tab
            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var totSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var totMenu = getMenu(totSettings, "TOTSettings");

            var ImpostorSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var ImpostorMenu = getMenu(ImpostorSettings, "ImpostorSettings");

            var neutralSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

            var crewmateSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

            var modifierSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");

            var torTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var torTabHighlight = getTabHighlight(torTab, "TownOfThemTab", "TownOfThem.Resources.TabIcon.png");

            var impostorTab = UnityEngine.Object.Instantiate(roleTab, torTab.transform);
            var impostorTabHighlight = getTabHighlight(impostorTab, "ImpostorTab", "TownOfThem.Resources.TabIconImpostor.png");

            var neutralTab = UnityEngine.Object.Instantiate(roleTab, impostorTab.transform);
            var neutralTabHighlight = getTabHighlight(neutralTab, "NeutralTab", "TownOfThem.Resources.TabIconNeutral.png");

            var crewmateTab = UnityEngine.Object.Instantiate(roleTab, neutralTab.transform);
            var crewmateTabHighlight = getTabHighlight(crewmateTab, "CrewmateTab", "TownOfThem.Resources.TabIconCrewmate.png");

            var modifierTab = UnityEngine.Object.Instantiate(roleTab, crewmateTab.transform);
            var modifierTabHighlight = getTabHighlight(modifierTab, "ModifierTab", "TownOfThem.Resources.TabIconModifier.png");

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
                [totSettings.gameObject] = torTabHighlight,
                [ImpostorSettings.gameObject] = impostorTabHighlight,
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
                    setListener(settingsHighlightMap, copiedIndex);
                }));
            }

            destroyOptions(new List<List<OptionBehaviour>>{
                totMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                ImpostorMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                neutralMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                crewmateMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                modifierMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
            });

            List<OptionBehaviour> torOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> impostorOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> neutralOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> crewmateOptions = new List<OptionBehaviour>();
            List<OptionBehaviour> modifierOptions = new List<OptionBehaviour>();


            List<Transform> menus = new List<Transform>() { totMenu.transform, ImpostorMenu.transform, neutralMenu.transform, crewmateMenu.transform, modifierMenu.transform };
            List<List<OptionBehaviour>> optionBehaviours = new List<List<OptionBehaviour>>() { torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions };

            for (int i = 0; i < CustomOption.options.Count; i++)
            {
                CustomOption option = CustomOption.options[i];
                if ((int)option.type > 4) continue;
                if (option.optionBehaviour == null)
                {
                    StringOption stringOption = UnityEngine.Object.Instantiate(template, menus[(int)option.type]);
                    optionBehaviours[(int)option.type].Add(stringOption);
                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                    stringOption.TitleText.text = option.name;
                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();

                    option.optionBehaviour = stringOption;
                }
                option.optionBehaviour.gameObject.SetActive(true);
            }

            setOptions(
                new List<GameOptionsMenu> { totMenu, ImpostorMenu, neutralMenu, crewmateMenu, modifierMenu },
                new List<List<OptionBehaviour>> { torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions },
                new List<GameObject> { totSettings, ImpostorSettings, neutralSettings, crewmateSettings, modifierSettings }
            );

            adaptTaskCount(__instance);
        }

        private static void setListener(Dictionary<GameObject, SpriteRenderer> settingsHighlightMap, int index)
        {
            foreach (KeyValuePair<GameObject, SpriteRenderer> entry in settingsHighlightMap)
            {
                entry.Key.SetActive(false);
                entry.Value.enabled = false;
            }
            settingsHighlightMap.ElementAt(index).Key.SetActive(true);
            settingsHighlightMap.ElementAt(index).Value.enabled = true;
        }

        private static void destroyOptions(List<List<OptionBehaviour>> optionBehavioursList)
        {
            foreach (List<OptionBehaviour> optionBehaviours in optionBehavioursList)
            {
                foreach (OptionBehaviour option in optionBehaviours)
                    UnityEngine.Object.Destroy(option.gameObject);
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
            tab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = TownOfThem.ModHelpers.ModHelpers.LoadSprite(tabSpritePath, 100f);
            tab.name = "tabName";

            return tabHighlight;
        }

        private static void setOptions(List<GameOptionsMenu> menus, List<List<OptionBehaviour>> options, List<GameObject> settings)
        {
            if (!(menus.Count == options.Count && options.Count == settings.Count))
            {
                TownOfThem.Main.Log.LogError("List counts are not equal");
                return;
            }
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].Children = options[i].ToArray();
                settings[i].gameObject.SetActive(false);
            }
        }

        private static void adaptTaskCount(GameOptionsMenu __instance)
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
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = option.name;
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.selections[option.selection].ToString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            CustomOption.ShareOptionSelections();
            CustomOption.saveVanillaOptions();
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    public class AmongUsClientOnPlayerJoinedPatch
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer != null)
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
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            float offset = 2.75f;
            foreach (CustomOption option in CustomOption.options)
            {
                if (GameObject.Find("totSettings") && option.type != CustomOption.CustomOptionType.General && option.type != CustomOptionType.HideNSeekMain)
                    continue;
                if (GameObject.Find("ImpostorSettings") && option.type != CustomOption.CustomOptionType.Impostor)
                    continue;
                if (GameObject.Find("NeutralSettings") && option.type != CustomOption.CustomOptionType.Neutral)
                    continue;
                if (GameObject.Find("CrewmateSettings") && option.type != CustomOption.CustomOptionType.Crewmate)
                    continue;
                if (GameObject.Find("ModifierSettings") && option.type != CustomOption.CustomOptionType.Modifier)
                    continue;
                if (GameObject.Find("GuesserSettings") && option.type != CustomOption.CustomOptionType.Guesser)
                    continue;
                if (GameObject.Find("HideNSeekSettings") && option.type != CustomOption.CustomOptionType.HideNSeekRoles)
                    continue;
                if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null)
                {
                    bool enabled = true;
                    var parent = option.parent;
                    while (parent != null && enabled)
                    {
                        enabled = parent.selection != 0;
                        parent = parent.parent;
                    }
                    option.optionBehaviour.gameObject.SetActive(enabled);
                    if (enabled)
                    {
                        offset -= option.isHeader ? 0.75f : 0.5f;
                        option.optionBehaviour.transform.localPosition = new Vector3(option.optionBehaviour.transform.localPosition.x, offset, option.optionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }


    [HarmonyPatch]
    class GameOptionsDataPatch
    {
        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
        private static void Postfix(ref string __result)
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == AmongUs.GameOptions.GameModes.HideNSeek) return; // Allow Vanilla Hide N Seek
            int counter = TownOfThem.Main.optionsPage;
            string hudString = counter != 0 ? TownOfThem.ModHelpers.ModHelpers.cs(DateTime.Now.Second % 2 == 0 ? Color.white : Color.red, "(Use scroll wheel if necessary)\n\n") : "";
            int maxPage = 6;
                switch (counter)
                {
                    case 0:
                        hudString += $"{LoadTranslation("ckptPage1")} \n\n" + __result;
                        break;
                    case 1:
                        hudString += $"{LoadTranslation("ckptPage2")} \n" + Cockpit_GetGameOptionText.GetModSettingGameOptions();
                        break;
                    case 2:
                        hudString += $"{LoadTranslation("ckptPage3")} \n" + Cockpit_GetGameOptionText.GetImpostorGameOptions();
                        break;
                    case 3:
                        hudString += $"{LoadTranslation("ckptPage4")} \n" + Cockpit_GetGameOptionText.GetNeutralGameOptions();
                        break;
                    case 4:
                        hudString += $"{LoadTranslation("ckptPage5")} \n" + Cockpit_GetGameOptionText.GetCrewmateGameOptions();
                        break;
                    case 5:
                        hudString += $"{LoadTranslation("ckptPage6")} \n" + Cockpit_GetGameOptionText.GetModifierGameOptions();
                        break;
                }
            

            hudString += $"\n {LoadTranslation("ckptToOtherPages")} ({counter + 1}/{maxPage})";
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            int page = TownOfThem.Main.optionsPage;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TownOfThem.Main.optionsPage = (TownOfThem.Main.optionsPage + 1) % 6;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                TownOfThem.Main.optionsPage = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                TownOfThem.Main.optionsPage = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                TownOfThem.Main.optionsPage = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                TownOfThem.Main.optionsPage = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                TownOfThem.Main.optionsPage = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                TownOfThem.Main.optionsPage = 5;
            }
            if (page != TownOfThem.Main.optionsPage)
            {
                Vector3 position = (Vector3)FastDestroyableSingleton<HudManager>.Instance?.GameSettings?.transform.localPosition;
                FastDestroyableSingleton<HudManager>.Instance.GameSettings.transform.localPosition = new Vector3(position.x, 2.9f, position.z);
            }
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


    // This class is taken from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patch/CustomOption/Patch.cs, Licensed under GPLv3
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
            if (CachedPlayer.LocalPlayer?.PlayerControl.CanMove != true)
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

            Scroller.Inner = __instance.GameSettings.transform;
            __instance.GameSettings.transform.SetParent(Scroller.transform);
        }
    }
}