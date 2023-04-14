using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownOfThem.Patch
{
    [HarmonyPatch(typeof(MeetingHud),nameof(MeetingHud.Start))]
    class MeetingStartPatch
    {
        public static void Postfix()
        {
            Main.IsMeeting = true;
        }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.OnDestroy))]
    class MeetingOnDestroyPatch
    {
        public static void Postfix()
        {
            Main.IsMeeting = false;
        }
    }
}
