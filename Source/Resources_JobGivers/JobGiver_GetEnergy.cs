// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class JobGiver_GetEnergy_SelfShutdown : JobGiver_GetEnergy
	{
		public int tickInterval = 3000;
		protected override Job TryGiveJob(Pawn pawn)
		{
			//if (!ShouldAutoRecharge(pawn))
			//{
			//	return null;
			//}
			if (WVC_MMWM.settings.useCustomShutdownBehavior)
			{
				if (ShutdownUtility.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
				{
					Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			else
			{
				// Vanilla
				if (RCellFinder.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
				{
					Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			return null;
		}
	}

	public class JobGiver_GetEnergy_Charger : JobGiver_GetEnergy
	{
		public int tickInterval = 625;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (ShutdownUtility.TryGetEnergy(pawn, out Need_MechEnergy energy))
			{
				if (ShouldChargeNow(pawn))
				{
					return TryGetChargeJob(pawn, energy);
				}
				return null;
			}
			return null;
		}

		private Job TryGetChargeJob(Pawn pawn, Need_MechEnergy energy)
		{
			float maxRechargeLimit = GetMaxRechargeLimit(pawn);
			if (energy.CurLevel + 0.1f < maxRechargeLimit - 5f)
			{
				Building_MechCharger closestCharger = RimWorld.JobGiver_GetEnergy_Charger.GetClosestCharger(pawn, pawn, forced: false);
				if (closestCharger != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.MechCharge, closestCharger);
					job.overrideFacing = Rot4.South;
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			else if (energy.CurLevel + 2f < maxRechargeLimit)
			{
				energy.CurLevel += 2f;
			}
			return null;
		}

		// Hook
		private bool ShouldChargeNow(Pawn caller)
		{
			return AnyMechWithLowEnergyOnSameMap(caller);
		}

		private static int checkDelayTick = -1;
		private static bool? cachedResult;

		public static void ResetCache()
		{
			checkDelayTick = -1;
			cachedResult = null;
		}

		public static bool AnyMechWithLowEnergyOnSameMap(Pawn caller)
		{
			if (!WVC_MMWM.settings.enableOpportunisticChargers)
			{
				return true;
			}
			if (cachedResult == null || checkDelayTick < Find.TickManager.TicksGame)
			{
				cachedResult = true;
				foreach (Pawn mech in caller.Map.mapPawns.AllPawnsSpawned)
				{
					if (mech.IsColonyMech && mech.TryGetEnergy(out Need_MechEnergy energy))
					{
						int minThreshold = GetMinAutorechargeThreshold(mech);
						if (minThreshold > energy.CurLevel)
						{
							cachedResult = false;
							break;
						}
					}
				}
				checkDelayTick = Find.TickManager.TicksGame + 5000;
			}
			return cachedResult.Value;
		}

	}
}
