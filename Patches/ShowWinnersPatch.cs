using Cpp2IL.Core.Utils;
using NextTheirTown.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NextTheirTown.Patches
{
    [Patch]
    public class CustomWinnerPatch
    {
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        [Patch(typeof(EndGameManager),nameof(EndGameManager.SetEverythingUp))]
        public void OnGameEndSetEverythingUp(EndGameManager __instance)
        {
            SetUpWinnerPlayers(__instance);
            SetUpWinText(__instance);
            SetUpRoleSummary(__instance);
        }

        private static void SetUpWinnerPlayers(EndGameManager manager)
        {
            manager.transform.GetComponentsInChildren<PoolablePlayer>().ToList().ForEach(pb => pb.gameObject.Destroy());

            var num = 0;
            var ceiling = Mathf.CeilToInt(7.5f);

            TempData.winners.Clear();

            foreach (var winningPlayerData in CustomWinnerManager.AllWinners) TempData.winners.Add(winningPlayerData);

            foreach (var winner in TempData.winners.ToArray().OrderBy(b => b.IsYou ? -1 : 0))
            {
                if (!(manager.PlayerPrefab && manager.transform)) break;

                var winnerPoolable = Object.Instantiate(manager.PlayerPrefab, manager.transform);
                if (winner == null) continue;

                //var winnerRole = PlayerRole.GetRole(winner.PlayerName);
                //if (winnerRole == null!) continue;

                // ↓↓↓ These variables are from The Other Roles
                // Link: https://github.com/TheOtherRolesAU/TheOtherRoles/blob/main/TheOtherRoles/Patches/EndGamePatch.cs#L239
                // Variable names optimizing by ChatGPT
                var offsetMultiplier = num % 2 == 0 ? -1 : 1;
                var indexOffset = (num + 1) / 2;
                var lerpFactor = indexOffset / ceiling;
                var scaleLerp = Mathf.Lerp(1f, 0.75f, lerpFactor);
                float positionOffset = num == 0 ? -8 : -1;

                winnerPoolable.transform.localPosition = new Vector3(offsetMultiplier * indexOffset * scaleLerp,
                    FloatRange.SpreadToEdges(-1.125f, 0f, indexOffset, ceiling),
                    positionOffset + indexOffset * 0.01f) * 0.9f;

                var scaleValue = Mathf.Lerp(1f, 0.65f, lerpFactor) * 0.9f;
                var scale = new Vector3(scaleValue, scaleValue, 1f);

                winnerPoolable.transform.localScale = scale;
                winnerPoolable.UpdateFromPlayerOutfit(winner, PlayerMaterial.MaskType.ComplexUI, winner.IsDead, true);

                if (winner.IsDead)
                {
                    winnerPoolable.SetBodyAsGhost();
                    winnerPoolable.SetDeadFlipX(num % 2 == 0);
                }
                else
                {
                    winnerPoolable.SetFlipX(num % 2 == 0);
                }

                var namePos = winnerPoolable.cosmetics.nameText.transform.localPosition;

                winnerPoolable.SetName(winner.PlayerName + "\n" /*+ winnerRole.Name*/);
                //winnerPoolable.SetNameColor(winnerRole.Color);
                winnerPoolable.SetNamePosition(new Vector3(namePos.x, namePos.y, -15f));
                winnerPoolable.SetNameScale(new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z));

                num++;
            }
        }

        private static void SetUpWinText(EndGameManager manager)
        {
            var template = manager.WinText;
            var pos = template.transform.position;
            var winText = Object.Instantiate(template);

            winText.transform.position = new Vector3(pos.x, pos.y - 0.5f, pos.z);
            winText.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            winText.text = CustomWinnerManager.WinText;
            winText.color = CustomWinnerManager.WinColor;
            manager.BackgroundBar.material.SetColor(Color1, CustomWinnerManager.WinColor);

            // Reset
            CustomWinnerManager.SetWinText("");
            CustomWinnerManager.SetWinColor(Color.white);
            CustomWinnerManager.ResetCustomWinners();
        }

        private static void SetUpRoleSummary(EndGameManager manager)
        {
            var position = Camera.main!.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
            var roleSummary = Object.Instantiate(manager.WinText);

            roleSummary.transform.position = new Vector3(manager.Navigation.ExitButton.transform.position.x + 0.1f,
                position.y - 0.1f, -214f);
            roleSummary.transform.localScale = new Vector3(1f, 1f, 1f);
            roleSummary.fontSizeMax = roleSummary.fontSizeMin = roleSummary.fontSize = 1.5f;
            roleSummary.color = Color.white;

            StringBuilder summary = new("Player and Roles:");
            summary.Append("\n");
            //foreach (var role in PlayerRole.CachedRoles)
            //{
            //    var deadPlayer = DeadPlayerManager.DeadPlayers.FirstOrDefault(dp => dp.PlayerId == role.PlayerId);
            //    summary.Append(role.PlayerName).Append(' ')
            //        .Append(Utils.ToColorString(role.Role.Color, role.Role.Name));
            //    summary.Append(' ').Append(Utils.ToColorString(
            //        deadPlayer == null ? Palette.AcceptedGreen : Palette.ImpostorRed,
            //        deadPlayer == null ? LanguageConfig.Instance.Alive :
            //        deadPlayer.DeathReason == null ? LanguageConfig.Instance.UnknownKillReason :
            //        deadPlayer.DeathReason.GetLanguageDeathReason()));
            //    summary.Append("\n");
            //}

            roleSummary.text = summary.ToString();
        }
    }
}
