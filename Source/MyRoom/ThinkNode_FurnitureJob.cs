using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace MyRoom;

public abstract class ThinkNode_FurnitureJob : ThinkNode_JobGiver
{
    private static short tick;

    protected override Job TryGiveJob(Pawn pawn)
    {
        tick += 1;
        tick %= 29387;
        //semi-rare tick
        if (tick % Math.Min(29387, Commonality()) != 0
            || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
            || !pawn.RaceProps.ToolUser
            || pawn.IsPrisoner
            || !pawn.IsColonist)
        {
            return null;
        }

        var myBed = pawn.ownership.OwnedBed;

        var myRoom = pawn.ownership.OwnedRoom;

        return myRoom == null ? null : FurnitureJob(pawn, myBed, myRoom);
    }

    public abstract int Commonality();

    public abstract Job FurnitureJob(Pawn pawn, Building_Bed myBed, Room myRoom);
}