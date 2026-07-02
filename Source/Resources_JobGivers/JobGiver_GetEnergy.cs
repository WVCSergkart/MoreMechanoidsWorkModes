// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace WVC_WorkModes
{
    public class JobGiver_GetEnergy_SelfShutdown : JobGiver_GetEnergy
	{
		public int tickInterval = 3000;
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (ShutdownUtility.CanSelfShutdown(pawn.Position, pawn, pawn.Map, false))
			{
				return ShutdownJob(pawn.Position);
			}
			if (ShutdownUtility.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
			{
				return ShutdownJob(result);
			}
			//if (WVC_MMWM.settings.useCustomShutdownBehavior)
			//{
			//	if (ShutdownUtility.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
			//	{
			//		return ShutdownJob(result);
			//	}
			//}
			//else
			//{
			//	// Vanilla
			//	if (RCellFinder.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
			//	{
			//		return ShutdownJob(result);
			//	}
			//}
			return null;
		}

		private Job ShutdownJob(IntVec3 result)
		{
			Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
			job.checkOverrideOnExpire = true;
			//job.overrideFacing = Rot4.South;
			job.expiryInterval = tickInterval;
			return job;
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

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (!ThinkNode_CanShutdown.CanShutdown(pawn))
			{
				return ThinkResult.NoJob;
			}
			ThinkResult thinkResult = base.TryIssueJobPackage(pawn, jobParams);
			if (thinkResult == ThinkResult.NoJob)
			{
				foreach (ThinkNode subNode in subNodes)
				{
					ThinkResult result = ThinkResult.NoJob;
					try
					{
						result = subNode.TryIssueJobPackage(pawn, jobParams);
					}
					catch (Exception ex)
					{
						Log.Error("Exception in " + GetType()?.ToString() + " TryIssueJobPackage: " + ex.ToString());
					}
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return thinkResult;
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
