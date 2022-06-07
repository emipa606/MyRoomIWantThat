using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MyRoom.Common;

public static class RoomUtilities
{
    public static bool IsRoomTooNice(this Room room, Pawn pawn)
    {
        var roomStatDef = RoomStatDefOf.Impressiveness;
        return room.GetStat(roomStatDef) > pawn.MinWantedNice();
    }

    public static bool IsRoomTooCramped(this Room room)
    {
        var roomStatDef = RoomStatDefOf.Space;
        return room.GetStat(roomStatDef) < MyRoom.latest.spaceWanted;
    }

    public static bool HasAlreadyBluePrint(this Room room)
    {
        return (from Thing blueprint in room.ContainedAndAdjacentThings
            where blueprint is Blueprint
            select blueprint).Any();
    }

    public static List<IntVec3> CellsNotNextToDoorCardinal(this Room room)
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