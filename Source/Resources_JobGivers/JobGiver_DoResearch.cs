// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using WVC;

namespace WVC_WorkModes
{
	public class JobGiver_MechDoResearch : ThinkNode_JobGiver
	{

		public float researchFactor = 1f;

		private float? cachedBandwidthCost;

		// public override ThinkNode DeepCopy(bool resolve = true)
		// {
			// JobGiver_MechDoResearch obj = (JobGiver_MechDoResearch)base.DeepCopy(resolve);
			// obj.researchFactor = researchFactor;
			// return obj;
		// }

		protected override Job TryGiveJob(Pawn pawn)
		{
			ResearchManager researchManager = Find.ResearchManager;
			ResearchProjectDef currentProject = researchManager.currentProj;
			if (currentProject != null)
			{
				researchManager.ResearchPerformed(1f + (BandwidthCost(pawn) * researchFactor), pawn);
			}
			return null;
		}

		public float BandwidthCost(Pawn pawn)
		{
			if (!cachedBandwidthCost.HasValue)
			{
				if (!ModsConfig.BiotechActive)
				{
					cachedBandwidthCost = 0f;
				}
				else
				{
					cachedBandwidthCost = pawn.GetStatValue(StatDefOf.BandwidthCost);
				}
			}
			return cachedBandwidthCost.Value;
		}
	}
}
