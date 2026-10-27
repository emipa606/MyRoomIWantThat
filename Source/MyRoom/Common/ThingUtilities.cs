//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MyRoom.Common;

public static class ThingUtilities
{
    extension(Thing x)
    {
        public bool IsPretty()
        {
            var thingToCheck = x.GetInnerIfMinified();
            return thingToCheck is not Building_Bed && thingToCheck.GetStatValue(StatDefOf.Beauty) > 10f;
        }

        public float GetBeautifulValue()
        {
            return Math.Max(x?.GetInnerIfMinified().GetStatValue(StatDefOf.Beauty) ?? 0f, 0f);
        }

        public static bool IsBetterBed(Pawn pawn, Building_Bed myBed)
        {
            return false;
        }
    }

    private static bool isNextToBorder(IntVec3 cell, IEnumerable<IntVec3> borderCells)
    {
        for (var i = 0; i < 8; i++)
        {
            var borderCellsArray = borderCells as IntVec3[] ?? borderCells.ToArray();
            if (borderCellsArray.Contains(cell + GenAdj.AdjacentCells[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool PlaceThing(this Thing wanted, Pawn pawn, IEnumerable<IntVec3> roomCells, Rot4 rot, Room room,
        out Job furnitureJobResult)
    {
        var defToPlace = wanted.GetInnerIfMinified().def;
        rot = defToPlace.rotatable ? rot : Rot4.North;
        var roomBorder = room.BorderCells;
        var roomCellsArray = roomCells as IntVec3[] ?? roomCells.ToArray();
        var wallCells = roomCellsArray.Where(cell => isNextToBorder(cell, roomBorder)).InRandomOrder();
        var innerCells = roomCellsArray.Where(cell => !wallCells.Contains(cell)).InRandomOrder();
        var firstList = true;
        foreach (var listOfCells in new List<IEnumerable<IntVec3>> { wallCells, innerCells })
        {
            foreach (var vec3 in listOfCells)
            {
                var placeRot = rot;
                if (defToPlace.rotatable && firstList)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (roomCellsArray.Contains(vec3 + GenAdj.CardinalDirections[i]))
                        {
                            continue;
                        }

                        placeRot = new Rot4(i).Opposite;
                        if (new Random().Next(2) == 0)
                        {
                            break;
                        }
                    }
                }

                if (defToPlace.size.Area > 1)
                {
                    var cellsCovered = GenAdj.OccupiedRect(vec3, placeRot, defToPlace.Size);
                    if (cellsCovered.Any(cell => !roomCellsArray.Contains(cell)))
                    {
                        continue;
                    }
                }

                if (!GenConstruct.CanPlaceBlueprintAt(defToPlace, vec3, placeRot, room.Map).Accepted)
                {
                    continue;
                }

                if (vec3.GetFirstBuilding(room.Map)?.def == defToPlace)
                {
                    continue;
                }

                var bp = wanted.BlueprintInstall(pawn, vec3, room, placeRot);

                var job = bp?.InstallJob(pawn);

                if (job == null)
                {
                    continue;
                }

                furnitureJobResult = job;
                return true;
            }

            firstList = false;
        }

        furnitureJobResult = null;
        return false;
    }
}