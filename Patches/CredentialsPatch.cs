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
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuButtonPatch
    {
        public static void Start_Prefix(MainMenuManager __instance)
        {
            
        }
        
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    class ModLogoPatch
    {
        public static GameObject amongUsLogo;
        static void Postfix(MainMenuManager __instance)
        {
            if ((amongUsLogo = GameObject.Find("LOGO-AU")) != null)
            {
                amongUsLogo.transform.localScale *= 0.4f;
                amongUsLogo.transform.position += Vector3.up * 0.25f;
                amongUsLogo.GetComponent<SpriteRenderer>().sprite = ModHelpers.LoadSprite("TownOfThem.Resources.totLogo.png", 200);
                amongUsLogo.name = "totLogo";
            }
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
            if ((DateTime.Now.Month == 12) && (DateTime.Now.Day == 21))
            {
                modInfo.Append(string.Format(GetString(Language.StringKey.totBirthday), IsBeta ? ModName + "<color=#00b4eb> Beta</color>" : ModName));
            }
            else
            {
                modInfo.Append(IsBeta ? ModName + "<color=#00b4eb> Beta </color>" : ModName);
            }
            //version + credits
            modInfo.Append($"v{ModVer}\n<size=80%>" + GetString(Language.StringKey.ModInfo1) + "</size>\n");

            modInfo.Append("<size=65%>").Append(GetString(Language.StringKey.BuildTime)).Append(Main.BuildTime.ToString("yyyy-MM-dd"));
            if (IsBeta) modInfo.Append("\n").Append(GetString(Language.StringKey.ExpireTime)).Append(Main.ExpireTime.ToString());
            modInfo.Append("</size>");
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
            ping.Append(string.Format(GetString(Language.StringKey.Ping), AmongUsClient.Instance.Ping)).Append("</color>");
            __instance.text.text = VersionShowerPatch.modInfo.ToString() + ping.ToString();
            __instance.transform.localPosition = new Vector3(1f, 3f, __instance.transform.localPosition.z);
        }
    }
}
    