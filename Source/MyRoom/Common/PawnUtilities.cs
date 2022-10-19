using RimWorld;
using Verse;

namespace MyRoom.Common;

public static class PawnUtilities
{
    public static float MinWantedNice(this Pawn pawn)
    {
        if (IsAscetic(pawn))
        {
            return 0f;
        }

        return IsGreedy(pawn) ? float.MaxValue : MyRoom.latest.impressivenessWanted;
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