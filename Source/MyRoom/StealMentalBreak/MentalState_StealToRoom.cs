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
    public Thing Target;
    private int targetFoundTicks;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref targetFoundTicks, "targetFoundTicks");
        Scribe_References.Look(ref Target, "target");
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
        chooseNextTarget();
    }

    public override void MentalStateTick(int delta)
    {
        if (Target != null && pawn.CanReach(Target.Position, PathEndMode.Touch, Danger.Some))
        {
            chooseNextTarget();
        }

        if (pawn.IsHashIntervalTick(250, delta) && (Target == null || insultedTargetAtLeastOnce))
        {
            chooseNextTarget();
        }

        base.MentalStateTick(delta);
    }

    private void chooseNextTarget()
    {
        var candidates = this.candidates();
        if (candidates?.Any() == false)
        {
            Target = null;
            insultedTargetAtLeastOnce = false;
            targetFoundTicks = -1;
            return;
        }

        Thing thing = null;
        if (Target != null && Find.TickManager.TicksGame - targetFoundTicks > 1250 &&
            candidates.Any(x => x != Target))
        {
            if (candidates != null)
            {
                thing = candidates.Where(x => x != Target).RandomElementByWeight(getCandidateWeight);
            }
        }

        if (thing == null || thing == Target || !noPlan(thing))
        {
            return;
        }

        Target = thing;
        insultedTargetAtLeastOnce = false;
        targetFoundTicks = Find.TickManager.TicksGame;
    }

    private List<Thing> candidates()
    {
        var room = pawn.ownership.OwnedRoom;
        if (room == null)
        {
            return null;
        }

        var movable = pawn.Map.listerThings.AllThings.Where(x =>
                pawn.CanReserve(x) && x.def.Minifiable && noPlan(x))
            .ToList();
        foreach (var thing in room.ContainedAndAdjacentThings)
        {
            movable.Remove(thing);
        }

        return movable;
    }

    private static bool noPlan(Thing x)
    {
        return InstallBlueprintUtility.ExistingBlueprintFor(x) == null;
    }

    private float getCandidateWeight(Thing candidate)
    {
        var num = pawn.Position.DistanceTo(candidate.Position);
        var num2 = Mathf.Min(num / 40f, 1f);
        return (1f - num2 + 0.01f) * candidate.GetBeautifulValue();
    }
}