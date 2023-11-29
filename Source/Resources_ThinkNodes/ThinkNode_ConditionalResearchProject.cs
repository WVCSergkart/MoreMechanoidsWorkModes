// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

namespace WVC_WorkModes
{

	public class ThinkNode_ConditionalResearchProjects : ThinkNode_Conditional
	{

		public List<ResearchProjectDef> allOfResearchProjects;

		public List<ResearchProjectDef> anyOfResearchProjects;

		protected override bool Satisfied(Pawn pawn)
		{
			if (AllProjectsFinished(allOfResearchProjects) || AnyProjectFinished(anyOfResearchProjects))
			{
				return true;
			}
			return false;
		}

		public static bool AllProjectsFinished(List<ResearchProjectDef> researchProjects)
		{
			if (!researchProjects.NullOrEmpty())
			{
				for (int i = 0; i < researchProjects?.Count; i++)
				{
					if (researchProjects[i] == null || !researchProjects[i].IsFinished)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool AnyProjectFinished(List<ResearchProjectDef> researchProjects)
		{
			if (!researchProjects.NullOrEmpty())
			{
				for (int i = 0; i < researchProjects?.Count; i++)
				{
					if (researchProjects[i] != null && researchProjects[i].IsFinished)
					{
						return true;
					}
				}
			}
			return false;
		}

	}

}
