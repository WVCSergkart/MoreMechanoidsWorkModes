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

		protected override Job TryGiveJob(Pawn pawn)
		{
			// Mech hive mind is researching.
			ResearchManager researchManager = Find.ResearchManager;
			ResearchProjectDef currentProject = researchManager.currentProj;
			if (currentProject != null)
			{
				// researchAmount += researchManager.ResearchPointsPerWorkTick * Find.Storyteller.difficulty.researchSpeedFactor * pawn.GetStatValue(StatDefOf.BandwidthCost) / currentProject.CostFactor(pawn.Faction.def.techLevel);
				// pawn?.records.AddTo(RecordDefOf.ResearchPointsResearched, researchAmount);
				// float progress = GetProgress(currentProject);
				// progress += researchAmount;
				researchManager.ResearchPerformed(pawn.GetStatValue(StatDefOf.BandwidthCost) * researchFactor, pawn);
			}
			return null;
		}

		public Building GetClosestShutdownSpot(Pawn mech, ThingDef spotDefName, int maxDistance)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistance, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}
	}
}
