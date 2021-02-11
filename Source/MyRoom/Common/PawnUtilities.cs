using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MyRoom.Common
{
    public static class PawnUtilities
    {
        [Obsolete("MyBeds is deprecated, use the builtin pawn.ownership.OwnedBed")]
        public static List<Building_Bed> MyBeds(this Pawn pawn)
        {
            var myBed = (from Building_Bed bed in pawn.Map.listerBuildings.allBuildingsColonist
                where bed?.GetAssignedPawns() != null && bed.GetAssignedPawns().Contains(pawn)
                select bed).ToList();
            //allRooms = pawn.Map.regionGrid.allRooms;
            return myBed;
        }

        public static float MinWantedNice(this Pawn pawn)
        {
            if (IsAscetic(pawn))
            {
                return 0f;
            }

            if (IsGreedy(pawn))
            {
                return float.MaxValue;
            }

            return MyRoom.latest.impressivenessWanted;
        }


        public static int MaxWantedNice(this Pawn pawn)
        {
            return IsAscetic(pawn) ? -1 : short.MaxValue;
        }

        private static bool IsGreedy(this Pawn pawn)
        {
            return pawn.story.traits.HasTrait(TraitDefOf.Greedy);
        }

        private static bool IsAscetic(this Pawn pawn)
        {
            return pawn.story.traits.HasTrait(TraitDefOf.Ascetic);
        }

        public static bool WantThat(this Pawn pawn, Thing x, Building_Bed myBed)
        {
            return x.IsPretty() || x.IsBetterBed(pawn, myBed);
        }
    }
}