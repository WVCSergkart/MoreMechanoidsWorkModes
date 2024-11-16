using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
// using Verse.AI;
// using WVC;

namespace WVC_WorkModes
{

    [StaticConstructorOnStartup]
    public static class PostInitializationWorkModeRequirement
    {
        static PostInitializationWorkModeRequirement()
        {
            if (WVC_MMWM.settings.firstModLaunch)
            {
                WVC_MMWM.settings.firstModLaunch = false;
                WVC_MMWM.settings.Write();
            }
            AutoPatch();
        }

        public static void AutoPatch()
        {
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (thingDef?.race == null)
                {
                    continue;
                }
                if (!thingDef.race.IsMechanoid)
                {
                    continue;
                }
                if (!thingDef.HasComp<CompOverseerSubject>())
                {
                    continue;
                }
                if (!thingDef.HasComp<CompMechSettings>())
                {
                    thingDef.comps.Add(new CompProperties_MechSettings());
                }
            }
        }

    }

}
