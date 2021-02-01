using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MyRoom.Common
{
    public static class RoomUtilities
    {
        public static bool IsRoomTooNice(this Room room, Pawn pawn)
        {
            RoomStatDef roomStatDef = RoomStatDefOf.Impressiveness;
            return room.GetStat(roomStatDef) > pawn.MinWantedNice();
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


        [Obsolete("MyRoom is deprecated, use the builtin pawn.ownership.OwnedRoom")]
        public static List<Room> MyRoom(List<Building_Bed> myBed)
        {
            var myRoom = myBed?.Select(x => x.GetRoom()).ToList();
            return myRoom;
        }
    }
}