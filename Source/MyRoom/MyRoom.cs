//#define DEBUG

using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace MyRoom;

public class MyRoom : Mod
{
    public static MyModSettings latest;
    private static string currentVersion;

    public MyRoom(ModContentPack content) : base(content)
    {
        Settings = GetSettings<MyModSettings>();
        latest = Settings;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public MyModSettings Settings { get; }

    public override string SettingsCategory()
    {
        return "MyRoom - I want that!";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        listing_Standard.Gap();
        listing_Standard.Label("MyRoom.ImproveLabel".Translate());
        listing_Standard.Gap();
        Settings.timerMultiplier = Widgets.HorizontalSlider(listing_Standard.GetRect(20), Settings.timerMultiplier,
            100f, 0.1f, true, null, "MyRoom.Seldom".Translate(), "MyRoom.Often".Translate());
        listing_Standard.Gap();
        listing_Standard.Label(
            "MyRoom.WantedImpressiveness".Translate(RoomStatDefOf.Impressiveness
                .GetScoreStage(Settings.impressivenessWanted).label)
            , -1,
            "MyRoom.WantedImpressiveness.Tooltip".Translate());
        Settings.impressivenessWanted = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
            Settings.impressivenessWanted, 0, 250f, false, Settings.impressivenessWanted.ToString(), null, null, 1);
        listing_Standard.Gap();
        listing_Standard.Label(
            "MyRoom.MinimumSpace".Translate(RoomStatDefOf.Space.GetScoreStage(Settings.spaceWanted).label), -1,
            "MyRoom.MinimumSpace.Tooltip".Translate());
        Settings.spaceWanted = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
            Settings.spaceWanted, 0, 350f, false, Settings.spaceWanted.ToString(), null, null, 1);
        listing_Standard.Gap();
        listing_Standard.Label("MyRoom.TraitInfo".Translate());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("MyRoom.Version".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Settings.Write();
    }
}

public class MyModSettings : ModSettings
{
    public float impressivenessWanted = 50f;
    public float spaceWanted = 30f;
    public float timerMultiplier = 1.0f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref timerMultiplier, "TimerMultipier", 1.0f);
        Scribe_Values.Look(ref impressivenessWanted, "ImpressivenessWanted", 50f);
        Scribe_Values.Look(ref spaceWanted, "SpaceWanted", 30f);
    }
}