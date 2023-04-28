using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

namespace TownOfThem.Roles.Crew
{
    class Handicapped
    {
        public static float speed = CreateCustomObjects.CustomGameOptions.HandicappedSpeed.getFloat();
    }
}
