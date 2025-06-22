using RimWorld;
using Verse;

namespace MyRoom.Common;

public static class PawnUtilities
{
    public static float MinWantedNice(this Pawn pawn)
    {
        if (isAscetic(pawn))
        {
            return 0f;
        }

        return isGreedy(pawn) ? float.MaxValue : MyRoom.Latest.ImpressivenessWanted;
    }


    private static bool isGreedy(this Pawn pawn)
    {
        return pawn.story.traits.HasTrait(TraitDefOf.Greedy);
    }

    private static bool isAscetic(this Pawn pawn)
    {
        return pawn.story.traits.HasTrait(TraitDefOf.Ascetic);
    }

    public static bool WantThat(this Pawn pawn, Thing x, Building_Bed myBed)
    {
        return x.IsPretty() || x.IsBetterBed(pawn, myBed);
    }
}