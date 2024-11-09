// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
	public class JobGiver_GoToShutdownZone : ThinkNode_JobGiver
	{

		[Obsolete]
		public string spotDefName;
		[Obsolete]
		public string backupSpotDefName;
		[Obsolete]
		public int minDistanceForSpot = 9;
		[Obsolete]
		public int maxDistanceForSpot = 9999;

		public MechanoidWorkType workModeType = 0;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enable_GoToShutdownZoneJob)
			{
				//if (!pawn.Map.IsPlayerHome)
				//{
				//	return null;
				//}
				List<Zone> mapZones = pawn.Map?.zoneManager?.AllZones;
				if (mapZones.NullOrEmpty() || !ShutdownUtility.AnyMechanoidZone(mapZones))
				{
					return null;
				}
				if (ShutdownUtility.MechInShutdownZone(pawn, pawn.Position, workModeType))
				{
					return null;
				}
				if (ShutdownUtility.TryFindRandomMechShutdownZone(mapZones, pawn, pawn.Map, workModeType, out var result))
				{
					if (pawn.Position == result)
					{
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
					return job;
				}
			}
			return null;
		}
	}

}
