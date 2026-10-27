using RimWorld;
using Verse;

namespace MyRoom.Common;

public static class PawnUtilities
{
    extension(Pawn pawn)
    {
        public float MinWantedNice()
        {
            if (pawn.isAscetic())
            {
                return 0f;
            }

            return pawn.isGreedy() ? float.MaxValue : MyRoom.Latest.ImpressivenessWanted;
        }

        private bool isGreedy()
        {
            return pawn.story.traits.HasTrait(TraitDefOf.Greedy);
        }

        private bool isAscetic()
        {
            return pawn.story.traits.HasTrait(TraitDefOf.Ascetic);
        }

        public bool WantThat(Thing x, Building_Bed myBed)
        {
            return x.IsPretty() || Thing.IsBetterBed(pawn, myBed);
        }
    }
}