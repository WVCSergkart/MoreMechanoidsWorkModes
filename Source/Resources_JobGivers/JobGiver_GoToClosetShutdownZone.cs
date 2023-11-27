// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using WVC;
using System.Linq;

namespace WVC_WorkModes
{
	public class JobGiver_GoToShutdownZone : ThinkNode_JobGiver
	{

		public ThingDef spotDefName;
		public ThingDef backupSpotDefName;
		public int minDistanceForSpot = 9;
		public int maxDistanceForSpot = 9999;

		public MechanoidWorkType workModeType = 0;

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
