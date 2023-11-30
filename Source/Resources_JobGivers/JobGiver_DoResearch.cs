// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class JobGiver_MechDoResearch : ThinkNode_JobGiver
	{

		public float researchFactor = 1f;

		public StatDef statDef;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (statDef != null)
			{
				ResearchManager researchManager = Find.ResearchManager;
				ResearchProjectDef currentProject = researchManager.currentProj;
				if (currentProject != null)
				{
					researchManager.ResearchPerformed(1f + (pawn.GetStatValue(statDef) * researchFactor), pawn);
				}
			}
			return null;
		}

	}
}
