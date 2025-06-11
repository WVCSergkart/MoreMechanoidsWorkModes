// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

    public class JobGiver_GoToScavengeZone : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{
			// if (!pawn.Map.IsPlayerHome)
			// {
				// return null;
			// }
			List<Zone> mapZones = pawn.Map.zoneManager.AllZones;
			// if (mapZones.NullOrEmpty() || !ScavengeUtility.AnyScavengeZone(mapZones))
			// {
				// return null;
			// }
			if (ScavengeUtility.MechInScavengeZone(pawn, pawn.Position))
			{
				return null;
			}
			if (ScavengeUtility.TryFindFirstMechScavengeZone(mapZones, pawn, out var result))
			{
				if (pawn.Position == result)
				{
					return null;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
				return job;
			}
			return null;
		}

	}

	public class JobGiver_WanderScavenge : JobGiver_Wander
	{
		public JobGiver_WanderScavenge()
		{
			wanderRadius = 7f;
			ticksBetweenWandersRange = new IntRange(125, 200);
			wanderDestValidator = (Pawn pawn, IntVec3 loc, IntVec3 root) => true;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return ScavengeUtility.GetScavengeWanderRoot(pawn);
		}
	}

}
