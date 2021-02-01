using System.Collections.Generic;
using System.Linq;
using MyRoom.Common;
using RimWorld;
using Verse;
using Verse.AI;

namespace MyRoom
{
    public class ThinkNode_InstallFurniture : ThinkNode_FurnitureJob
    {
        public override int Commonality()
        {
            return (int)((MyRoom.latest.timerMultipier * 13 * Find.Maps.Sum(x => x.mapPawns.ColonistCount)) + 1);
        }

        public override Job FurnitureJob(Pawn pawn, Building_Bed myBed, Room myRoom)
        {
            if (myRoom == null)
            {
                return null;
            }
            if (myRoom.IsRoomTooNice(pawn))
            {
                return null;
            }
            var minifiedThings = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.MinifiedThing);
            var possibleThings = minifiedThings.Where(x =>
                                    pawn.WantThat(x, myBed)
                                    && pawn.CanReserve(x)
                                    && NoPlans(x));
            if (possibleThings.Count() == 0)
            {
                return null;
            }
            possibleThings.TryRandomElementByWeight(x => x.GetBeautifulValue(), out var wanted);
            //order thing installed in my room!
            if (wanted == null)
            {
                return null;
            }


            var rot = Rot4.Random;
            var roomCells = myRoom.CellsNotNextToDoorCardinal();
            if (wanted.PlaceThing(pawn, roomCells, rot, myRoom, out var furnitureJob1) && NoPlans(wanted))
            {
                return furnitureJob1;
            }

            return null;
        }

        private static bool NoPlans(Thing x)
        {
            return InstallBlueprintUtility.ExistingBlueprintFor(x) == null;
        }
    }
}