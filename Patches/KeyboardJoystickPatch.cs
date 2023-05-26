using InnerNet;
using TownOfThem.Patch;
using UnityEngine;

namespace TownOfThem.Patches
{
    [HarmonyPatch(typeof(KeyboardJoystick),nameof(KeyboardJoystick.Update))]
    class KeyBoardJoyStickPatch
    {
        public static void Postfix()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.E) && AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HostForceGameEnd, false);
            }
        }
    }
}
