using MyRoom.Common;
using Verse;
using Verse.AI;

namespace MyRoom.StealMentalBreak;

public class JobGiver_StealFurniture : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        var mentalState = pawn.MentalState as MentalState_StealToRoom;
        if (mentalState?.Target == null)
        {
            return null;
        }

        var myRoom = pawn.ownership.OwnedRoom;
        if (myRoom == null)
        {
            return null;
        }

        return mentalState.Target.PlaceThing(pawn, myRoom.CellsNotNextToDoorCardinal(),
            mentalState.Target.def.rotatable ? Rot4.Random : Rot4.North,
            myRoom, out var furnitureJobResult)
            ? furnitureJobResult
            : null;
    }
}