using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MyRoom.Common;

public static class RoomUtilities
{
    extension(Room room)
    {
        public bool IsRoomTooNice(Pawn pawn)
        {
            var roomStatDef = RoomStatDefOf.Impressiveness;
            return room.GetStat(roomStatDef) > pawn.MinWantedNice();
        }

        public bool IsRoomTooCramped()
        {
            var roomStatDef = RoomStatDefOf.Space;
            return room.GetStat(roomStatDef) < MyRoom.Latest.SpaceWanted;
        }

        public bool HasAlreadyBluePrint()
        {
            return (from Thing blueprint in room.ContainedAndAdjacentThings
                where blueprint is Blueprint
                select blueprint).Any();
        }

        public List<IntVec3> CellsNotNextToDoorCardinal()
        {
            var doors = new List<Building_Door>();

            foreach (var thing in room.ContainedAndAdjacentThings)
            {
                if (thing is Building_Door door)
                {
                    doors.Add(door);
                }
            }

            var cells = room.Cells.ToList();
            foreach (var buildingDoor in doors)
            {
                foreach (var cell in GenAdj.CellsAdjacentCardinal(buildingDoor))
                {
                    cells.Remove(cell);
                }
            }

            return cells;
        }
    }
}