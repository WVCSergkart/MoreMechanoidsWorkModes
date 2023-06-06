// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace WVC
{
	public class JobGiver_GoToClosetShutdownSpot : ThinkNode_JobGiver
	{
		public ThingDef spotDefName;
		public ThingDef backupSpotDefName;
		public int minDistanceForSpot = 9;
		public int maxDistanceForSpot = 9999;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableShutdownSearching == true)
			{
				Building spotIsClose = GetClosestShutdownSpot(pawn, spotDefName, minDistanceForSpot);
				if (spotIsClose != null)
				{
					return null;
				}
				Building spotIsFar = GetClosestShutdownSpot(pawn, spotDefName, maxDistanceForSpot);
				if (spotIsFar != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Goto, spotIsFar);
					return job;
				}
				Building backupSpotIsClose = GetClosestShutdownSpot(pawn, backupSpotDefName, minDistanceForSpot);
				if (backupSpotIsClose != null)
				{
					return null;
				}
				Building backupSpotIsFar = GetClosestShutdownSpot(pawn, backupSpotDefName, maxDistanceForSpot);
				if (backupSpotIsFar != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Goto, backupSpotIsFar);
					return job;
				}
			}
			return null;
		}

		// public Building GetCloseShutdownSpot(Pawn mech)
        // {
			// return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), minDistanceForSpot, delegate (Thing t)
			// {
				// return !t.IsForbidden(mech);
			// });
		// }

        // public Building GetClosestShutdownSpot(Pawn mech)
		// {
			// return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistanceForSpot, delegate (Thing t)
			// {
				// return !t.IsForbidden(mech);
			// });
		// }

		// public Building GetCloseBackupShutdownSpot(Pawn mech)
		// {
			// return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(backupSpotDefName), PathEndMode.OnCell, TraverseParms.For(mech), minDistanceForSpot, delegate (Thing t)
			// {
				// return !t.IsForbidden(mech);
			// });
		// }

		// public Building GetClosestBackupShutdownSpot(Pawn mech)
		// {
			// return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(backupSpotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistanceForSpot, delegate (Thing t)
			// {
				// return !t.IsForbidden(mech);
			// });
		// }

		public Building GetClosestShutdownSpot(Pawn mech, ThingDef spotDefName, int maxDistance)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistance, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}
	}
}
