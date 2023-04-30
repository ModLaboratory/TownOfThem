using UnityEngine;

namespace TownOfThem;
public static class Utils
{
    public static string ColorString(Color32 color, string str) => $"<color=#{color.r:x2}{color.g:x2}{color.b:x2}{color.a:x2}>{str}</color>";




}
