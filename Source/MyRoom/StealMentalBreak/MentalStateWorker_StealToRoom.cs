using Verse;
using Verse.AI;

namespace MyRoom.StealMentalBreak;

public class MentalStateWorker_StealToRoom : MentalStateWorker
{
    public override bool StateCanOccur(Pawn pawn)
    {
        if (!base.StateCanOccur(pawn))
        {
            return false;
        }

        var ownedBed = pawn.ownership.OwnedBed;
        return ownedBed?.GetRoom() != null && !ownedBed.GetRoom().PsychologicallyOutdoors;
    }
}