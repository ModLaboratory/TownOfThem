using AmongUs.GameOptions;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace TownOfThem.Roles
{
    public enum ModRoleTypes
    {
        Crew,
        Neu,
        Imp,
    }
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class SelectRolesPatch
    {
        
        public static void Postfix(RoleManager __instance)
        {
            
        }
        //public static void
        public static void assign()
        {

        }
        public static PlayerRolesData getPlayersByAURoleType()
        {
            List<PlayerControl> crew = new();
            List<PlayerControl> imp = new();
            foreach (PlayerControl pc in PlayerControl.AllPlayerControls)
            {
                switch (pc.Data.RoleType)
                {
                    case RoleTypes.Crewmate:
                    case RoleTypes.Engineer:
                    case RoleTypes.Scientist:
                        crew.Add(pc);
                        break;
                    case RoleTypes.Impostor:
                    case RoleTypes.Shapeshifter:
                        imp.Add(pc);
                        break;
                }
            }
            return new(crew, null, imp);
        }

    }
    public class PlayerRolesData
    {
        public List<PlayerControl> Crewmate = new();
        public List<PlayerControl> Neutral = new();
        public List<PlayerControl> Impostor = new();
        public PlayerRolesData(List<PlayerControl> crew, List<PlayerControl> neu, List<PlayerControl> imp)
        {
            this.Crewmate = crew;
            this.Neutral = neu;
            this.Impostor = imp;
        }
    }
}
