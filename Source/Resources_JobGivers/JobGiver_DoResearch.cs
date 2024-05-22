// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

	public class JobGiver_MechDoResearch : ThinkNode_JobGiver
	{

		public float researchAmount = 1f;

		[Obsolete]
		public StatDef statDef;

		protected override Job TryGiveJob(Pawn pawn)
		{
			// if (statDef != null)
			// {
			// }
			ResearchManager researchManager = Find.ResearchManager;
			ResearchProjectDef currentProject = researchManager.GetProject();
			if (currentProject != null)
			{
				researchManager.ResearchPerformed(researchAmount, pawn);
			}
			return null;
		}

	}

}
