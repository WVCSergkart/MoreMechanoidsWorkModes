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

		public MechanoidWorkType workModeType = 0;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableShutdownSearching)
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
			return null;
		}
	}

}
