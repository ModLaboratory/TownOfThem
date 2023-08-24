using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NextTheirTown.Modules
{

    public static class CustomWinnerManager
    {
        public static Il2CppSystem.Collections.Generic.List<WinningPlayerData> AllWinners { get; } = new();

        public static string WinText { get; private set; } = "";
        public static Color WinColor { get; private set; } = Color.white;

        public static void RegisterCustomWinner(PlayerControl winner)
        {
            AllWinners.Add(new WinningPlayerData(winner.Data));
        }

        public static void RegisterCustomWinners(IEnumerable<PlayerControl> winners)
        {
            Enumerable.ToList(winners).ForEach(w => AllWinners.Add(new WinningPlayerData(w.Data)));
        }

        public static void ResetCustomWinners()
        {
            AllWinners.Clear();
        }

        public static void SetWinText(string text)
        {
            WinText = text;
        }

        public static void SetWinColor(Color color)
        {
            WinColor = color;
        }
    }
}
