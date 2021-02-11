//#define DEBUG

using RimWorld;
using UnityEngine;
using Verse;

namespace MyRoom
{
    public class MyRoom : Mod
    {
        public static MyModSettings latest;

        public MyRoom(ModContentPack content) : base(content)
        {
            Settings = GetSettings<MyModSettings>();
            latest = Settings;
        }

        public MyModSettings Settings { get; set; }

        public override string SettingsCategory()
        {
            return "MyRoom - I want that!";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Gap();
            listing_Standard.Label("Improve rooms frequency multiplier");
            listing_Standard.Gap();
            Settings.timerMultiplier = Widgets.HorizontalSlider(listing_Standard.GetRect(20), Settings.timerMultiplier,
                100f, 0.1f, true, null, "Seldom", "Often");
            listing_Standard.Gap();
            listing_Standard.Label(
                "Wanted impressiveness: " +
                RoomStatDefOf.Impressiveness.GetScoreStage(Settings.impressivenessWanted).label, -1,
                "Pawns will try to improve their room to this score");
            Settings.impressivenessWanted = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
                Settings.impressivenessWanted, 0, 250f, false, Settings.impressivenessWanted.ToString(), null, null, 1);
            listing_Standard.Gap();
            listing_Standard.Label(
                "Minimum space needed: " +
                RoomStatDefOf.Space.GetScoreStage(Settings.spaceWanted).label, -1,
                "Pawns will not improve their room if the room have less space than this value");
            Settings.spaceWanted = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
                Settings.spaceWanted, 0, 350f, false, Settings.spaceWanted.ToString(), null, null, 1);
            listing_Standard.Gap();
            listing_Standard.Label("Ascetic pawns will not improve their room, Greedy will always do it");
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
}