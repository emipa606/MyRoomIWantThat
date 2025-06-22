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
    public static bool IsPretty(this Thing x)
    {
        var thingToCheck = x.GetInnerIfMinified();
        return thingToCheck is not Building_Bed && thingToCheck.GetStatValue(StatDefOf.Beauty) > 10f;
    }

    public static float GetBeautifulValue(this Thing x)
    {
        return Math.Max(x?.GetInnerIfMinified().GetStatValue(StatDefOf.Beauty) ?? 0f, 0f);
    }

    public static bool IsBetterBed(this Thing bed, Pawn pawn, Building_Bed myBed)
    {
        return false;
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
#if DEBUG
                                Log.Message($"Found cell next to a wall, will place with rotation {placeRot}");
#endif
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
#if DEBUG
                        Log.Message("Placed furniture would cover a non-room cell (probably the door-entrance)");
#endif
                        continue;
                    }
                }

                if (!GenConstruct.CanPlaceBlueprintAt(defToPlace, vec3, placeRot, room.Map).Accepted)
                {
#if DEBUG
                        Log.Message("Not Place-able");
#endif
                    continue;
                }

                if (vec3.GetFirstBuilding(room.Map)?.def == defToPlace)
                {
#if DEBUG
                        Log.Message("Placed furniture would have replaced an item");
#endif
                    continue;
                }

                var bp = wanted.BlueprintInstall(pawn, vec3, room, placeRot);

                if (bp == null)
                {
#if DEBUG
                        Log.Message("Couldn't place blueprint, oops");
#endif
                    continue;
                }

                var job = bp.InstallJob(pawn);

                if (job == null)
                {
                    continue;
                }

                furnitureJobResult = job;
                return true;
#if DEBUG
                    Log.Message("No job for bp");
#endif
            }

            firstList = false;
        }

        furnitureJobResult = null;
        return false;
    }
}