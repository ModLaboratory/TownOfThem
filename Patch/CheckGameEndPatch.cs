﻿using HarmonyLib;
using TownOfThem.CreateCustomObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownOfThem.Patch
{
    enum CustomGameOverReason
    {
        BattleRoyaleLastPlayerWin,
    }
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            if (CustomGameOptions.DebugMode.getBool()) return false;
            if (!GameData.Instance) return false;
            if (CheckAndEndForBattleRoyaleLastPlayerWin()) return false;
            return true;
        }
        public static bool CheckAndEndForBattleRoyaleLastPlayerWin()
        {
            if (TownOfThem.ModHelpers.ModHelpers.GetAlivePlayerList().Count == 1 && CustomGameOptions.gameModes.selection == 1)
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLastPlayerWin, false);
                return true;
            }
            return false;
        }
    }
}