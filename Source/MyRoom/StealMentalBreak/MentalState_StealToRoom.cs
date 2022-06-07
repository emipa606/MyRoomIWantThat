using System.Collections.Generic;
using System.Linq;
using MyRoom.Common;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MyRoom.StealMentalBreak;

public class MentalState_StealToRoom : MentalState
{
    private bool insultedTargetAtLeastOnce;
    private int lastInsultTicks = -999999;
    public Thing target;
    private int targetFoundTicks;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref targetFoundTicks, "targetFoundTicks");
        Scribe_References.Look(ref target, "target");
        Scribe_Values.Look(ref insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce");
        Scribe_Values.Look(ref lastInsultTicks, "lastInsultTicks");
    }

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Quiet;
    }

    public override void PostStart(string reason)
    {
        base.PostStart(reason);
        ChooseNextTarget();
    }

    public override void MentalStateTick()
    {
        if (target != null && pawn.CanReach(target.Position, PathEndMode.Touch, Danger.Some))
        {
            ChooseNextTarget();
        }

        if (pawn.IsHashIntervalTick(250) && (target == null || insultedTargetAtLeastOnce))
        {
            ChooseNextTarget();
        }

        base.MentalStateTick();
    }

    private void ChooseNextTarget()
    {
        var candidates = Candidates();
        if (candidates?.Any() == false)
        {
            target = null;
            insultedTargetAtLeastOnce = false;
            targetFoundTicks = -1;
            return;
        }

        Thing thing = null;
        if (target != null && Find.TickManager.TicksGame - targetFoundTicks > 1250 &&
            candidates.Any(x => x != target))
        {
            if (candidates != null)
            {
                thing = candidates.Where(x => x != target).RandomElementByWeight(GetCandidateWeight);
            }
        }

        if (thing == null || thing == target || !NoPlan(thing))
        {
            return;
        }

        target = thing;
        insultedTargetAtLeastOnce = false;
        targetFoundTicks = Find.TickManager.TicksGame;
    }

    private List<Thing> Candidates()
    {
        var room = pawn.ownership.OwnedRoom;
        if (room == null)
        {
            return null;
        }

        var movable = pawn.Map.listerThings.AllThings.Where(x =>
                pawn.CanReserve(x) && x.def.Minifiable && NoPlan(x))
            .ToList();
        foreach (var thing in room.ContainedAndAdjacentThings)
        {
            movable.Remove(thing);
        }

        return movable;
    }

    private static bool NoPlan(Thing x)
    {
        return InstallBlueprintUtility.ExistingBlueprintFor(x) == null;
    }

    private float GetCandidateWeight(Thing candidate)
    {
        var num = pawn.Position.DistanceTo(candidate.Position);
        var num2 = Mathf.Min(num / 40f, 1f);
        return (1f - num2 + 0.01f) * candidate.GetBeautifulValue();
    }
}