using System;
using System.Collections.Generic;
using System.Linq;
using MyRoom.Common;
using RimWorld;
using Verse;
using Verse.AI;

namespace MyRoom
{
    public abstract class ThinkNode_FurnitureJob : ThinkNode_JobGiver
    {
        private static short _tick = 0;

        protected override Job TryGiveJob(Pawn pawn)
        {
            _tick += 1;
            _tick %= 29387;
            //semi-rare tick
            if (_tick % Math.Min(29387, Commonality()) != 0
                || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
                || !pawn.RaceProps.ToolUser
                || pawn.IsPrisoner)
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
}