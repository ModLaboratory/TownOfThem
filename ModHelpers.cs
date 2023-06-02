using HarmonyLib;
using Hazel;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TownOfThem.Utilities;
using UnityEngine;
using TownOfThem;
using Unity.Services.Core.Telemetry.Internal;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

namespace TownOfThem
{
    public enum MurderAttemptResult
    {
        PerformKill,
        SuppressKill,
        BlankKill,
    }
    public static class ModHelpers
    {
        public static Sprite LoadSprite(string path, float pixelsPerUnit = 1f)
        {
            Sprite sprite = null;
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            using MemoryStream ms = new();
            stream.CopyTo(ms);
            ImageConversion.LoadImage(texture, ms.ToArray());
            sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), pixelsPerUnit);
            return sprite;
        }
        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", Convert.ToByte(c.r), Convert.ToByte(c.g), Convert.ToByte(c.b), Convert.ToByte(c.a), s);
        }

        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;
            return null;
        }

        public static Il2CppSystem.Collections.Generic.List<PlayerControl> GetAlivePlayerList()
        {
            Il2CppSystem.Collections.Generic.List<PlayerControl> pcAliveList = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            foreach(PlayerControl pc in PlayerControl.AllPlayerControls)
            {
                if (!pc.Data.IsDead)
                {
                    pcAliveList.Add(pc);
                }
            }
            return pcAliveList;
        }
        public static PlayerControl ToPlayerControl(this GameData.PlayerInfo pinfo)
        {
            foreach(var pc in PlayerControl.AllPlayerControls)
            {
                if (pc.Data.Equals(pinfo))
                {
                    return pc;
                }
            }
            return null;
        }
    } 
}
