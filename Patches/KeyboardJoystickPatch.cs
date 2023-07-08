using InnerNet;
using TownOfThem.Patches;
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
                Main.Log.LogInfo("check");
            }
            if((Input.GetKeyDown(KeyCode.LeftShift)|| Input.GetKeyDown(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.T))
            {
                
            }
        }
    }
}
