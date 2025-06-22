//#define DEBUG

using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace MyRoom;

public class MyRoom : Mod
{
    public static MyModSettings Latest;
    private static string currentVersion;

    public MyRoom(ModContentPack content) : base(content)
    {
        Settings = GetSettings<MyModSettings>();
        Latest = Settings;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    private MyModSettings Settings { get; }

    public override string SettingsCategory()
    {
        return "MyRoom - I want that!";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.Gap();
        listingStandard.Label("MyRoom.ImproveLabel".Translate());
        listingStandard.Gap();
        Settings.TimerMultiplier = Widgets.HorizontalSlider(listingStandard.GetRect(20), Settings.TimerMultiplier,
            100f, 0.1f, true, null, "MyRoom.Seldom".Translate(), "MyRoom.Often".Translate());
        listingStandard.Gap();
        listingStandard.Label(
            "MyRoom.WantedImpressiveness".Translate(RoomStatDefOf.Impressiveness
                .GetScoreStage(Settings.ImpressivenessWanted).label)
            , -1,
            "MyRoom.WantedImpressiveness.Tooltip".Translate());
        Settings.ImpressivenessWanted = Widgets.HorizontalSlider(listingStandard.GetRect(20),
            Settings.ImpressivenessWanted, 0, 250f, false, Settings.ImpressivenessWanted.ToString(), null, null, 1);
        listingStandard.Gap();
        listingStandard.Label(
            "MyRoom.MinimumSpace".Translate(RoomStatDefOf.Space.GetScoreStage(Settings.SpaceWanted).label), -1,
            "MyRoom.MinimumSpace.Tooltip".Translate());
        Settings.SpaceWanted = Widgets.HorizontalSlider(listingStandard.GetRect(20),
            Settings.SpaceWanted, 0, 350f, false, Settings.SpaceWanted.ToString(), null, null, 1);
        listingStandard.Gap();
        listingStandard.Label("MyRoom.TraitInfo".Translate());
        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("MyRoom.Version".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Settings.Write();
    }
}

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