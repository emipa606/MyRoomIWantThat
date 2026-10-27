using Verse;

namespace MyRoom;

public class MyModSettings : ModSettings
{
    public float ImpressivenessWanted = 50f;
    public float SpaceWanted = 30f;
    public float TimerMultiplier = 1.0f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref TimerMultiplier, "TimerMultipier", 1.0f);
        Scribe_Values.Look(ref ImpressivenessWanted, "ImpressivenessWanted", 50f);
        Scribe_Values.Look(ref SpaceWanted, "SpaceWanted", 30f);
    }
}