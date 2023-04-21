using HarmonyLib;
using TownOfThem.CreateCustomObjects;
using UnityEngine;

namespace TownOfThem.Patch
{
    enum CustomGameOverReason
    {
        BattleRoyaleLastPlayerWin,
        HostForceGameEnd,
    }
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            if (!GameData.Instance) return false;
            if (CheckAndEndForHostPressesHotkeyToForceEndGame()) return false;
            if (CheckAndEndForBattleRoyaleLastPlayerWins()) return false;
            if (CustomGameOptions.DebugMode.getBool()) return false;
            return true;
        }
        private static bool CheckAndEndForBattleRoyaleLastPlayerWins()
        {
            if (TownOfThem.ModHelpers.ModHelpers.GetAlivePlayerList().Count == 1 && CustomGameOptions.gameModes.selection == 1)
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLastPlayerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndForHostPressesHotkeyToForceEndGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HostForceGameEnd, false);
                return true;
            }
            return false;
        }
    }
}
