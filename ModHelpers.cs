using HarmonyLib;
using Hazel;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TownOfThem.Utilities;
using UnityEngine;
using TownOfThem.CustomRPCs;
using Unity.Services.Core.Telemetry.Internal;

namespace TownOfThem.ModHelpers
{
    public enum MurderAttemptResult
    {
        PerformKill,
        SuppressKill,
        BlankKill,
    }
    class ModHelpers
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
        public static void ShareVersion()
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)TownOfThem.CustomRPCs.CustomRPC.ShareModVersion, Hazel.SendOption.Reliable, -1);
            writer.Write((byte)AmongUsClient.Instance.ClientId);
            writer.Write((byte)Main.IntModVer);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.ShareModVersion(AmongUsClient.Instance.ClientId, Main.IntModVer);
        }
        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
        
    }
}
