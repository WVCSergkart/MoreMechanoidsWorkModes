// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class JobGiver_GoToShutdownZone : ThinkNode_JobGiver
	{

		public ThingDef spotDefName;
		public ThingDef backupSpotDefName;
		public int minDistanceForSpot = 9;
		public int maxDistanceForSpot = 9999;

		public MechanoidWorkType workModeType = 0;

		// public override ThinkNode DeepCopy(bool resolve = true)
		// {
			// JobGiver_GoToShutdownZone obj = (JobGiver_GoToShutdownZone)base.DeepCopy(resolve);
			// obj.workModeType = workModeType;
			// obj.spotDefName = spotDefName;
			// obj.backupSpotDefName = backupSpotDefName;
			// obj.minDistanceForSpot = minDistanceForSpot;
			// obj.maxDistanceForSpot = maxDistanceForSpot;
			// return obj;
		// }

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableShutdownSearching)
			{
				if (WVC_MMWM.settings.shutdownModeZonesOrSpots)
				{
					if (!pawn.Map.IsPlayerHome)
					{
						return null;
					}
					List<Zone> mapZones = pawn.Map.zoneManager.AllZones;
					if (mapZones.NullOrEmpty() || !ShutdownUtility.AnyMechanoidZone(mapZones))
					{
						return null;
					}
					if (ShutdownUtility.TryFindNearbyMechShutdownZone(mapZones, pawn, pawn.Map, workModeType, out var result))
					{
						if (pawn.Position == result)
						{
							return null;
						}
						Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
						// job.checkOverrideOnExpire = true;
						// job.expiryInterval = 250;
						return job;
					}
				}
				else
				{
					if (spotDefName != null)
					{
						Building spotIsClose = ShutdownUtility.GetClosestShutdownSpot(pawn, spotDefName, minDistanceForSpot);
						if (spotIsClose != null)
						{
							return null;
						}
						Building spotIsFar = ShutdownUtility.GetClosestShutdownSpot(pawn, spotDefName, maxDistanceForSpot);
						if (spotIsFar != null)
						{
							Job job = JobMaker.MakeJob(JobDefOf.Goto, spotIsFar);
							return job;
						}
					}
					if (backupSpotDefName != null)
					{
						Building backupSpotIsClose = ShutdownUtility.GetClosestShutdownSpot(pawn, backupSpotDefName, minDistanceForSpot);
						if (backupSpotIsClose != null)
						{
							return null;
						}
						Building backupSpotIsFar = ShutdownUtility.GetClosestShutdownSpot(pawn, backupSpotDefName, maxDistanceForSpot);
						if (backupSpotIsFar != null)
						{
							Job job = JobMaker.MakeJob(JobDefOf.Goto, backupSpotIsFar);
							return job;
						}
					}
				}
			}
			return null;
		}
	}

}
