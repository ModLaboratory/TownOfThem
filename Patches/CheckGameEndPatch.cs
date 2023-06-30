﻿using HarmonyLib;
using TownOfThem.Modules;
using TownOfThem.Patches;
using TownOfThem.Roles.Neu;
using UnityEngine;

namespace TownOfThem.Patch
{
    enum CustomGameOverReason
    {
        BattleRoyaleLastPlayerWin,
        JesterWin,
        HostForceGameEnd,
    }
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            if (!GameData.Instance) return false;
            if (BattleRoyaleLastPlayerWin()) return false;
            if (JesterWin()) return false;
            if (CustomGameOptions.DebugMode.getBool()) return false;
            return false;
        }
        private static bool BattleRoyaleLastPlayerWin()
        {
            if (TownOfThem.ModHelpers.GetAlivePlayerList().Count == 1 && CustomGameOptions.gameModes.selection == 1)
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLastPlayerWin, false);
                return true;
            }
            return false;
        }
        private static bool JesterWin()
        {
            if (Jester.players.Contains(ExileControllerWrapUpPatch.lastExiledPlayer))
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.JesterWin, false);
                return true;
            }
            return false;
        }
    }
}
