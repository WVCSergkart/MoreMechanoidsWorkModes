using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
// using WVC;

namespace WVC_WorkModes
{

    [StaticConstructorOnStartup]
	public static class PostInitializationWorkModeRequirement
	{
		static PostInitializationWorkModeRequirement()
		{
			if (WVC_MMWM.settings.WVC_HiveMindResearching || WVC_MMWM.settings.WVC_Scavenging)
			{
				List<MechWorkModeDef> mechWorkModeDefs = new();
				// List<ResearchProjectDef> researchPrerequisites = new();
				List<WorkModeResearchRequirementDef> requirementDef = new();
				string mainInfo = "\n\n" + "WVC_WorkModes_ModeTechnologyRequirement".Translate().Resolve().Colorize(ColoredText.TipSectionTitleColor);
				// string info = "";
				foreach (WorkModeResearchRequirementDef def in DefDatabase<WorkModeResearchRequirementDef>.AllDefsListForReading)
				{
					requirementDef.Add(def);
					// mechWorkModeDefs = new();
					foreach (MechWorkModeDef modeDef in def.workModes)
					{
						if (!mechWorkModeDefs.Contains(modeDef))
						{
							mechWorkModeDefs.Add(modeDef);
						}
					}
					// foreach (ResearchProjectDef researchDef in def.researchPrerequisites)
					// {
						// if (!researchPrerequisites.Contains(researchDef))
						// {
							// researchPrerequisites.Add(researchDef);
						// }
					// }
				}
				foreach (MechWorkModeDef modeDef2 in mechWorkModeDefs)
				{
					modeDef2.description += mainInfo;
					foreach (WorkModeResearchRequirementDef reqDef in requirementDef)
					{
						if (reqDef.workModes.Contains(modeDef2))
						{
							string info = "\n" + reqDef.researchPrerequisites.Select((ResearchProjectDef x) => x.label).ToLineList("  - ", capitalizeItems: true);
							modeDef2.description += info;
						}
					}
				}
				// foreach (ResearchProjectDef researchDef in researchPrerequisites)
				// {
					// if (researchDef.descriptionHyperlinks.NullOrEmpty())
					// {
						// researchDef.descriptionHyperlinks = new();
					// }
					// foreach (WorkModeResearchRequirementDef reqDef in requirementDef)
					// {
						// if (reqDef.researchPrerequisites.Contains(researchDef))
						// {
							// foreach (MechWorkModeDef modeDef3 in reqDef.workModes)
							// {
								// researchDef.descriptionHyperlinks.Add(new DefHyperlink(modeDef3));
							// }
						// }
					// }
				// }
			}
		}
	}

}
